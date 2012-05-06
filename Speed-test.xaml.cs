using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text.RegularExpressions;
using System.IO;

namespace network_toolkit
{
    public partial class Speed_test : PhoneApplicationPage
    {
        bool textfieldGotFocus = false;
        bool firstDownloadProgressChanged;
        long size;
        public Speed_test()
        {
            InitializeComponent();
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedIndex == 0)
                ApplicationBar.IsVisible = true;
            else
                ApplicationBar.IsVisible = false;
        }

        private void download_Click(object sender, EventArgs e)
        {
            if ((ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled)
            {
                progressBar.Value = 0;
                progressBar.Visibility = System.Windows.Visibility.Visible;
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
                err.Visibility = System.Windows.Visibility.Collapsed;
                testFile.IsEnabled = false;
                try
                {
                    firstDownloadProgressChanged = true;
                    size = 0;
                    WebClient webClient = new WebClient();
                    webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(downloadCompleted);
                    webClient.DownloadProgressChanged +=new DownloadProgressChangedEventHandler(downloadProgressChanged);
                    webClient.OpenReadAsync(new Uri(testFile.Text, UriKind.Absolute), Environment.TickCount);
                }
                catch (Exception ex)
                {
                    err.Visibility = System.Windows.Visibility.Visible;
                    progressBar.Visibility = System.Windows.Visibility.Collapsed;
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
                    testFile.IsEnabled = true;
                }
            }
        }

        private void downloadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            double end = Environment.TickCount;
            try{
                double start;
                if (double.TryParse(e.UserState.ToString(), out start))
                {
                    download.Visibility = System.Windows.Visibility.Visible;
                    speed.Text = (size / 1000000 / ((end - start) / 1000)).ToString("0.00") + " Mbps";
                }
            }
            catch (Exception ex)
            {
            }
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            testFile.IsEnabled = true;
        }

        private void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (firstDownloadProgressChanged)
                size = e.TotalBytesToReceive;
        }

        private void testFile_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textfieldGotFocus)
            {
                if (!Regex.IsMatch(testFile.Text, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"))
                {
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
                    MessageBox.Show("URL is not valid");
                }
                else
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            }
        }

        private void testFile_GotFocus(object sender, RoutedEventArgs e)
        {
            textfieldGotFocus = true;
        }
    }
}