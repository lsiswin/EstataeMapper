using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using EstateMapperClient.Services;
using EstateMapperLibrary.Models;
using Microsoft.Build.Framework;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace EstateMapperClient.ViewModels
{
    public class AddHouseViewModel : BindableBase, IDialogAware
    {
        public AddHouseViewModel(IDialogService dialogService, IHouseService service)
        {
            UploadMainImageCommand = new DelegateCommand(UploadMainImage);
            UploadFloorPlanCommand = new DelegateCommand(
                UploadFloorPlan,
                () => isLayoutChanged
            ).ObservesProperty(() => Name);
            RemoveFloorPlanCommand = new DelegateCommand<LayoutDto>(RemoveFloorPlan);
            AddTagCommand = new DelegateCommand(AddTag);
            RemoveTagCommand = new DelegateCommand<TagDto>(RemoveTag);
            SubmitCommand = new DelegateCommand(Submit);
            CanleCommand = new DelegateCommand(() =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            });
            this.dialogService = dialogService;
            this.service = service;
        }

        private async void Submit()
        {
            try
            {
                // 创建保存目录
                var savePath = GetSavePath();

                // 创建目录（递归创建不存在的父目录）
                Directory.CreateDirectory(savePath);

                // 保存主图
                string mainImageUrl = null;
                if (MainImage != null)
                {
                    var mainImageDest = Path.Combine(
                        savePath,
                        "main",
                        Path.GetFileName(MainImage.OriginalPath)
                    );
                    Directory.CreateDirectory(Path.GetDirectoryName(mainImageDest));
                    File.Copy(MainImage.OriginalPath, mainImageDest);
                    mainImageUrl = mainImageDest;
                }
                // 构造提交数据
                var propertyData = new HouseDto
                {
                    MainImageUrl = mainImageUrl,
                    Layouts = Layouts.ToList(),
                    SubRegionId = (int)Region.SelectedSubRegionId,
                    Name = Name,
                    DetailAddress = House.DetailAddress,
                    Price = House.Price,
                    Tags = Tags.ToList(),
                };
                // 调用后台保存逻辑
                if (IsUpdate)
                {
                    var result = await service.UpdateAsync(propertyData);
                    if (result.Status == ResultStatus.OK)
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                    }
                }
                else
                {
                    var result = await service.AddAsync(propertyData);
                    if (result.Status == ResultStatus.FORBIDDEN)
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.Retry));
                    }
                    if (result.Status == ResultStatus.OK)
                    {
                        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                    }
                }
                RequestClose?.Invoke(new DialogResult(ButtonResult.No));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提交失败：{ex.Message}");
            }
        }

        private string GetSavePath()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var relativePath = Path.Combine("PropertyUploads");
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(baseDirectory, relativePath, timestamp);
        }

        public RegionViewModel Region { get; } = new RegionViewModel();
        private HouseDto house = new HouseDto();

        public HouseDto House
        {
            get { return house; }
            set
            {
                house = value;
                RaisePropertyChanged();
            }
        }

        private BitmapImage GenerateThumbnail(string imagePath, int maxSize)
        {
            using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.DecodePixelWidth = maxSize;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }

        // 标签管理
        private ObservableCollection<TagDto> _tags = new ObservableCollection<TagDto>();
        public ObservableCollection<TagDto> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                RaisePropertyChanged();
            }
        }

        private TagDto _newTag = new();
        public TagDto NewTag
        {
            get => _newTag;
            set
            {
                _newTag = value;
                RaisePropertyChanged();
                //ClearErrors(nameof(NewTag.TagName));
            }
        }

        private void RemoveTag(TagDto tag)
        {
            if (tag != null)
            {
                Tags.Remove(tag);
            }
        }

        private void AddTag()
        {
            // 新增：检查标签是否为空
            if (string.IsNullOrWhiteSpace(NewTag.TagName?.Trim()))
            {
                return;
            }
            if (
                Tags.Any(t =>
                    t.TagName?.Trim()
                        .Equals(NewTag.TagName?.Trim(), StringComparison.OrdinalIgnoreCase) ?? false
                )
            )
            {
                return;
            }

            Tags.Add(new TagDto { TagName = NewTag.TagName?.Trim() });
            NewTag = new TagDto(); // 清空输入
            //ClearErrors(nameof(NewTag.TagName)); // 清除 TagName 的错误
        }

        private readonly Dictionary<string, List<string>> _errors = new();
        private readonly IDialogService dialogService;
        private readonly IHouseService service;

        //// INotifyDataErrorInfo 实现
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        //public bool HasErrors => _errors.Any();

        //public IEnumerable GetErrors(string propertyName) =>
        //    _errors.TryGetValue(propertyName, out var errors) ? errors : null;

        //private void SetError(string error, string propertyName)
        //{
        //    if (!_errors.ContainsKey(propertyName))
        //        _errors[propertyName] = new List<string>();

        //    _errors[propertyName].Add(error);
        //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //}

        //private void ClearErrors(string propertyName)
        //{
        //    _errors.Remove(propertyName);
        //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //}

        private void RemoveFloorPlan(LayoutDto dto)
        {
            if (dto != null)
            {
                Layouts.Remove(dto);
            }
        }

        private void UploadFloorPlan()
        {
            var param = new DialogParameters();
            param.Add("name", Name);
            dialogService.ShowDialog(
                "Layout",
                param,
                result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        var layout = result.Parameters.GetValue<LayoutDto>("layout");
                        // 更新户型图显示（根据实际需求调整）
                        Layouts.Add(layout);
                    }
                }
            );
        }

        private void UploadMainImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var imagePath = openFileDialog.FileName;
                MainImage = new ImageModel
                {
                    OriginalPath = imagePath,
                    Thumbnail = GenerateThumbnail(imagePath, 300),
                };
            }
        }

        private ImageModel _mainImage = new ImageModel();
        public ImageModel MainImage
        {
            get => _mainImage;
            set
            {
                _mainImage = value;
                RaisePropertyChanged();
            }
        }

        // 户型图集合
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(isLayoutChanged));
            }
        }

        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var regions = parameters.GetValue<ObservableCollection<Region>>("region");
            Region.Regions = regions;
            Region.SubRegions = new ObservableCollection<SubRegion>(
                Region.Regions.SelectMany(r => r.SubRegions).ToList()
            );
            if (parameters.ContainsKey("House"))
            {
                House = parameters.GetValue<HouseDto>("House");
                Name = House.Name;
                Layouts = new ObservableCollection<LayoutDto>(House.Layouts);
                Tags = new ObservableCollection<TagDto>(House.Tags);
                MainImage.Thumbnail = GenerateThumbnail(House.MainImageUrl, 300);
                var subregion = Region
                    .SubRegions.Where(p => p.SubRegionId == House.SubRegionId)
                    .FirstOrDefault();
                Region.SelectedRegionId = subregion.RegionId;
                Region.SelectedSubRegionId = subregion.SubRegionId;
                IsUpdate = true;
            }
        }

        private bool IsUpdate { get; set; }

        // 命令集合
        public DelegateCommand UploadMainImageCommand { get; }
        public DelegateCommand UploadFloorPlanCommand { get; private set; }
        public DelegateCommand<LayoutDto> RemoveFloorPlanCommand { get; }
        public DelegateCommand AddTagCommand { get; }
        public DelegateCommand<TagDto> RemoveTagCommand { get; }
        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand CanleCommand { get; }
        private ObservableCollection<LayoutDto> layouts = new ObservableCollection<LayoutDto>();
        public bool isLayoutChanged => !string.IsNullOrWhiteSpace(Name);
        public ObservableCollection<LayoutDto> Layouts
        {
            get { return layouts; }
            set
            {
                layouts = value;
                RaisePropertyChanged();
            }
        }

        public class ImageModel
        {
            public string OriginalPath { get; set; }
            public BitmapImage Thumbnail { get; set; }
            private string fileName;

            public string FileName
            {
                get { return fileName; }
                set { fileName = Path.GetFileName(OriginalPath); }
            }
        }
    }
}
