//2012 Sami Hostikka <dev@01.fi>
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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.IO.IsolatedStorage;

namespace network_toolkit.ViewModels
{
    public class MainViewModel
    {
        #region Initialize
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
        #endregion

        #region Etc
        /// <summary>
        /// Creates and adds a few Menu objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            string favorites = "";
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings.TryGetValue<string>("favorites", out favorites);
            deserialize(favorites);

            this.MenuItems.Add(new Menu("port scanner", "/port-scanner.xaml"));
            this.MenuItems.Add(new Menu("port knocker", "/port-knocker/browse.xaml"));
            this.MenuItems.Add(new Menu("speed-test", "/speed-test.xaml"));
            this.IsDataLoaded = true;
        }

        public string serialize()
        {
            string serialized = "";
            try
            {
                /*if (FavoriteItems.Count > 0)
                {*/
                MessageBox.Show(FavoriteItems.Count + "");
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObservableCollection<Menu>));
                        dataContractSerializer.WriteObject(memoryStream, FavoriteItems);
                        memoryStream.Position = 0;
                        serialized = streamReader.ReadToEnd();
                        MessageBox.Show(serialized);
                    }
                //}
            }
            catch (Exception e)
            {
            }
            return serialized;
        }

        private void deserialize(string serialized)
        {
            try
            {
                MessageBox.Show(serialized);
                if (!serialized.Equals(""))
                {
                    DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObservableCollection<Menu>));
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serialized)))
                    {
                        FavoriteItems = dataContractSerializer.ReadObject(memoryStream) as ObservableCollection<Menu>;
                    }
                    MessageBox.Show(FavoriteItems.Count + "");
                    MessageBox.Show("kk");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void addToFavorites(string title, string url)
        {
            FavoriteItems.Add(new Menu(title, url));
        }
        #endregion
    }
}
