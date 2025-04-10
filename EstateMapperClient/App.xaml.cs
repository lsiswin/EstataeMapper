using System.Windows;
using DryIoc;
using EstateMapperClient.Common;
using EstateMapperClient.Services;
using EstateMapperClient.ViewModels;
using EstateMapperClient.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;

namespace EstateMapperClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App:PrismApplication
    {
        protected override Window CreateShell()
        {
            
            return Container.Resolve<LoginView>();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
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
            containerRegistry.Register<MainWindow>();
            containerRegistry.Register<LoginView>();

            containerRegistry.Register<IHouseService,HouseService>();
            containerRegistry.Register<ILayoutService, LayoutService>();
            containerRegistry.Register<ITagService, TagService>();


            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>("Home");
            containerRegistry.RegisterForNavigation<SettingView,SettingViewModel>("Setting");
            containerRegistry.RegisterForNavigation<HouseView,HouseViewModel>("HouseDetail");
        }
    }
}
