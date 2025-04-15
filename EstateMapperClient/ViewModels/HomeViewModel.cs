using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperClient.Services;
using EstateMapperLibrary.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace EstateMapperClient.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IHouseService houseService;
        private readonly IDialogService dialogService;

        public HomeViewModel(IHouseService houseService, IDialogService dialogService)
        {
            this.houseService = houseService;
            this.dialogService = dialogService;
            LoadDataAsync();
            // 监听页码变化
            Pagination.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(Pagination.CurrentPage))
                    await LoadPageData();
            };
            FindHouseCommand = new DelegateCommand(() =>
            {
                TokenStorage.DeleteToken();
            });
        }

        // 当前页数据（使用ObservableCollection）
        private ObservableCollection<HouseDto> _currentItems;
        public ObservableCollection<HouseDto> CurrentItems
        {
            get => _currentItems;
            set => SetProperty(ref _currentItems, value);
        }

        private async Task LoadPageData()
        {
            var response = await houseService.GetPageAsync(
                new HousePagedRequest
                {
                    PageNumber = Pagination.CurrentPage,
                    PageSize = Pagination.PageSize,
                }
            );

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

        private async void LoadDataAsync()
        {
            Progress = true;
            if(Progress)
                await LoadPageData();
            var result = await houseService.GetRegionsAsync();
            Regions = new ObservableCollection<Region>(result.Result);
            SubRegions = new ObservableCollection<SubRegion>(
                Regions.SelectMany(r => r.SubRegions).ToList()
            );
            
            Progress = false;
        }

        public PaginationViewModel Pagination { get; } = new();

        #region 行政区初始化
        private ObservableCollection<Region> _regions;
        public ObservableCollection<Region> Regions
        {
            get => _regions;
            set
            {
                _regions = value;
                RaisePropertyChanged();
            }
        }
        private int? _selectedRegionId;
        public int? SelectedRegionId
        {
            get => _selectedRegionId;
            set
            {
                _selectedRegionId = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasSelectedRegion));
                FilterSubRegions();
            }
        }
        private ObservableCollection<SubRegion> _subRegions;
        public ObservableCollection<SubRegion> SubRegions
        {
            get => _subRegions;
            set
            {
                _subRegions = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<SubRegion> _filteredSubRegions;
        public ObservableCollection<SubRegion> FilteredSubRegions
        {
            get => _filteredSubRegions;
            set
            {
                _filteredSubRegions = value;
                RaisePropertyChanged();
            }
        }

        private void FilterSubRegions()
        {
            if (SelectedRegionId.HasValue)
            {
                FilteredSubRegions = new ObservableCollection<SubRegion>(
                    SubRegions.Where(s => s.RegionId == SelectedRegionId.Value)
                );
            }
            else
            {
                FilteredSubRegions?.Clear();
            }
        }

        public bool HasSelectedRegion => SelectedRegionId.HasValue;
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
        public DelegateCommand FindHouseCommand { get; set; }
    }
}
