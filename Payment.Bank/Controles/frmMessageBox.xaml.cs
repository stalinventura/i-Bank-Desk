using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Xml.Linq;

namespace Payment.Bank.Controles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmMessageBox : MetroWindow
    {
        public enum MessageType { Info, Error, Alert };
        public string Input { get; set; }
        private const string ICONS_PATH = "/Controles/Icon/";
        public frmMessageBox()
        {
            InitializeComponent();
        }

        private void setMessageIcon(string imagePath)
        {
            MessageIcon.Source = new BitmapImage(new Uri(ICONS_PATH + imagePath, UriKind.RelativeOrAbsolute));
        }

        public void OnInit(string message, string Title, MessageType type, String[] inputOptions = null)
        {
            try
            {
                switch (type)
                {
                    case MessageType.Info:
                        setMessageIcon("Info.png");
                        break;
                    case MessageType.Error:
                        setMessageIcon("Cancel.png");
                        break;
                    case MessageType.Alert:
                        setMessageIcon("Alert.png");
                        break;
                }
                this.Title = Title;
                this.TextBlock.Text = message;
            }
            catch { }
        }


    }
}
