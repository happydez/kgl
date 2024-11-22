using Avalonia.Controls;
using Avalonia.Media.Imaging;
using KGLab.ViewModels;

namespace KGLab.Views;

public partial class ImageViewerWindow : Window
{
    public ImageViewerWindow()
    {
        InitializeComponent();
    }

    public ImageViewerWindow(Bitmap image, string title) : this()
    {
        DataContext = new ImageViewerViewModel(image, title);
    }
}
