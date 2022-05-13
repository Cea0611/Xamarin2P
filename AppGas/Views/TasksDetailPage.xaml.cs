using AppGas.Models;
using AppGas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGas.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksDetailPage : ContentPage
    {
        public TasksDetailPage()
        {
            InitializeComponent();

            BindingContext = new TasksDetailViewModel();
        }

        public TasksDetailPage(TaskModel taskSelected)
        {
            InitializeComponent();

            BindingContext = new TasksDetailViewModel(taskSelected);
        }
    }
}