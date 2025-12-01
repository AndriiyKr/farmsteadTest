<<<<<<< HEAD
﻿// <copyright file="SplashScreenWindow.xaml.cs" company="PlaceholderCompany">
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
=======
﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace FarmsteadMap.WPF
{
    public partial class SplashScreenWindow : Window
    {
        private const int MIN_SPLASH_TIME_MS = 4000;
        public SplashScreenWindow()
        {
            InitializeComponent();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task<bool> internetCheckTask = CheckInternetConnectionAsync();
            Task progressTask = AnimateProgressAsync(MIN_SPLASH_TIME_MS);
            await Task.WhenAll(internetCheckTask, progressTask);

            this.Close();
        }
        private async Task AnimateProgressAsync(int durationMs)
        {
            const int steps = 100; 
            int delayPerStep = durationMs / steps;

            for (int i = 0; i <= steps; i++)
            {
                LoadingProgressBar.Value = i;
                await Task.Delay(delayPerStep);
            }
        }
        private async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    var response = await client.GetAsync("http://clients3.google.com/generate_204");
                    return response.IsSuccessStatusCode;
                }
>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
            }
            catch
            {
                return false;
            }
        }
<<<<<<< HEAD

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
=======
    }

>>>>>>> 6a304175c57de642982c922e554039d953aa8cb3
}