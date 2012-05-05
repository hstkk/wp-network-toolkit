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
using System.ComponentModel;
using System.Linq;

namespace network_toolkit.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Initialize
        public event PropertyChangedEventHandler PropertyChanged;

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

        /// <summary>
        /// Serializes FavoriteItems ObservableCollection for saving.
        /// </summary>
        /// <returns>Serialized FavoriteItems</returns>
        public string serialize()
        {
            string serialized = "";
            try
            {
                if (FavoriteItems.Count > 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObservableCollection<Menu>));
                        dataContractSerializer.WriteObject(memoryStream, FavoriteItems);
                        memoryStream.Position = 0;
                        serialized = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
            }
            return serialized;
        }

        /// <summary>
        /// Deserializes saved FavoriteItems to ObservableCollection.
        /// </summary>
        /// <param name="serialized">Serialized FavoriteItems</param>
        private void deserialize(string serialized)
        {
            try
            {
                if (!serialized.Equals(""))
                {
                    DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObservableCollection<Menu>));
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serialized)))
                    {
                        FavoriteItems = dataContractSerializer.ReadObject(memoryStream) as ObservableCollection<Menu>;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("FavoriteItems"));
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// Adds a new favorite item.
        /// </summary>
        /// <param name="title">Favorite title</param>
        /// <param name="url">Favorite url</param>
        public void addToFavorites(string title, string url)
        {
            if (!title.Equals("") && !url.Equals("") && (from m in FavoriteItems where m.Url == url select m).Count() == 0)
                FavoriteItems.Add(new Menu(title, url));
        }

        /// <summary>
        /// Remove a favorite item.
        /// </summary>
        /// <param name="menu">Favorite item</param>
        public void deleteFromFavorites(Menu menu)
        {
            if(menu != null)
                FavoriteItems.Remove(menu);
        }
        #endregion
    }
}
