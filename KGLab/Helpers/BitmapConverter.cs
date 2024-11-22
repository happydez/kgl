using Avalonia.Media.Imaging;
using System.Drawing;
using System.IO;

public static class BitmapConverter
{
    public static System.Drawing.Bitmap ToSystemDrawingBitmap(this Avalonia.Media.Imaging.Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        bitmap.Save(stream);

        return new System.Drawing.Bitmap(stream);
    }

    public static Avalonia.Media.Imaging.Bitmap ToAvaloniaBitmap(this System.Drawing.Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Seek(0, SeekOrigin.Begin);

        return new Avalonia.Media.Imaging.Bitmap(stream);
    }
}