using System;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using EstateMapperClient.Common;
using EstateMapperLibrary.Models;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace EstateMapperClient.ViewModels
{
    public class MainWindowViewModel : BindableBase, IConfigureService
    {
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            menuItems = new ObservableCollection<MenuItem>();
            NavigateCommand = new DelegateCommand<MenuItem>(ExecuteNavigate);
            ExecuLeftCommand = new DelegateCommand(() =>
            {
                if (!IsLeftDrawerOpen)
                {
                    IsLeftDrawerOpen = true;
                }
                else
                {
                    IsLeftDrawerOpen = false;
                }
            });
            MovePrevCommand = new DelegateCommand(OnMovePrev, CanMovePrev);
            MoveNextCommand = new DelegateCommand(OnMoveNext, CanMoveNext);
            HomeCommand = new DelegateCommand(OnHome);
        }

        #region 导航功能
        private void OnHome()
        {
            // 导航到首页并重置历史
            regionManager.RequestNavigate(
                PrismManager.MainViewRegionName,
                "Home",
                navResult =>
                {
                    if (navResult.Result == true)
                    {
                        journal = navResult.Context.NavigationService.Journal;
                        journal.Clear(); // 清空导航历史
                    }
                }
            );
        }

        private bool CanMovePrev() => journal?.CanGoBack == true;

        private bool CanMoveNext() => journal?.CanGoBack == true;

        private void OnMovePrev() => journal.GoBack();

        private void OnMoveNext() => journal.GoForward();

        private IRegionNavigationJournal journal;
        public DelegateCommand MovePrevCommand { get; }
        public DelegateCommand MoveNextCommand { get; }
        public DelegateCommand HomeCommand { get; }

        private readonly IRegionManager regionManager;

        public DelegateCommand<MenuItem> NavigateCommand { get; }

        private void ExecuteNavigate(MenuItem menu)
        { // 导航到指定视图
            regionManager
                .Regions[PrismManager.MainViewRegionName]
                .RequestNavigate(
                    menu.ViewName,
                    result =>
                    {
                        if (result.Result == true)
                        {
                            journal = result.Context.NavigationService.Journal;
                            MovePrevCommand.RaiseCanExecuteChanged();
                            MoveNextCommand.RaiseCanExecuteChanged();
                        }
                    }
                );
            IsLeftDrawerOpen = false;
        }
        #endregion
        public DelegateCommand ExecuLeftCommand { get; set; }

        private bool isLeftDrawerOpen;

        public bool IsLeftDrawerOpen
        {
            get { return isLeftDrawerOpen; }
            set
            {
                isLeftDrawerOpen = value;
                RaisePropertyChanged();
            }
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

        public void Configure(LoginResponse response)
        {
            CreateMenu();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("Home");
        }
    }
}
