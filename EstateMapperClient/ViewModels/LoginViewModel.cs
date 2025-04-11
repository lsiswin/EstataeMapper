using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EstateMapperClient.Services;
using EstateMapperLibrary.Models;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using RestSharp;

namespace EstateMapperClient.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        public LoginViewModel(ILoginService service, IDialogService dialogService,IEventAggregator eventAggregator)
        {
            // 初始化命令
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            LoginCommand = new DelegateCommand(OnLogin);
            RegisterCommand = new DelegateCommand(OnRegister);
            ForgotPasswordCommand = new DelegateCommand(OnForgotPassword);
            CloseCommand = new DelegateCommand(CloseCurrentWindow);
            this.service = service;
            this.dialogService = dialogService;
        }

        /// <summary>
        /// 忘记密码命令
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnForgotPassword() { }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async void OnRegister()
        {
            dialogService.ShowDialog(
                "Register",
                result =>
                {
                    this.UserName = result.Parameters.GetValue<string>("username");
                }
            );

        }

        /// <summary>
        /// 登录命令
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async void OnLogin()
        {
            var user = new UserDto() { UserName = this.UserName, Password = this.Password };
            var result = await service.LoginAsync(user);
            if (result.Status == ResultStatus.OK) {
                Application
                .Current.MainWindow.Show();
                CloseCurrentWindow();
            }else
            {
                MessageQueue.Enqueue("登录失败,请重试!");
                
                UserName = "";
                Password = string.Empty;
            }
        }
        private SnackbarMessageQueue messageQueue;

        public SnackbarMessageQueue MessageQueue
        {
            get { return messageQueue; }
            set { messageQueue = value; }
        }


        public void CloseCurrentWindow()
        {
            var window = Application
                .Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.IsActive);

            window?.Close();
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                RaisePropertyChanged();
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged();
            }
        }

        // 命令定义
        public DelegateCommand LoginCommand { get; }
        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand ForgotPasswordCommand { get; }
        public DelegateCommand CloseCommand { get; }

        // 加载状态
        private bool _isLoading;
        private readonly ILoginService service;
        private readonly IDialogService dialogService;

        public bool IsLoggingIn
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
    }
}
