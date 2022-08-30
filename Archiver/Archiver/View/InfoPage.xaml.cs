using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        private ResourceManager rm;

        public InfoPage()
        {
            InitializeComponent();
            rm = new ResourceManager("Archiver.Resources.Strings", this.GetType().Assembly);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(rm.GetString("references"));
            refLbl.Text = sb.ToString();
        }
    }
}