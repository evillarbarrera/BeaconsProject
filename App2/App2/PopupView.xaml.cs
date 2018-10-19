using App2.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupView
	{
        ItemsViewModel viewModel;

        public PopupView (string Tipo)
		{
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel(Tipo);

        }

        public string Tipo { get; set; }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }

    }
}