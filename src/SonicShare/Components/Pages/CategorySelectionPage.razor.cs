using SonicShare.Models;

namespace SonicShare.Components.Pages;

public partial class CategorySelectionPage
{
    public static readonly List<SendCategoryItem> CategoryItems = new()
    {
        new SendCategoryItem("Contacts", "people-outline", SendCategory.Contacts),
        new SendCategoryItem("Images", "image-outline", SendCategory.Images),
        new SendCategoryItem("Music", "musical-notes-outline", SendCategory.Music),
        new SendCategoryItem("Videos", "videocam-outline", SendCategory.Videos),
        new SendCategoryItem("Archive", "archive-outline", SendCategory.Archive),
        new SendCategoryItem("Files", "document-outline", SendCategory.Files)
    };

    async ValueTask OnCategorySelected(SendCategoryItem item)
    {
        if (item == null)
            return;

        switch(item.SendCategory)
        {
            default:
                var files =await  FilePicker.PickMultipleAsync();

                foreach (var file in files)
                {
                    FileManager.Add(new(file.FullPath,file.ContentType));
                }
                break;

        }
    }
}