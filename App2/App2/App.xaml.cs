using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App2.Views;
using Estimotes;
using Plugin.BLE;
using Android.Bluetooth;
using Plugin.BLE.Abstractions.Contracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App2
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://localhost:5000";
        public static bool UseMockDataStore = true;
        private const string EstimoteUuid = @"B9407F30-F5F8-466E-AFF9-25556B57FE7D";
        private static string UltimoBeaconsLeido = string.Empty;
        private static string BeaconsLeido = string.Empty;

        private static int BeaconAnterior = -2;
        private static int BeaconActual= 0;

        private static bool task = true;

        IBluetoothLE bluetoothBLE;

        public App()
        {

            InitializeComponent();

            MainPage = new MainPage();

        }

        protected override async void OnStart()
        {
            try
            {
                bluetoothBLE = CrossBluetoothLE.Current;
                var status = await EstimoteManager.Instance.Initialize();

                if (status == BeaconInitStatus.BluetoothOff || status == BeaconInitStatus.BluetoothMissing) {
                    //obligar a encender el bluetoot para iniciar la aplicacion

                    if (bluetoothBLE.State != BluetoothState.On)
                    {
                        await Current.MainPage.DisplayAlert("Atencion", "Bluetooth deshabilitado, presione Ok para habilitar.", "OK");

                        BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                        bluetoothAdapter.Enable();

                        status = await EstimoteManager.Instance.Initialize();
                    }


                }
                if (status == BeaconInitStatus.Success)
                {
                    EstimoteManager.Instance.Ranged += Instance_Ranged;
                    EstimoteManager.Instance.RegionStatusChanged += Instance_RegionStatusChanged;
                    EstimoteManager.Instance.StartRanging(new BeaconRegion("beacon_Ksec", EstimoteUuid));

                }

                else
                {
                    await Current.MainPage.DisplayAlert("Atencion", "No se a detectado un dispositivo de validación cercano, mientras este no sea detectado no podra iniciar la aplicación, acerquese a un dispositivo y vuelva a iniciar la aplicación", "OK");
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

                }

                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    validar();
                    return task;
                });

            }
            catch (Exception ex) {
                await Current.MainPage.DisplayAlert("Atención", "La aplicacion no se puede ejecutar si no se encuentra hablilitado y/o disponible el Bluetooth", "Ok");
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }

        }

        private void Instance_RegionStatusChanged(object sender, BeaconRegionStatusChangedEventArgs e)
        {

        }

        private async void validar(){

            if (BeaconAnterior == BeaconActual)
            {
                task = false;
                await Current.MainPage.DisplayAlert("Atención", "No se han encontrado beacon disponibles y la aplicacion se detendra", "Ok");
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

            }

            BeaconAnterior++;

        }


        private void Instance_Ranged(object sender, System.Collections.Generic.IEnumerable<IBeacon> e)
        {
            try
            {
                BeaconActual++;
                var data = string.Empty;
                foreach (var beacon in e) {

                    if (beacon.Major == 5555)
                    {

                        data = $@"Hora: {DateTime.Now.TimeOfDay}
                                Major: {beacon.Major}
                                Minor: {beacon.Minor}
                                Proximity: {beacon.Proximity}";

                        BeaconsLeido = $@"{beacon.Major}{beacon.Minor}";
                        if (UltimoBeaconsLeido != BeaconsLeido)
                        {
                            UltimoBeaconsLeido = BeaconsLeido;
                            Current.MainPage.DisplayAlert("Exito", $@"Beacon encontrado {data}", "OK");
                        }
                    }
                    else {

                        Current.MainPage.DisplayAlert("Atencion", "No se a detectado un dispositivo de validación cercano, mientras este no sea detectado no podra iniciar la aplicación, acerquese a un dispositivo y vuelva a iniciar la aplicación", "OK");
                        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                    }

                    break;
                   }

            }
            catch (Exception ex)
            {


            }

        }

        protected override void OnSleep()
        {
            EstimoteManager.Instance.StopMonitoring(new BeaconRegion("beacon_Ksec", EstimoteUuid));
        }

        protected override async void OnResume()
        {
            try
            {

                bluetoothBLE = CrossBluetoothLE.Current;
                var status = await EstimoteManager.Instance.Initialize();

                if (status == BeaconInitStatus.BluetoothOff || status == BeaconInitStatus.BluetoothMissing)
                {
                    //obligar a encender el bluetoot para iniciar la aplicacion

                    if (bluetoothBLE.State != BluetoothState.On)
                    {
                        await Current.MainPage.DisplayAlert("Atencion", "Bluetooth deshabilitado, presione Ok para habilitar.", "OK");

                        BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                        bluetoothAdapter.Enable();

                        status = await EstimoteManager.Instance.Initialize();
                    }


                }
                if (status == BeaconInitStatus.Success)
                {
                    EstimoteManager.Instance.Ranged += Instance_Ranged;
                    EstimoteManager.Instance.RegionStatusChanged += Instance_RegionStatusChanged;
                    EstimoteManager.Instance.StartRanging(new BeaconRegion("beacon_Ksec", EstimoteUuid));
                    
                }

                else
                {
                    await Current.MainPage.DisplayAlert("Atencion", "No se a detectado un dispositivo de validación cercano, mientras este no sea detectado no podra iniciar la aplicación, acerquese a un dispositivo y vuelva a iniciar la aplicación", "OK");
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

                }
            }
            catch (Exception ex)
            {
                await Current.MainPage.DisplayAlert("Atención", "La aplicacion no se puede ejecutar si no se encuentra hablilitado y/o disponible el Bluetooth", "Ok");
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }

    }
}
