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
///             "id": "123e4567-e89b-12d3-a456-426614174000",
///             "title": "Affaldshierakiet",
///             "description": "Tryk på ét trin. De bedste løsniger er øverst",
///             "topics": [
///                 {
///                     "id": "123e4567-e89b-12d3-a456-426614174000",
///                     "title":"Forebyggelse",
///                     "description":"Undgå at skabe affald i første omgang",
///                     "body":"Eksempel: Tag en drikkedunk med i stedet for at købe en ny flaske",
///                     "calloutLabel":"Otto siger:",
///                     "calloutText":"Det bedste affald er det, vi slet ikke skaber",
///                     "qoute":"Otto anbefaler: måske kan en andens gamle ting blive din nye favorit",
///                 }
///             ]
///         }
///     ]
/// }
/// </example>
public record UpdateExhibitionRequest(
    string ExhibitionName,
    List<UpdateSubjectRequest> Subjects
    )
{
    /// <summary>
    /// The name of the exhibition.
    /// </summary>
    public string ExhibitionName { get; init; } = ExhibitionName;

    /// <summary>
    /// A list of the subjects taht you want to update.
    /// </summary>
    public List<UpdateSubjectRequest> Subjects { get; init; } = Subjects;
}
