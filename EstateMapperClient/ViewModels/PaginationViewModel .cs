using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using EstateMapperLibrary.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace EstateMapperClient.ViewModels
{
    public class PaginationViewModel : BindableBase
    {

        // 从PagedResult同步数据的方法
        public void SyncFromPagedResult<T>(PagedResult<T> result)
        {
            CurrentPage = result.PageNumber;
            TotalCount = result.TotalCount;
            PageSize = result.PageSize;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            UpdatePageNumbers();
        }

        private int currentPage = 1;

        public int CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value, () => UpdatePageNumbers());
        }



        // 总记录数
        private int totalCount;

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; RaisePropertyChanged(); }
        }



        // 每页数量
        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value;RaisePropertyChanged(); }
        }


        // 计算总页数


        private int totalPages;

        public PaginationViewModel()
        {
            GoToPageCommand = new DelegateCommand<int?>(GoToPage);
        }

        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages =value; RaisePropertyChanged(); }
        }

        

        // 页码集合
        public ObservableCollection<int> PageNumbers { get; } = new();

        // 生成页码（带智能截断）
        public void UpdatePageNumbers()
        {
            PageNumbers.Clear();

            int start = Math.Max(1, CurrentPage - 2);
            int end = Math.Min(TotalPages, CurrentPage + 2);

            // 添加前导省略号
            if (start > 1) PageNumbers.Add(-1); // 用-1表示省略号

            for (int i = start; i <= end; i++)
                PageNumbers.Add(i);

            // 添加尾部省略号
            if (end < TotalPages) PageNumbers.Add(-1);
        }

        // 命令定义（增加可用性判断）
        public DelegateCommand FirstPageCommand => new DelegateCommand(
            () => CurrentPage = 1,
            () => CurrentPage > 1).ObservesProperty(() => CurrentPage);

        public DelegateCommand PreviousPageCommand => new DelegateCommand(
            () => CurrentPage--,
            () => CurrentPage > 1).ObservesProperty(() => CurrentPage);

        public DelegateCommand NextPageCommand => new DelegateCommand(
            () => CurrentPage++,
            () => CurrentPage < TotalPages).ObservesProperty(() => CurrentPage);

        public DelegateCommand LastPageCommand => new DelegateCommand(
            () => CurrentPage = TotalPages,
            () => CurrentPage < TotalPages).ObservesProperty(() => CurrentPage);

        public DelegateCommand<int?> GoToPageCommand { get; private set; }

        private void GoToPage(int? page)
        {
            if (page > 0 && page <= TotalPages)
                CurrentPage = (int)page;
        }

       
    }
}
