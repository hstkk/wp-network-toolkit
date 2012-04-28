using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace network_toolkit.ViewModels
{
    public class MainViewModel
    {
        /// <summary>
        /// Initialises Items list.
        /// </summary>
        public MainViewModel()
        {
            this.MenuItems = new ObservableCollection<Menu>();
            this.FavoriteItems = new ObservableCollection<Menu>();

        }

        /// <summary>
        /// A collection for Menu objects.
        /// </summary>
        public ObservableCollection<Menu> MenuItems { get; private set; }

        /// <summary>
        /// A collection for Favorite objects.
        /// </summary>
        public ObservableCollection<Menu> FavoriteItems { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few Menu objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            this.MenuItems.Add(new Menu("port scanner", "port-scanner.xaml"));
            this.MenuItems.Add(new Menu("port knocker", "port-knocker.xaml"));
            this.MenuItems.Add(new Menu("speed-test", "speed-test.xaml"));
            this.IsDataLoaded = true;
        }
    }
}
