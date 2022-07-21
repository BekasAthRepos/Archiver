using Archiver.Service;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archiver
{
    public partial class App : Application
    {
        private static Database connection;
        public static Database Database
        {
            get
            {
                if (connection == null)
                {
                    connection = new Database(Path.Combine(Environment.
                                     GetFolderPath(Environment.SpecialFolder
                                     .LocalApplicationData), "archiver.db3"));
                }
                return connection;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
