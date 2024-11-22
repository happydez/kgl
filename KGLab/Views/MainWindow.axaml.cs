using Avalonia.Controls;
using Avalonia.Input;
using KGLab.ViewModels;

namespace KGLab.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenOriginalImageModal(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel && viewModel.OriginalImage != null)
            {
                var viewer = new ImageViewerWindow(viewModel.OriginalImage, "Исходное изображение");
                viewer.ShowDialog(this);
            }
        }

        private void OpenProcessedImageModal(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel && viewModel.ProcessedImage != null)
            {
                var viewer = new ImageViewerWindow(viewModel.ProcessedImage, viewModel.FilterDescription);
                viewer.ShowDialog(this);
            }
        }

        private void OriginalHistogramPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm && vm.OriginalHistogram != null)
            {
                var viewer = new ImageViewerWindow(vm.OriginalHistogram, "Гистограмма исходного изображения");
                viewer.Show();
            }
        }

        private void ProcessedHistogramPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm && vm.ProcessedHistogram != null)
            {
                var viewer = new ImageViewerWindow(vm.ProcessedHistogram, "Гистограмма обработанного изображения");
                viewer.Show();
            }
        }
    }
}