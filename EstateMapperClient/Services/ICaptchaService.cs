using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EstateMapperClient.Services
{
    public interface ICaptchaService
    {
        string GenerateCaptcha(int length = 6);
        BitmapImage GenerateCaptchaImage(string captchaText);
    }
}
