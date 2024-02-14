using Archiver.Service;
using Archiver.View;
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
                if (connection != null)
                    return connection;
                else
                    return null;
            }
        }

        private static AdManager adManager;
        public static AdManager AdManager
        {
            get
            {
                if (adManager != null)
                    return adManager;
                else
                    return null;
            }
        }

        public App()
        {
            connection = new Database(Path.Combine(Environment.
                         GetFolderPath(Environment.SpecialFolder
                         .LocalApplicationData), "archiver.db3"));
            InitializeComponent();
            adManager = new AdManager();
            MainPage = new NavigationPage(new WelcomePage());

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
