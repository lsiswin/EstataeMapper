using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using DryIoc;
using EstateMapperClient.Common;
using EstateMapperClient.Services;
using EstateMapperLibrary.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using RestSharp;

namespace EstateMapperClient.ViewModels
{
    public class RegisterViewModel : ValidatableBase, IDialogAware
    {
        private readonly ICaptchaService _captchaService;
        private readonly ILoginService service;

        public RegisterViewModel(ICaptchaService captchaService, ILoginService service)
        {
            _captchaService = captchaService;
            this.service = service;
            RegisterCommand = new DelegateCommand(ExecuteRegister);
            CancelCommand = new DelegateCommand(CancelRegister);
            RefreshCaptchaCommand = new DelegateCommand(GenerateNewCaptcha);
            GenerateNewCaptcha();
        }

        private void CancelRegister()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        #region 属性
        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                ValidateUsername();
            }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                ValidatePassword();
            }
        }
        private string confirmPassword;

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                ValidatePassword();
            }
        }

        private string phone;

        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                RaisePropertyChanged();
            }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged();
            }
        }

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

        private string captchaInput;

        public string CaptchaInput
        {
            get { return captchaInput; }
            set
            {
                captchaInput = value;
                ValidateCaptcha();
            }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }

        private string _currentCaptcha;
        public BitmapImage CaptchaImage { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand RefreshCaptchaCommand { get; }
        public DelegateCommand RegisterCommand { get; }
        #endregion
        private void ValidateUsername()
        {
            ClearErrors(nameof(UserName));
            if (string.IsNullOrWhiteSpace(UserName))
                AddError(nameof(UserName), "用户名不能为空");
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));
            if (string.IsNullOrWhiteSpace(Password))
                AddError(nameof(Password), "密码不能为空");
        }
        #region 验证码
        private void GenerateNewCaptcha()
        {
            _currentCaptcha = _captchaService.GenerateCaptcha();
            CaptchaImage = _captchaService.GenerateCaptchaImage(_currentCaptcha);
            RaisePropertyChanged(nameof(CaptchaImage));
        }

        private void ValidateCaptcha()
        {
            ClearErrors(nameof(CaptchaInput));
            if (string.IsNullOrWhiteSpace(CaptchaInput))
                AddError(nameof(CaptchaInput), "验证码不能为空");
            else if (CaptchaInput.ToUpper() != _currentCaptcha)
                AddError(nameof(CaptchaInput), "验证码错误");
        }
        #endregion

        // 注册命令
        private async void ExecuteRegister()
        {
            if (Password != ConfirmPassword)
            {
                AddError(nameof(Password), "两次输入的密码不一致!");
                return;
            }
            if (HasErrors)
                return;
            if (CaptchaInput.ToUpper() != _currentCaptcha)
            {
                AddError(nameof(CaptchaInput), "验证码错误");
                return;
            }
            // 执行注册逻辑
            var user = new UserDto()
            {
                UserName = this.UserName,
                Password = this.Password,
                Email = this.Email,
                Phone = this.Phone,
            };
            var response = await service.RegisterAsync(user);
            if (response.Status == ResultStatus.OK)
            {
                // 2. 关闭当前注册窗口
                var parameters = new DialogParameters
            {
                { "username", UserName }
            };

                // 3. 传递注册结果
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
            }
            else
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.No));
            }

        }
    }
}
