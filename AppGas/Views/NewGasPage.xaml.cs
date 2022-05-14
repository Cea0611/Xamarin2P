using AppGas.Models;
using AppMaps.ViewModels;
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
    public partial class NewGasPage : ContentPage
    {
        public Item Gas { get; set; }
        public NewGasPage()
        {
            InitializeComponent();
            BindingContext = new NewGasViewModel();
        }
    }
}