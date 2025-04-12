using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EstateMapperClient.Common
{
    public class PasswordHelper : DependencyObject
    {
        public static readonly DependencyProperty Password = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordHelper),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged)
        );

        private static void OnBoundPasswordChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            PasswordBox passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            SetPassword(box, box.Password);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(Password);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(Password, value);
        }
    }
}
