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
        public AddHouseViewModel()
        {
            UploadMainImageCommand = new DelegateCommand(UploadMainImage);
            UploadFloorPlanCommand = new DelegateCommand(UploadFloorPlan);
            RemoveFloorPlanCommand = new DelegateCommand<ImageModel>(RemoveFloorPlan);
            AddTagCommand = new DelegateCommand(AddTag);
            RemoveTagCommand = new DelegateCommand<TagDto>(RemoveTag);
            SubmitCommand = new DelegateCommand(Submit);
            CanleCommand = new DelegateCommand(() =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            });
        }

        private void Submit()
        {
            try
            {
                // 创建保存目录
                var savePath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "PropertyUploads",
                    DateTime.Now.ToString("yyyyMMddHHmmss")
                );
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

                // 保存户型图

                //保存标签

                // 构造提交数据
                var propertyData = new HouseDto
                {
                    MainImageUrl = mainImageUrl,
                    Layouts = Layouts,
                    SubRegionId = (int)Region.SelectedSubRegionId,
                    Name = House.Name,
                    DetailAddress = House.DetailAddress,
                    Price = House.Price,
                    Tags = Tags.ToList(),
                };

                // 调用后台保存逻辑



                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提交失败：{ex.Message}");
            }
        }

        public RegionViewModel Region { get; } = new RegionViewModel();
        private HouseDto house= new HouseDto();

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

        private void RemoveFloorPlan(ImageModel image)
        {
            if (image != null)
            {
                FloorPlans.Remove(image);
            }
        }

        private void UploadFloorPlan()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Multiselect = true,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    FloorPlans.Add(
                        new ImageModel
                        {
                            OriginalPath = filePath,
                            FileName = Path.GetFileName(filePath),
                        }
                    );
                }
            }
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

        private ImageModel _mainImage;
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
        private ObservableCollection<ImageModel> _floorPlans =
            new ObservableCollection<ImageModel>();
        public ObservableCollection<ImageModel> FloorPlans
        {
            get => _floorPlans;
            set
            {
                _floorPlans = value;
                RaisePropertyChanged();
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
            }
        }

        // 命令集合
        public DelegateCommand UploadMainImageCommand { get; }
        public DelegateCommand UploadFloorPlanCommand { get; }
        public DelegateCommand<ImageModel> RemoveFloorPlanCommand { get; }
        public DelegateCommand AddTagCommand { get; }
        public DelegateCommand<TagDto> RemoveTagCommand { get; }
        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand CanleCommand { get; }
        public List<LayoutDto> Layouts { get; private set; }

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
