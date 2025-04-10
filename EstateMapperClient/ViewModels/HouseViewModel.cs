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
    public class HouseViewModel:BindableBase
    {
		private ObservableCollection<LayoutDto> layouts;

        public HouseViewModel()
        {
            Layouts = new ObservableCollection<LayoutDto>
            {
                new LayoutDto{ 
                    LayoutName=120,
                    LayoutUrl="D:\\Cshrap\\EstateMapper\\EstateMapperLibrary\\Images\\11b627f726b8b46414b6713c202e5e6.png"
                },
                new LayoutDto{ LayoutName=130,
                    LayoutUrl="D:\\Cshrap\\EstateMapper\\EstateMapperLibrary\\Images\\11b627f726b8b46414b6713c202e5e6.png"}
            };
        }

        public ObservableCollection<LayoutDto> Layouts
        {
			get { return layouts; }
			set { layouts = value; RaisePropertyChanged(); }
		}

		
	}
}
