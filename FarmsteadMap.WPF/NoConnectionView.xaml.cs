// <copyright file="NoConnectionView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Забезпечує логіку взаємодії для NoConnectionView.xaml (Екран відсутності з'єднання).
    /// </summary>
    public partial class NoConnectionView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoConnectionView"/> class.
        /// </summary>
        public NoConnectionView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Подія, що спрацьовує при натисканні кнопки "Повторити".
        /// </summary>
        public event EventHandler? RetryClicked; // Виправлено CS8618 та SA1201

        /// <summary>
        /// Обробник натискання кнопки "Повторити".
        /// </summary>
        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            this.RetryClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}