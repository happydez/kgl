using System.Drawing;

namespace ImageProcessor;

public static class HistogramGenerator
{
    public static Bitmap GenerateHistogram(Bitmap? image)
    {
        if (image == null)
            return new Bitmap(256, 100);

        int width = image.Width;
        int height = image.Height;

        int[] histogram = new int[256];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = image.GetPixel(x, y);
                int intensity = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);

                histogram[intensity]++;
            }
        }

        int maxValue = histogram.Max();
        double scale = 99.0 / maxValue;

        Bitmap histogramImage = new Bitmap(256, 100);

        using (Graphics g = Graphics.FromImage(histogramImage))
        {
            g.Clear(Color.White);

            for (int i = 0; i < 256; i++)
            {
                int barHeight = (int)(histogram[i] * scale);
                g.DrawLine(Pens.Black, i, 100, i, 100 - barHeight);
            }
        }

        return histogramImage;
    }
}
