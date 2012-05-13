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
using System.ComponentModel;

namespace network_toolkit.ViewModels
{
    public class Chart : INotifyPropertyChanged
    {
        #region Initialize
        public event PropertyChangedEventHandler PropertyChanged;
        public string title { get; set; }
        private double _download;
        public double download { 
            get{
                return _download;
            }
            set{
                if (_download != value)
                {
                    _download = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("download"));
                }
            }
        }

        public Chart(string title)
        {
            this.title = title;
        }
        #endregion
    }
}
