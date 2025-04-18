using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary.Models;
using Prism.Common;
using Prism.Mvvm;
using Prism.Regions;

namespace EstateMapperClient.ViewModels
{
    public class HouseViewModel:BindableBase,INavigationAware
    {
        public HouseViewModel()
        {
            
        }
        private HouseDto house;

        public HouseDto House
        {
            get { return house; }
            set { house = value; RaisePropertyChanged(); }
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var param = navigationContext.Parameters;
            if (param.TryGetValue<HouseDto>("House", out var dto))
                this.House = dto;
        }
    }
}
