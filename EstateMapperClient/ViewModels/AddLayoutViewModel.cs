using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using EstateMapperLibrary.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace EstateMapperClient.ViewModels
{
    public class AddLayoutViewModel : BindableBase, IDialogAware
    {
        public AddLayoutViewModel()
        {
            UploadImageCommand = new DelegateCommand(UploadImage, () => CanUpload)
                .ObservesProperty(() => LayoutName)
                .ObservesProperty(() => Description);

            SaveCommand = new DelegateCommand(Save);
            CanleCommand = new DelegateCommand(() =>{
                RequestClose?.Invoke(new DialogResult(ButtonResult.No));
            });;
        }

        public string HouseName { get; set; }
        private string layoutName;

        public string LayoutName
        {
            get { return layoutName; }
            set { layoutName = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(CanUpload)); }
        }

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(CanUpload)); }
        }

        public bool CanUpload =>
            !string.IsNullOrWhiteSpace(LayoutName) && !string.IsNullOrWhiteSpace(Description);

        // 上传户型图
        public DelegateCommand UploadImageCommand { get; }

        private void UploadImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
            }
        }

        // 保存
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CanleCommand { get; private set; }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(ImagePath))
                return;

            var layout = new LayoutDto
            {
                LayoutName = int.Parse(LayoutName), // 根据实际需求调整类型
                Description = Description,
                LayoutUrl = GenerateImagePath(), // 生成保存路径
            };

            // 触发关闭并返回结果
            var parameters = new DialogParameters { { "layout", layout } };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        }

        private string GenerateImagePath()
        {
            // 示例：保存到本地目录，实际需根据项目需求调整
            var fileName = $"{HouseName}_{LayoutName}_{Description}{Path.GetExtension(ImagePath)}";
            var savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layouts", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            File.Copy(ImagePath, savePath);
            return savePath;
        }

        // Dialog 实现
        public string Title => "添加户型图";
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { 
            if(parameters.ContainsKey("name"))
                HouseName = parameters.GetValue<string>("name");
        }
    }
}
