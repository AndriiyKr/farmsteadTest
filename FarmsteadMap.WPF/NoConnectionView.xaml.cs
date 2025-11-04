using System;
using System.Windows;
using System.Windows.Controls;

namespace FarmsteadMap.WPF
{
    public partial class NoConnectionView : UserControl
    {
        public event EventHandler RetryClicked;

        public NoConnectionView()
        {
            InitializeComponent();
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            RetryClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}