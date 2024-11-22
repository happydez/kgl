using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KGLab.ViewModels;

public partial class ImageViewerViewModel : ObservableObject
{
    [ObservableProperty]
    private Bitmap image;

    [ObservableProperty]
    private string title;

    public ImageViewerViewModel(Bitmap image, string title)
    {
        Image = image;
        Title = title;
    }
}
