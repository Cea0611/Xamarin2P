using AppGas.Data;
using AppGas.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGas
{
    public partial class App : Application
    {
        // Propiedad estatica para instanciar y regresar la variable de SQLite
        private static TaskDatabase tasksDatabase;
        public static TaskDatabase TaskDatabase
        {
            get
            {
                if (tasksDatabase == null) tasksDatabase = new TaskDatabase();
                return tasksDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            NavigationPage nav = new NavigationPage(new TaskListPage());
            nav.BarBackgroundColor = (Color)App.Current.Resources["BackgroundColor"];
            nav.BarTextColor = (Color)App.Current.Resources["BarTextColor"];
            MainPage = nav;
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
