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

namespace network_toolkit
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        /// <summary>
        /// Load data for the ViewModel Items.
        /// </summary>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        /// <summary>
        /// Handle selection changed on ListBox
        /// </summary>
/*        private void toolListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (toolListBox.SelectedIndex == -1)
                return;


            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));

            // Reset selected index to -1 (no selection)
            toolListBox.SelectedIndex = -1;
        }*/

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string url = (sender as Button).Tag.ToString();
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }
    }
}