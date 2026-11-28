using Radzen;

namespace VidensVejen.Web.Helpers;

internal static class DeleteConfirmation
{
    internal static async Task<bool> ConfirmAsync(DialogService dialog, string message, string title)
    {
        return await dialog.Confirm(message, title,
            new ConfirmOptions { OkButtonText = "Slet", CancelButtonText = "Annullér" }) == true;
    }
}
