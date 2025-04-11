

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace EstateMapperClient.Services
{
    public class CaptchaService:ICaptchaService
    {
        private static readonly Random _random = new();
        private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // 排除易混淆字符

       
        public string GenerateCaptcha(int length = 6)
        {
            return new string(Enumerable.Repeat(Chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public BitmapImage GenerateCaptchaImage(string captchaText)
        {
            using var bitmap = new Bitmap(200, 60);
            using var graphics = Graphics.FromImage(bitmap);

            // 绘制背景
            graphics.Clear(Color.White);

            // 添加干扰线
            for (int i = 0; i < 10; i++)
            {
                var pen = new Pen(GetRandomColor());
                graphics.DrawLine(pen,
                    new Point(_random.Next(0, 200), _random.Next(0, 60)),
                    new Point(_random.Next(0, 200), _random.Next(0, 60)));
            }

            // 绘制文字
            for (int i = 0; i < captchaText.Length; i++)
            {
                graphics.DrawString(captchaText[i].ToString(),
                    new Font("Arial", 10, FontStyle.Bold),
                    new SolidBrush(GetRandomColor()),
                    new PointF(30 + i * 15, 15));
            }

            // 转换为BitmapImage
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(_random.Next(256),
                _random.Next(256), _random.Next(256));
        }
    }
}
