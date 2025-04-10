using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace EstateMapperClient.ViewModels
{
    public class LoginViewModel:BindableBase
    {
        public LoginViewModel()
        {
            // 初始化命令
            LoginCommand = new DelegateCommand(OnLogin);
            RegisterCommand = new DelegateCommand(OnRegister);
            ForgotPasswordCommand = new DelegateCommand(OnForgotPassword);
        }
        /// <summary>
        /// 忘记密码命令
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnForgotPassword()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 注册命令
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnRegister()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 登录命令
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnLogin()
        {
            throw new NotImplementedException();
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        // 命令定义
        public DelegateCommand LoginCommand { get; }
        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand ForgotPasswordCommand { get; }

        // 加载状态
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
    }
}
