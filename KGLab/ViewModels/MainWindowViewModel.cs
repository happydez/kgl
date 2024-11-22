using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageProcessor;
using System.IO;
using System.Threading.Tasks;

using Avalonia.Controls;

using SD = System.Drawing;
using KGLab.Views;

namespace KGLab.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private int filterRadius;

    [ObservableProperty]
    private Bitmap? originalImage;

    [ObservableProperty]
    private Bitmap? processedImage;

    [ObservableProperty]
    private Bitmap? originalHistogram;

    [ObservableProperty]
    private Bitmap? processedHistogram;

    [ObservableProperty]
    private string filterDescription = "Нет применённого фильтра";

    public MainWindowViewModel()
    {
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(OriginalImage) && OriginalImage != null)
            {
                OriginalHistogram = HistogramGenerator.GenerateHistogram(OriginalImage.ToSystemDrawingBitmap())?.ToAvaloniaBitmap();
            }

            if (e.PropertyName == nameof(ProcessedImage) && ProcessedImage != null)
            {
                ProcessedHistogram = HistogramGenerator.GenerateHistogram(ProcessedImage.ToSystemDrawingBitmap())?.ToAvaloniaBitmap();
            }
        };
    }

    [RelayCommand]
    private async Task OpenImage()
    {
        var dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "png", "jpg", "jpeg" } });

        var result = await dialog.ShowAsync(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null);
        if (result?.Length > 0)
        {
            using var stream = File.OpenRead(result[0]);
            OriginalImage = new Bitmap(stream);
            ProcessedImage = null;
            FilterDescription = "Нет применённого фильтра";
        }
    }

    [RelayCommand]
    private async Task SaveImage()
    {
        if (ProcessedImage == null) return;

        var saveDialog = new SaveFileDialog();
        saveDialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "png", "jpg" } });

        var result = await saveDialog.ShowAsync(App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null);
        if (result != null)
        {
            using var stream = File.OpenWrite(result);
            ProcessedImage.Save(stream);
        }
    }

    [RelayCommand]
    private void OpenOriginalHistogramModal()
    {
        if (OriginalHistogram != null)
        {
            var viewer = new ImageViewerWindow(OriginalHistogram, "Гистограмма исходного изображения");
            viewer.Show();
        }
    }

    [RelayCommand]
    private void OpenProcessedHistogramModal()
    {
        if (ProcessedHistogram != null)
        {
            var viewer = new ImageViewerWindow(ProcessedHistogram, "Гистограмма обработанного изображения");
            viewer.Show();
        }
    }

    [RelayCommand]
    private void ApplyMedianFilter()
    {
        if (OriginalImage == null) return;

        var systemBitmap = OriginalImage.ToSystemDrawingBitmap();
        var filteredBitmap = OrderFilters.ApplyMedianFilter(systemBitmap, filterRadius);
        ProcessedImage = filteredBitmap.ToAvaloniaBitmap();
        FilterDescription = "Результат: Медианный фильтр";
    }

    [RelayCommand]
    private void ApplyMinFilter()
    {
        if (OriginalImage == null) return;

        var systemBitmap = OriginalImage.ToSystemDrawingBitmap();
        var filteredBitmap = OrderFilters.ApplyMinFilter(systemBitmap, filterRadius);
        ProcessedImage = filteredBitmap.ToAvaloniaBitmap();
        FilterDescription = "Результат: Минимальный фильтр";
    }

    [RelayCommand]
    private void ApplyMaxFilter()
    {
        if (OriginalImage != null)
        {
            var systemBitmap = OriginalImage.ToSystemDrawingBitmap();
            var filteredBitmap = OrderFilters.ApplyMaxFilter(systemBitmap, filterRadius);
            ProcessedImage = filteredBitmap.ToAvaloniaBitmap();
            FilterDescription = "Результат: Максимальный фильтр";
        }
    }

    [RelayCommand]
    private void ApplyMeanFilter()
    {
        if (OriginalImage != null)
        {
            var systemBitmap = OriginalImage.ToSystemDrawingBitmap();
            var filteredBitmap = OrderFilters.ApplyMeanFilter(systemBitmap, filterRadius);
            ProcessedImage = filteredBitmap.ToAvaloniaBitmap();
            FilterDescription = "Результат: Усредняющий фильтр";
        }
    }

    [RelayCommand]
    private void OpenImageFullView(Bitmap image)
    {
        if (image == null) return;

        var title = (image == OriginalImage) ? "Исходное изображение" : FilterDescription;

        var viewer = new ImageViewerWindow(image, title);
        viewer.Show();
    }

    partial void OnFilterRadiusChanged(int value)
    {
        if (value < 1)
        {
            FilterRadius = 1;
        }
        else if (value > 10)
        {
            FilterRadius = 10;
        }
    }
}