using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using App2.Models;
using App2.Views;

namespace App2.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ItemsViewModel()
        {
            Title = "--Beacon Test--";
            Rut = "18.756.415-8";
            Nombre = "Juan Pablo Ibañez Dastres";
            Funcion = "Trabajador";

        }

        public ItemsViewModel(string t) {
            Tipo = t;
            Fecha = DateTime.Now.ToString();
        }

        public ItemsViewModel(string data, string prueba)
        {
            Title = data;
        }
    }
}