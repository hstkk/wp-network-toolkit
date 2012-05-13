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
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace network_toolkit
{
    public partial class Speed_test : PhoneApplicationPage
    {
        WebClient webClient;
        bool textfieldGotFocus = false;
        bool firstDownloadProgressChanged;
        long size;
        ObservableCollection<SpeedTest> history;
        CollectionViewSource collectionViewSource;
        public Speed_test()
        {
            InitializeComponent();
            webClient = new WebClient();
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(downloadCompleted);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChanged);

            history = new ObservableCollection<SpeedTest>(Dataprovider.getSpeedTests());
            collectionViewSource = new CollectionViewSource();

            try
            {
                collectionViewSource.SortDescriptions.Add(new System.ComponentModel.SortDescription("Created", System.ComponentModel.ListSortDirection.Descending));
                collectionViewSource.Source = history;
            }
            catch (Exception e)
            {
            }

            listBox.DataContext = collectionViewSource;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedIndex == 0)
                ApplicationBar.Mode = ApplicationBarMode.Default;
            else
                ApplicationBar.Mode = ApplicationBarMode.Minimized;
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
            if (size < 5000000)
            {
                try
                {
                    double start;
                    if (double.TryParse(e.UserState.ToString(), out start))
                    {
                        download.Visibility = System.Windows.Visibility.Visible;
                        
                        double result = size / 1000000 / ((end - start) / 1000);
                        SpeedTest speedTest = new SpeedTest();
                        speedTest.Created = DateTime.Now;
                        speedTest.Download = result;
                        Dataprovider.addSpeedTest(speedTest);
                        history.Add(speedTest);
                        speed.Text = result.ToString("0.00") + " Mbps";
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                speed.Text = "";
                download.Visibility = System.Windows.Visibility.Collapsed;
            }
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            testFile.IsEnabled = true;
        }

        private void downloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (firstDownloadProgressChanged)
            {
                firstDownloadProgressChanged = false;
                size = e.TotalBytesToReceive;
                if (size > 5000000)
                {
                    try
                    {
                        webClient.CancelAsync();
                    }
                    catch (Exception ex)
                    {
                    }
                    MessageBox.Show("Test file is too big!");
                }
            }
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

        private void applicationBarMenuItem_Click(object sender, EventArgs e)
        {
            Dataprovider.clearDatabase();
            history.Clear();
        }
    }
}