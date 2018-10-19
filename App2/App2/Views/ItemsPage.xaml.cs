using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App2.Models;
using App2.Views;
using App2.ViewModels;
using Rg.Plugins.Popup.Services;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();

        }

        public ItemsPage(string data)
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel(data,"a");

        }

        private void ButtonEntrada_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PopupView("Entrada"));

        }

        private void ButtonSalida_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PopupView("Salida"));

        }
    }
}