using System.Drawing;

namespace ImageProcessor;

#pragma warning disable CA1416
public static class OrderFilters
{
    /// <summary>
    /// Медианный фильтр (50-й процентиль).
    /// Медианный фильтр заменяет яркость пикселя на медиану значений яркости пикселей в его окрестности.
    /// </summary>
    public static Bitmap ApplyMedianFilter(Bitmap image, int radius)
    {
        return ApplyPercentileFilter(image, radius, 50); // Медиана = 50-й процентиль
    }

    /// <summary>
    /// Фильтр минимума (0-й процентиль).
    /// Заменяет значение пикселя на минимальное значение в его окрестности.
    /// </summary>
    public static Bitmap ApplyMinFilter(Bitmap image, int radius)
    {
        return ApplyPercentileFilter(image, radius, 0); // Минимум = 0-й процентиль
    }

    /// <summary>
    /// Фильтр максимума (100-й процентиль).
    /// Заменяет значение пикселя на минимальное значение в его окрестности.
    /// </summary>
    public static Bitmap ApplyMaxFilter(Bitmap image, int radius)
    {
        return ApplyPercentileFilter(image, radius, 100); // Максимум = 100-й процентиль
    }

    /// <summary>
    /// Усредняющий фильтр.
    /// Заменяет значение текущего пикселя на среднее значение яркости
    /// (grayscale) пикселей в его окрестности.
    /// </summary>
    public static Bitmap ApplyMeanFilter(Bitmap image, int radius)
    {
        Bitmap result = new Bitmap(image.Width, image.Height);
        int maskSize = 2 * radius + 1;

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                int sum = 0;
                int count = 0;

                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        int neighborX = Math.Clamp(x + dx, 0, image.Width - 1);
                        int neighborY = Math.Clamp(y + dy, 0, image.Height - 1);

                        Color neighborPixel = image.GetPixel(neighborX, neighborY);
                        int gray = (int)(0.299 * neighborPixel.R + 0.587 * neighborPixel.G + 0.114 * neighborPixel.B);

                        sum += gray;
                        count++;
                    }
                }

                int mean = sum / count;
                result.SetPixel(x, y, Color.FromArgb(mean, mean, mean));
            }
        }

        return result;
    }

    /// <summary>
    /// Общая реализация процентильного фильтра.
    /// </summary>
    public static Bitmap ApplyPercentileFilter(Bitmap image, int radius, double percentile)
    {
        Bitmap result = new Bitmap(image.Width, image.Height);

        int maskSize = 2 * radius + 1;
        int neighborhoodSize = maskSize * maskSize;

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                int[] neighborhood = new int[neighborhoodSize];
                int index = 0;

                for (int dx = -radius; dx <= radius; dx++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        int neighborX = Math.Clamp(x + dx, 0, image.Width - 1);
                        int neighborY = Math.Clamp(y + dy, 0, image.Height - 1);

                        Color neighborPixel = image.GetPixel(neighborX, neighborY);
                        int gray = (int)(0.299 * neighborPixel.R + 0.587 * neighborPixel.G + 0.114 * neighborPixel.B);

                        neighborhood[index++] = gray;
                    }
                }

                // Вычисляем процентиль
                int value = GetPercentile(neighborhood, percentile);
                result.SetPixel(x, y, Color.FromArgb(value, value, value));
            }
        }

        return result;
    }

    /// <summary>
    /// Метод вычисления процентиля.
    /// </summary>
    private static int GetPercentile(int[] values, double percentile)
    {
        Array.Sort(values);
        int index = (int)Math.Ceiling((percentile / 100.0) * values.Length) - 1;

        return values[Math.Clamp(index, 0, values.Length - 1)];
    }
}
#pragma warning restore CA1416