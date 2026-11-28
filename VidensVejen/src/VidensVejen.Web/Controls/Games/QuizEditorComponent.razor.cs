using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;
using VidensVejen.ContractsLib.Authentication;
using VidensVejen.ContractsLib.Responses.ExhibitionService;
using VidensVejen.Web.Models.Games;

namespace VidensVejen.Web.Controls.Games;

/// <summary>Coordinates the shared quizzes workflow for its dedicated pages.</summary>
public partial class QuizEditorComponent
{
    /// <summary>Gets or sets the identifier supplied by the page route.</summary>
    [Parameter]
    public Guid? ExistingDraftId { get; set; }
    /// <summary>Gets or sets the identifier supplied by the page route.</summary>
    [Parameter]
    public Guid? PublishedQuizId { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    private bool _canEdit;
    private bool _canPublish;
    private bool _accessDenied;
    private string FormTitle => PublishedQuizId.HasValue ? "Ny kladde fra publiceret quiz"
        : ExistingDraftId.HasValue ? "Quizkladde" : "Opret quiz";

    private string ResetText => ExistingDraftId.HasValue || PublishedQuizId.HasValue ? "Luk" : "Nulstil";

    private QuizInputModel? _model;
    private Guid? _draftId;
    private bool _isBusy;
    private bool _draftVersionNeedsReload;
    private readonly List<string> _errors = [];
    private string? _message;

    private List<ExhibitionNameResponse> _exhibitions = [];
    private string? _exhibitionsError;

    private (Guid? DraftId, Guid? PublishedId)? _loadedRoute;

    /// <summary>
    /// Initializes the quiz editor when its route parameters change.
    /// </summary>
    /// <remarks>Loads the accessible draft or published quiz, determines the user's editing and publishing
    /// permissions, and loads the available exhibitions. Repeated calls for the same route parameters return without
    /// reloading.</remarks>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        var route = (ExistingDraftId, PublishedQuizId);
        if (_loadedRoute == route)
        {
            return;
        }

        _model = null;
        _draftId = null;
        _draftVersionNeedsReload = false;
        _exhibitionsError = null;
        ClearMessages();

        var authentication = AuthenticationStateTask;
        if (authentication == null)
        {
            _accessDenied = true;
            return;
        }

        var user = (await authentication).User;
        _canEdit = user.Identity?.IsAuthenticated == true && user.HasClaim(CustomClaimTypes.Permission, "Game.Edit");
        _canPublish = user.Identity?.IsAuthenticated == true && user.HasClaim(CustomClaimTypes.Permission, "Game.Publish");
        _accessDenied = !_canEdit && !(_canPublish && ExistingDraftId.HasValue);
        if (_accessDenied)
        {
            return;
        }

        _loadedRoute = route;

        if (ExistingDraftId is Guid id)
        {
            await RunAsync(async () =>
            {
                var draft = await QuizService.GetDraftAsync(id);
                _draftId = draft.Id;
                _model = QuizEditorMapper.FromDraft(draft);
                _model.RowVersion = draft.RowVersion;
            }, "Quizkladden kunne ikke hentes.");
        }
        else if (PublishedQuizId is Guid quizId)
        {
            await RunAsync(async () =>
            {
                _model = QuizEditorMapper.FromPublished(await QuizService.GetPublishedAsync(quizId));
                _message = "Gem en kladde for at arbejde videre med en ny version af quizzen.";
            }, "Quizzen kunne ikke hentes.");
        }
        else
        {
            await StartDraftAsync();
        }

        try
        {
            var options = await ExhibitionService.GetAllNamesAsync();

            _exhibitions = options.Where(option => option.Id != Guid.Empty).ToList();
            if (options.Count > 0 && _exhibitions.Count == 0)
            {
                _exhibitionsError = "Udstillingslisten mangler udstillingernes id'er.";
            }
            else if (_exhibitions.Count == 0)
            {
                _exhibitionsError = "Der er ingen udstillinger at vælge.";
            }
        }
        catch (HttpRequestException)
        {
            _exhibitionsError = "Udstillingerne kunne ikke hentes.";
        }
        catch (JsonException)
        {
            _exhibitionsError = "Udstillingslisten kunne ikke læses.";
        }
    }

    private Task StartDraftAsync()
    {
        return RunAsync(async () =>
    {
        if (!_canEdit)
        {
            return;
        }

        var input = new QuizInputModel();
        await EnsureDraftAsync(input);
        _model = input;
    }, "Quizkladden kunne ikke oprettes.");
    }

    private Task SaveDraftAsync(QuizInputModel input)
    {
        return RunAsync(async () =>
    {
        if (!_canEdit)
        {
            return;
        }

        if (!await EnsureDraftAsync(input))
        {
            return;
        }

        var id = _draftId.GetValueOrDefault();
        await QuizService.SaveDraftAsync(id, QuizEditorMapper.BuildDraftRequest(input));

        _draftVersionNeedsReload = true;
        _message = "Quizkladden er gemt.";
        await LoadDraftVersionAsync(id, input);
        _draftVersionNeedsReload = false;
    }, "Quizkladden kunne ikke gemmes.");
    }

    private Task Create(QuizInputModel input)
    {
        return RunAsync(async () =>
    {
        if (!_canPublish)
        {
            return;
        }

        QuizEditorMapper.ValidatePublication(input, _errors);

        if (_errors.Count != 0)
        {
            return;
        }

        if (!await EnsureDraftAsync(input))
        {
            return;
        }

        var id = _draftId.GetValueOrDefault();
        await QuizService.PublishAsync(id, QuizEditorMapper.BuildRequest(id, input));
        _draftId = null;
        _model = null;
        _message = "Quizzen er publiceret.";
    }, "Publiceringen kunne ikke bekræftes.");
    }

    private async Task<bool> EnsureDraftAsync(QuizInputModel input)
    {
        if (_draftVersionNeedsReload)
        {
            _errors.Add("Åbn quizkladden igen fra oversigten for at fortsætte.");
            return false;
        }

        var id = _draftId ??= await QuizService.CreateDraftAsync();
        if (input.RowVersion.Length == 0)
        {
            await LoadDraftVersionAsync(id, input);
        }

        return true;
    }

    private async Task LoadDraftVersionAsync(Guid id, QuizInputModel input)
    {
        var draft = await QuizService.GetDraftAsync(id);
        if (draft.RowVersion.Length == 0)
        {
            throw new JsonException("The quiz draft response is missing its row version.");
        }

        input.RowVersion = draft.RowVersion;
    }

    private async Task RunAsync(Func<Task> action, string failureMessage)
    {
        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        ClearMessages();
        try
        {
            await action();
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException
            or OperationCanceledException)
        {
            if (_draftVersionNeedsReload)
            {
                _errors.Add("Kladdens nye version kunne ikke hentes. Åbn kladden igen fra oversigten.");
            }
            else
            {
                _errors.Add(failureMessage);
            }
        }
        finally
        {
            _isBusy = false;
        }
    }

    private void ResetForm()
    {
        if (ExistingDraftId.HasValue || PublishedQuizId.HasValue)
        {
            Navigation.NavigateTo("games");
            return;
        }

        _model = new QuizInputModel { RowVersion = _model?.RowVersion ?? [] };
        ClearMessages();
    }

    private void ClearMessages()
    {
        _message = null;
        _errors.Clear();
    }
}
