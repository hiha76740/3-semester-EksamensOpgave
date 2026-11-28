using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Text.Json;
using VidensVejen.ContractsLib.Authentication;
using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.Web.Models.Exhibitions;
using VidensVejen.Web.Models.Games;

namespace VidensVejen.Web.Controls.Exhibitions;

/// <summary>Coordinates the shared exhibitions workflow for its dedicated pages.</summary>
public partial class ExhibitionEditorComponent
{
    /// <summary>Gets or sets the identifier supplied by the page route.</summary>
    [Parameter]
    public Guid? ExistingDraftId { get; set; }

    /// <summary>Gets or sets the identifier supplied by the page route.</summary>
    [Parameter]
    public Guid? PublishedExhibitionId { get; set; }

    private (Guid? DraftId, Guid? PublishedId)? _loadedRoute;

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    private bool _accessDenied = true;
    private bool _canEditExhibition;
    private bool _canPublishExhibition;
    private bool _canPublishQuiz;
    private bool _canEditQuiz;
    private ExhibitionInputModel? _model;
    private Guid? _draftId;
    private Guid? _sourceExhibitionId;
    private IBrowserFile? _uploadedThemeFile;
    private Guid? _uploadedThemeId;
    private bool _isBusy;
    private string? _error;
    private string? _message;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        var route = (ExistingDraftId, PublishedExhibitionId);
        if (_loadedRoute == route)
        {
            return;
        }

        var authentication = AuthenticationStateTask;
        if (authentication != null)
        {
            var user = (await authentication).User;
            _canEditExhibition = user.Identity?.IsAuthenticated == true
                && user.HasClaim(CustomClaimTypes.Permission, "Exhibition.Edit");
            _canPublishExhibition = user.Identity?.IsAuthenticated == true
                && user.HasClaim(CustomClaimTypes.Permission, "Exhibition.Publish");
            _canEditQuiz = user.Identity?.IsAuthenticated == true
                && user.HasClaim(CustomClaimTypes.Permission, "Game.Edit");
            _canPublishQuiz = user.Identity?.IsAuthenticated == true
                && user.HasClaim(CustomClaimTypes.Permission, "Game.Publish");
        }

        _accessDenied = !_canEditExhibition && !(_canPublishExhibition && ExistingDraftId.HasValue);
        if (_accessDenied)
        {
            return;
        }

        _loadedRoute = route;
        _model = null;
        _error = null;
        _message = null;
        _draftId = null;
        _sourceExhibitionId = null;
        _uploadedThemeFile = null;
        _uploadedThemeId = null;
        if (ExistingDraftId.HasValue)
        {
            await LoadDraftAsync();
        }
        else if (PublishedExhibitionId.HasValue)
        {
            await LoadPublishedAsync();
        }
        else
        {
            await StartDraftAsync();
        }
    }

    private Task LoadPublishedAsync()
    {
        return RunAsync(async () =>
    {
        if (!_canEditExhibition || PublishedExhibitionId is not Guid id)
        {
            return;
        }

        var exhibition = await ExhibitionService.GetPublishedAsync(id);
        var subjects = exhibition.Subjects.Select(saved =>
        {
            var subject = new ExhibitionSubjectInputModel
            {
                Title = saved.Title,
                TitleEn = saved.TitleEn ?? string.Empty,
                Slug = saved.Slug,
                Description = saved.Description,
                DescriptionEn = saved.DescriptionEn ?? string.Empty,
                IsQuiz = saved.QuizId.HasValue,
                QuizId = saved.QuizId
            };
            subject.Topics.AddRange(saved.Topics.Select(topic => new ExhibitionTopicInputModel
            {
                Title = topic.Title,
                TitleEn = topic.TitleEn ?? string.Empty,
                Description = topic.Description,
                DescriptionEn = topic.DescriptionEn ?? string.Empty,
                Body = topic.Body,
                BodyEn = topic.BodyEn ?? string.Empty,
                CalloutLabel = topic.CalloutLabel,
                CalloutLabelEn = topic.CalloutLabelEn ?? string.Empty,
                CalloutText = topic.CalloutText,
                CalloutTextEn = topic.CalloutTextEn ?? string.Empty,
                Quote = topic.Quote ?? string.Empty,
                QuoteEn = topic.QuoteEn ?? string.Empty
            }));
            return subject;
        }).ToList();

        _sourceExhibitionId = exhibition.Id;
        _uploadedThemeId = exhibition.ThemeImageId;
        _model = new ExhibitionInputModel
        {
            ExhibitionName = exhibition.Title,
            ExhibitionNameEn = exhibition.TitleEn ?? string.Empty,
            RowVersion = exhibition.RowVersion,
            Subjects = subjects
        };
    }, "Udstillingen kunne ikke hentes.");
    }

    private async Task StartDraftAsync()
    {
        if (!_canEditExhibition)
        {
            return;
        }

        Guid? newId = null;
        await RunAsync(async () =>
        {
            newId = await ExhibitionService.CreateDraftAsync();
        }, "Kladden kunne ikke oprettes.");
        if (newId is Guid id)
        {
            NavigationManager.NavigateTo($"exhibition/drafts/{id:D}", replace: true);
        }
    }

    private Task LoadDraftAsync()
    {
        return RunAsync(async () =>
    {
        var id = ExistingDraftId.GetValueOrDefault();
        var draft = await ExhibitionService.GetDraftAsync(id);
        var subjects = new List<ExhibitionSubjectInputModel>();
        foreach (var saved in draft.SubjectDrafts)
        {
            var subject = new ExhibitionSubjectInputModel
            {
                Title = saved.Title ?? string.Empty,
                TitleEn = saved.TitleEn ?? string.Empty,
                Slug = saved.Slug,
                Description = saved.Description ?? string.Empty,
                DescriptionEn = saved.DescriptionEn ?? string.Empty,
                IsQuiz = saved.IsQuiz,
                QuizId = saved.QuizId,
                QuizDraftId = saved.QuizDraftId
            };

            subject.Topics.AddRange(saved.TopicDrafts.Select(topic => new ExhibitionTopicInputModel
            {
                Title = topic.Title ?? string.Empty,
                TitleEn = topic.TitleEn ?? string.Empty,
                Description = topic.Description ?? string.Empty,
                DescriptionEn = topic.DescriptionEn ?? string.Empty,
                Body = topic.Body ?? string.Empty,
                BodyEn = topic.BodyEn ?? string.Empty,
                CalloutLabel = topic.CalloutLabel ?? string.Empty,
                CalloutLabelEn = topic.CalloutLabelEn ?? string.Empty,
                CalloutText = topic.CalloutText ?? string.Empty,
                CalloutTextEn = topic.CalloutTextEn ?? string.Empty,
                Quote = topic.Quote ?? string.Empty,
                QuoteEn = topic.QuoteEn ?? string.Empty
            }));

            if (subject.IsQuiz && subject.QuizDraftId is Guid quizDraftId
                && await FetchQuizDraftAsync(quizDraftId) == null)
            {
                subject.QuizDraftId = null;
            }

            subjects.Add(subject);
        }

        while (!draft.SourceExhibitionId.HasValue && subjects.Count < 4)
        {
            subjects.Add(new ExhibitionSubjectInputModel());
        }

        _uploadedThemeId = draft.ThemeImageId;
        _draftId = id;
        _sourceExhibitionId = draft.SourceExhibitionId;
        _model = new ExhibitionInputModel
        {
            ExhibitionName = draft.Name ?? string.Empty,
            ExhibitionNameEn = draft.NameEn ?? string.Empty,
            Subjects = subjects,
            RowVersion = draft.RowVersion
        };
    }, "Kladden kunne ikke hentes.");
    }

    private async Task<QuizDraftResponse?> FetchQuizDraftAsync(Guid id)
    {
        try
        {
            return await QuizService.GetDraftAsync(id);
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task SaveDraftAsync(ExhibitionInputModel input)
    {
        if (!_canEditExhibition)
        {
            return;
        }

        Guid? savedId = null;
        await RunAsync(async () =>
        {
            var imageId = await ResolveThemeIdAsync(input.ThemeFile);
            savedId = await SaveDraftContentAsync(input, imageId);
            _message = "Kladden er gemt.";
        }, "Kladden kunne ikke gemmes.");

        if (PublishedExhibitionId.HasValue && savedId is Guid id)
        {
            _loadedRoute = (id, null);
            NavigationManager.NavigateTo($"exhibition/drafts/{id:D}");
        }
    }

    private async Task<Guid> SaveDraftContentAsync(ExhibitionInputModel input, Guid? imageId)
    {
        var request = ExhibitionEditorMapper.BuildDraftRequest(input, imageId, _sourceExhibitionId.HasValue);
        if (_draftId is Guid id)
        {
            input.RowVersion = await ExhibitionService.SaveDraftAsync(id, request);
            return id;
        }

        if (_sourceExhibitionId is not Guid sourceId)
        {
            throw new InvalidOperationException("The source exhibition is missing.");
        }

        var created = await ExhibitionService.CreateRevisionDraftAsync(sourceId, request);
        _draftId = created.Id;
        input.RowVersion = created.RowVersion;
        return created.Id;
    }

    private Task Create(ExhibitionInputModel input)
    {
        return RunAsync(async () =>
    {
        if (!_canPublishExhibition || (!_draftId.HasValue && !_sourceExhibitionId.HasValue))
        {
            return;
        }

        _error = ValidatePublication(input);
        if (_error != null)
        {
            return;
        }

        var imageId = await ResolveThemeIdAsync(input.ThemeFile);
        if (!imageId.HasValue)
        {
            _error = "Vælg et temabillede.";
            return;
        }

        var quizzes = new List<(ExhibitionSubjectInputModel Subject, Guid DraftId, QuizInputModel Quiz)>();
        foreach (var subject in input.Subjects)
        {
            if (!subject.IsQuiz || subject.QuizDraftId is not Guid quizDraftId)
            {
                continue;
            }

            if (!_canPublishQuiz)
            {
                _error = "Du skal have Game.Publish for at publicere quizkladderne.";
                return;
            }

            var draft = await FetchQuizDraftAsync(quizDraftId);
            if (draft == null)
            {
                subject.QuizDraftId = null;
                _error = $"Quizkladden til {subject.Title} findes ikke længere. Vælg eller opret en quiz.";
                return;
            }

            var quiz = QuizEditorMapper.FromDraft(draft);
            quiz.RowVersion = draft.RowVersion;
            quiz.ExhibitionId = _sourceExhibitionId ?? _draftId.GetValueOrDefault();
            var quizErrors = new List<string>();
            QuizEditorMapper.ValidatePublication(quiz, quizErrors);
            if (quizErrors.Count > 0)
            {
                _error = $"Quiz til {subject.Title}: {string.Join(" ", quizErrors)}";
                return;
            }

            quizzes.Add((subject, quizDraftId, quiz));
        }

        var id = _canEditExhibition
            ? await SaveDraftContentAsync(input, imageId)
            : _draftId.GetValueOrDefault();
        foreach (var (subject, quizDraftId, quiz) in quizzes)
        {
            var response = await QuizService.PublishAsync(quizDraftId, QuizEditorMapper.BuildRequest(quizDraftId, quiz));
            subject.QuizId = response.QuizId;
            subject.QuizDraftId = null;
            if (_canEditExhibition)
            {
                input.RowVersion = await ExhibitionService.SaveDraftAsync(id, ExhibitionEditorMapper.BuildDraftRequest(input, imageId, _sourceExhibitionId.HasValue));
            }
        }

        var request = ExhibitionEditorMapper.BuildPublicationRequest(id, input, imageId.Value, _sourceExhibitionId.HasValue);

        if (_sourceExhibitionId.HasValue)
        {
            await ExhibitionService.PublishRevisionAsync(id, request);
        }
        else if (await ExhibitionService.CreateAsync(request) != HttpStatusCode.Created)
        {
            _error = "Udstillingen kunne ikke publiceres.";
            return;
        }

        await Dialog.Alert("Udstillingen er publiceret.", "Publicér udstilling");
        NavigationManager.NavigateTo("/exhibitions");
    }, "Publiceringen kunne ikke bekræftes. Kontrollér udstillingslisten, før du prøver igen.");
    }

    private async Task<Guid?> ResolveThemeIdAsync(IBrowserFile? file)
    {
        if (file == null)
        {
            return _uploadedThemeId;
        }

        if (ReferenceEquals(file, _uploadedThemeFile))
        {
            return _uploadedThemeId;
        }

        var image = await ImageService.UploadImage(file, ExhibitionInputModel.MaxThemeFileSize);
        _uploadedThemeFile = file;
        _uploadedThemeId = image.ImageId;
        return _uploadedThemeId;
    }

    private async Task RunAsync(Func<Task> action, string failureMessage)
    {
        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        _error = null;
        _message = null;
        try
        {
            await action();
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException
            or InvalidOperationException or IOException or OperationCanceledException)
        {
            _error = failureMessage;
        }
        finally
        {
            _isBusy = false;
        }
    }

    private string? ValidatePublication(ExhibitionInputModel input)
    {
        if (string.IsNullOrWhiteSpace(input.ExhibitionName) || string.IsNullOrWhiteSpace(input.ExhibitionNameEn))
        {
            return "Udfyld udstillingsnavnet på dansk og engelsk.";
        }

        if (input.ThemeFile == null && !_uploadedThemeId.HasValue)
        {
            return "Vælg et temabillede.";
        }

        var selectedQuizIds = new HashSet<Guid>();
        var selectedDraftIds = new HashSet<Guid>();
        for (var i = 0; i < input.Subjects.Count; i++)
        {
            var subject = input.Subjects[i];
            if (string.IsNullOrWhiteSpace(subject.Title) || string.IsNullOrWhiteSpace(subject.TitleEn)
                || string.IsNullOrWhiteSpace(subject.Description) || string.IsNullOrWhiteSpace(subject.DescriptionEn))
            {
                return $"Udfyld titel og beskrivelse på begge sprog for subject {i + 1}.";
            }

            if (subject.IsQuiz)
            {
                if (!subject.QuizId.HasValue && !subject.QuizDraftId.HasValue)
                {
                    return $"Vælg eller opret en quiz til subject {i + 1}.";
                }

                if ((subject.QuizId is Guid quizId && !selectedQuizIds.Add(quizId))
                    || (subject.QuizDraftId is Guid quizDraftId && !selectedDraftIds.Add(quizDraftId)))
                {
                    return $"Vælg en anden quiz til subject {i + 1}. Quizzen er allerede valgt på et andet subject.";
                }

                continue;
            }

            if (subject.Topics.Count == 0)
            {
                return $"Tilføj mindst ét emne til subject {i + 1}.";
            }

            if (subject.Topics.Any(topic => new[]
                {
                    topic.Title, topic.TitleEn, topic.Description, topic.DescriptionEn, topic.Body,
                    topic.BodyEn, topic.CalloutLabel, topic.CalloutLabelEn, topic.CalloutText, topic.CalloutTextEn
                }.Any(string.IsNullOrWhiteSpace)))
            {
                return $"Udfyld titel, beskrivelse, brødtekst og fremhævet boks på begge sprog for emnerne i subject {i + 1}.";
            }
        }

        return null;
    }
}
