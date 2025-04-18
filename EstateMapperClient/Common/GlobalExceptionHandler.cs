using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.Logging;
using Prism.Services.Dialogs;

namespace EstateMapperClient.Common
{
    public class GlobalExceptionHandler : IGlobalExceptionHandler
    {
        private readonly ILogger logger;
        private readonly IDialogService dialogService;

        public GlobalExceptionHandler(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public void HandleException(Exception ex)
        {
            //记录异常日志
            //logger.LogError(
            //    $"异常类型: {ex.GetType().Name}\n消息: {ex.Message}\n堆栈: {ex.StackTrace}"
            //);
            switch (ex)
            {
                case AccessViolationException access:
                    AccessError(access);
                    break;
            }
        }

        private void AccessError(AccessViolationException access)
        {
            Application.Current.MainWindow.Hide();
            var prarm = new DialogParameters();
            prarm.Add("value",access.Message);
            Application.Current.Dispatcher.Invoke(() => dialogService.ShowDialog("Login", prarm, result => { }));
            Application.Current.MainWindow.Show();
        }
    }
}
