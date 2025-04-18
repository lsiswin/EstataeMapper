using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary.Models;
using Prism.Mvvm;

namespace EstateMapperClient.ViewModels
{
    public class RegionViewModel: BindableBase
    {
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
        private int? selectedSubRegionId;

        public int? SelectedSubRegionId
        {
            get { return selectedSubRegionId; }
            set
            {
                selectedSubRegionId = value;
                RaisePropertyChanged();
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
    }
}
