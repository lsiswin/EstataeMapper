﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DryIoc;
using EstateMapperClient.Common;
using EstateMapperClient.Events;
using EstateMapperClient.Services;
using EstateMapperClient.ViewModels;
using EstateMapperClient.Views;
using EstateMapperLibrary.Models;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace EstateMapperClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        public static void LoginOut(IContainerProvider containerProvider)
        {
            Current.MainWindow.Hide();
            var dialog = containerProvider.Resolve<IDialogService>();
            dialog.ShowDialog(
                "Login",
                callback =>
                {
                    if (callback.Result == ButtonResult.OK)
                    {
                        Environment.Exit(0);
                        return;
                    }
                    Current.MainWindow.Show();
                }
            );
        }

        protected override void OnInitialized()
        {
            
            var dialog = Container.Resolve<IDialogService>();
            var response = new LoginResponse();
            dialog.ShowDialog(
                "Login",
                callback =>
                {
                    if (callback.Result == ButtonResult.OK)
                    {
                        response.Token = callback.Parameters.GetValue<string>("token");
                    }
                }
            );
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure(response);
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .GetContainer()
                .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry
                .GetContainer()
                .RegisterInstance(@"http://localhost:5172/", serviceKey: "webUrl");

            // 必须注册窗口类型

            containerRegistry.Register<IHouseService, HouseService>();
            containerRegistry.Register<ILayoutService, LayoutService>();
            containerRegistry.Register<ITagService, TagService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<ICaptchaService, CaptchaService>();

            // 注册自定义宿主 Window
            containerRegistry.RegisterDialogWindow<CustomDialogWindow>();
            containerRegistry.RegisterDialog<RegisterView, RegisterViewModel>("Register");
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>("Login");
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>("Setting");
            containerRegistry.RegisterForNavigation<HouseView, HouseViewModel>("HouseDetail");
        }
    }
}
