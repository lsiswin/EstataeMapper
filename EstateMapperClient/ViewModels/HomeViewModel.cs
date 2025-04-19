using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperClient.Common;
using EstateMapperClient.Services;
using EstateMapperLibrary.Models;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace EstateMapperClient.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IHouseService houseService;
        private readonly IDialogService dialogService;
        private readonly IRegionManager regionManager;

        public HomeViewModel(
            IHouseService houseService,
            IDialogService dialogService,
            IRegionManager regionManager
        )
        {
            this.houseService = houseService;
            this.dialogService = dialogService;
            this.regionManager = regionManager;
            LoadDataAsync();
            // 监听页码变化
            Pagination.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(Pagination.CurrentPage))
                {
                    LoadPageData();
                }
            };
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            ExecuteFindHouseCommand = new DelegateCommand(LoadPageData);
            HouseDetailCommand = new DelegateCommand<HouseDto>(HouseDetail);
            ExecutedHouseCommand = new DelegateCommand<HouseDto>(ExecutedHouse);
        }

        private void ExecutedHouse(HouseDto dto)
        {
            var parame = new DialogParameters { { "region", Region.Regions } };
            if (dto != null)
            {
                parame.Add("House", dto);
            }
            dialogService.ShowDialog(
                "AddHouse",
                parame,
                r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        MessageQueue.Enqueue("添加成功");
                    }
                    else if (r.Result == ButtonResult.Retry)
                    {
                        MessageQueue.Enqueue("权限不足,添加失败!");
                    }
                    // 处理确认按钮点击事件
                    LoadPageData();
                }
            );
        }

        private void HouseDetail(HouseDto dto)
        {
            var parame = new NavigationParameters { { "House", dto } };
            regionManager
                .Regions[PrismManager.MainViewRegionName]
                .RequestNavigate("HouseDetail", parame);
        }

        // 当前页数据（使用ObservableCollection）
        private ObservableCollection<HouseDto> _currentItems;
        public ObservableCollection<HouseDto> CurrentItems
        {
            get => _currentItems;
            set => SetProperty(ref _currentItems, value);
        }

        private async void LoadPageData()
        {
            var pagerequest = new HousePagedRequest
            {
                PageNumber = Pagination.CurrentPage,
                PageSize = Pagination.PageSize,
            };
            if (!string.IsNullOrEmpty(SearchText))
            {
                pagerequest.Search = SearchText;
            }
            if (Region.SelectedSubRegionId.HasValue)
            {
                pagerequest.RegionId = Region.SelectedSubRegionId;
            }
            var response = await houseService.GetPageAsync(pagerequest);

            // 同步分页信息
            Pagination.SyncFromPagedResult(response.Result);
            HouseDtos = response.Result.Items;
            // 更新数据
            CurrentItems = new ObservableCollection<HouseDto>(HouseDtos);
        }

        public string Title { get; set; } = "Home";
        private bool progress;

        public bool Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                RaisePropertyChanged();
            }
        }
        private string searchText;

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RaisePropertyChanged();
            }
        }

        private async void LoadDataAsync()
        {
            Progress = true;
            if (Progress)
                LoadPageData();
            var result = await houseService.GetRegionsAsync();
            Region.Regions = new ObservableCollection<EstateMapperLibrary.Models.Region>(
                result.Result
            );
            Region.SubRegions = new ObservableCollection<SubRegion>(
                Region.Regions.SelectMany(r => r.SubRegions).ToList()
            );
            Progress = false;
        }

        public PaginationViewModel Pagination { get; } = new();

        #region 行政区初始化
        public RegionViewModel Region { get; } = new RegionViewModel();

        #endregion
        private List<HouseDto> houseDtos;

        public List<HouseDto> HouseDtos
        {
            get { return houseDtos; }
            set
            {
                houseDtos = value;
                RaisePropertyChanged();
            }
        }

        public SnackbarMessageQueue MessageQueue { get; private set; }
        public DelegateCommand ExecuteFindHouseCommand { get; set; }
        public DelegateCommand<HouseDto> HouseDetailCommand { get; }
        public DelegateCommand<HouseDto> ExecutedHouseCommand { get; private set; }
    }
}
