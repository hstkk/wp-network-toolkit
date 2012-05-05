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
using System.Runtime.Serialization;

namespace network_toolkit.ViewModels
{
    [DataContract]
    public class Menu
    {
        #region Initialize
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Url { get; set; }

        public Menu(string Title, string Url)
        {
            this.Title = Title;
            this.Url = Url;
        }
        #endregion
    }
}
