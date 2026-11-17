namespace VidensVejen.ContractsLib.Requests.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="ExhibitionName"></param>
/// <param name="Subjects"></param>
/// <example>
/// {
///     "exhibitionName": "Vand og hav",
///     "subjects": [
///         {
///             "title": "Affaldshierakiet",
///             "description": "Tryk på ét trin. De bedste løsniger er øverst",
///             "topics": [
///                 {
///                     "title":"Forebyggelse",
///                     "description":"Undgå at skabe affald i første omgang",
///                     "body":"Eksempel: Tag en drikkedunk med i stedet for at købe en ny flaske",
///                     "calloutLabel":"Otto siger:",
///                     "calloutText":"Det bedste affald er det, vi slet ikke skaber",
///                     "qoute":"Otto anbefaler: måske kan en andens gamle ting blive din nye favorit"
///                 }
///             ]
///         }
///     ]
/// }
/// </example>
public record CreateExhibitionRequest(
    string ExhibitionName,
    List<CreateSubjectRequest> Subjects
    )
{
    /// <summary>
    /// The name of the exhibition.
    /// </summary>
    public string ExhibitionName { get; init; } = ExhibitionName;

    /// <summary>
    /// List of the subjects objects you want exhibition to have
    /// </summary>
    public List<CreateSubjectRequest> Subjects { get; init; } = Subjects;
}
