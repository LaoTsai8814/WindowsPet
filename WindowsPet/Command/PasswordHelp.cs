using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WindowsPet.Command
{
    public class PasswordHelp
    {


        public static string GetPasswd(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswdProperty);
        }

        public static void SetPasswd(DependencyObject obj, string value)
        {
            obj.SetValue(PasswdProperty, value);
        }

        // Using a DependencyProperty as the backing store for Passwd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswdProperty =
            DependencyProperty.RegisterAttached("Passwd", typeof(string), typeof(PasswordHelp), new PropertyMetadata(string.Empty, OnPasswdChanged));

        private static void OnPasswdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (!GetIsUpdating(passwordBox))
                {
                    passwordBox.Password = e.NewValue?.ToString() ?? "";
                }
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }

            //throw new NotImplementedException();
        }
        private static readonly DependencyProperty IsUpdatingProperty =
        DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelp));

        private static bool GetIsUpdating(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject obj, bool value)
        {
            obj.SetValue(IsUpdatingProperty, value);
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPasswd(passwordBox, passwordBox.Password);
            passwordBox.SetValue(PasswdProperty, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }


        #region Use On Register Account
        public static string GetConfirmPasswd(DependencyObject obj)
        {
            return (string)obj.GetValue(ConfirmPasswdProperty);
        }

        public static void SetConfirmPasswd(DependencyObject obj, string value)
        {
            obj.SetValue(ConfirmPasswdProperty, value);
        }

        // Using a DependencyProperty as the backing store for ConfirmPasswd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmPasswdProperty =
            DependencyProperty.RegisterAttached("ConfirmPasswd", typeof(string), typeof(PasswordHelp), new PropertyMetadata(string.Empty, OnPasswdChanged));
        #endregion


    }
}
