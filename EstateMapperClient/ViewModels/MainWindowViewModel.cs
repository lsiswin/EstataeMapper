using System.Collections.ObjectModel;
using System.Windows.Navigation;
using EstateMapperClient.Common;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace EstateMapperClient.ViewModels
{
    public class MainWindowViewModel : BindableBase,IConfigureService
    {
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            menuItems = new ObservableCollection<MenuItem>();
            NavigateCommand = new DelegateCommand<MenuItem>(ExecuteNavigate);
        }

        private readonly IRegionManager regionManager;

        public DelegateCommand<MenuItem> NavigateCommand { get; private set; }

        private void ExecuteNavigate(MenuItem menu)
        { // 导航到指定视图
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(menu.ViewName);
            IsLeftDrawerOpen = false;            
        }
        private bool isLeftDrawerOpen;

        public bool IsLeftDrawerOpen
        {
            get { return isLeftDrawerOpen; }
            set { isLeftDrawerOpen = value;RaisePropertyChanged(); }
        }

        private ObservableCollection<MenuItem> menuItems;

        public ObservableCollection<MenuItem> MenuItems
        {
            get { return menuItems; }
            set
            {
                menuItems = value;
                RaisePropertyChanged();
            }
        }

        void CreateMenu()
        {
            menuItems.Add(
                new MenuItem
                {
                    Title = "首页",
                    Icon = PackIconKind.Home,
                    ViewName = "Home",
                }
            );
            menuItems.Add(
                new MenuItem
                {
                    Title = "房源详情",
                    Icon = PackIconKind.House,
                    ViewName = "HouseDetail",
                }
            );
            menuItems.Add(
                new MenuItem
                {
                    Title = "设置",
                    Icon = PackIconKind.Settings,
                    ViewName = "Setting",
                }
            );
        }
        public void Configure()
        {
            CreateMenu();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("Home");
        }
    }
}
