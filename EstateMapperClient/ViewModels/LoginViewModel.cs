using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EstateMapperClient.Common;
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
    public class LoginViewModel : ValidatableBase, IDialogAware
    {
        public LoginViewModel(
            ILoginService service,
            IDialogService dialogService,
            IEventAggregator eventAggregator
        )
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

        private void CloseCurrentWindow()
        {
            App.Current.Shutdown();
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
                    if (result.Result == ButtonResult.OK)
                    {
                        this.UserName = result.Parameters.GetValue<string>("username");
                        MessageQueue.Enqueue("注册成功,请登录!");
                    }
                    else if (result.Result == ButtonResult.No)
                    {
                        MessageQueue.Enqueue("注册失败,请重试!");
                    }
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
            if (result.Status == ResultStatus.OK)
            {
                RequestClose.Invoke(
                    new DialogResult(
                        ButtonResult.OK,
                        new DialogParameters
                        {
                            { "username", UserName },
                            { "token", result.Result.Token }
                        }
                    )
                );
            }
            else
            {
                MessageQueue.Enqueue("登录失败,请重试!");
                Password = string.Empty;
            }
        }

        private SnackbarMessageQueue messageQueue;

        public SnackbarMessageQueue MessageQueue
        {
            get { return messageQueue; }
            set { messageQueue = value; }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
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

        private readonly ILoginService service;
        private readonly IDialogService dialogService;

        public event Action<IDialogResult> RequestClose;


        public string Title => "";
    }
}
