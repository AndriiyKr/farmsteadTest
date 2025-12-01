// <copyright file="SplashScreenWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Логіка взаємодії для SplashScreenWindow.xaml.
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        private const int MinSplashTimeMs = 4000;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreenWindow"/> class.
        /// </summary>
        public SplashScreenWindow()
        {
            this.InitializeComponent();
        }

        private static async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(3);
                var response = await client.GetAsync("http://clients3.google.com/generate_204");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task<bool> internetCheckTask = CheckInternetConnectionAsync();
            Task progressTask = this.AnimateProgressAsync(MinSplashTimeMs);
            await Task.WhenAll(internetCheckTask, progressTask);

            this.Close();
        }

        private async Task AnimateProgressAsync(int durationMs)
        {
            const int steps = 100;
            int delayPerStep = durationMs / steps;

            for (int i = 0; i <= steps; i++)
            {
                this.LoadingProgressBar.Value = i;
                await Task.Delay(delayPerStep);
            }
        }
    }
}