using System;
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
            }
            catch
            {
                return false;
            }
        }
    }

}