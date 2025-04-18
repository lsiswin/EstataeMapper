using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DryIoc;
using EstateMapperClient.Common;
using EstateMapperClient.Events;
using EstateMapperClient.Services;
using EstateMapperClient.ViewModels;
using EstateMapperClient.Views;
using EstateMapperLibrary;
using EstateMapperLibrary.Models;
using Microsoft.Build.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Xaml.Behaviors.Layout;
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
            var response = new LoginResponse();
            dialog.ShowDialog(
                "Login",
                callback =>
                {
                    if (callback.Result == ButtonResult.OK)
                    {

                        response.Token = callback.Parameters.GetValue<string>("token");
                    }
                    Current.MainWindow.Show();
                }
            );
        }
        protected override void OnInitialized()
        {
            // 处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = (Exception)args.ExceptionObject;
                var handler = Container.Resolve<IGlobalExceptionHandler>();
                handler.HandleException(ex);
            };

            // 处理UI线程异常
            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                var handler = Container.Resolve<IGlobalExceptionHandler>();
                handler.HandleException(args.Exception);
                args.Handled = true; // 阻止应用崩溃
            };

            // 处理异步任务异常
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                var handler = Container.Resolve<IGlobalExceptionHandler>();
                handler.HandleException(args.Exception);
                args.SetObserved(); // 标记为已处理
            };
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
                    else
                    {
                        App.Current.Shutdown();
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

            // 注册异常处理器为单例
            containerRegistry.RegisterSingleton<IGlobalExceptionHandler, GlobalExceptionHandler>();

            // 必须注册窗口类型
            containerRegistry.Register<IHouseService, HouseService>();
            containerRegistry.Register<ILayoutService, LayoutService>();
            containerRegistry.Register<ITagService, TagService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<ICaptchaService, CaptchaService>();
            
            // 注册自定义宿主 Window
            containerRegistry.RegisterDialogWindow<CustomDialogWindow>();
            containerRegistry.RegisterDialog<RegisterView, RegisterViewModel>("Register");
            containerRegistry.RegisterDialog<AddHouseView, AddHouseViewModel>("AddHouse");
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>("Login");
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>("Setting");
            containerRegistry.RegisterForNavigation<HouseView, HouseViewModel>("HouseDetail");
        }
    }
}
