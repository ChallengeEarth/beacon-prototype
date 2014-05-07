using System;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using Color = Android.Graphics.Color;
using BeaconLibrary;
using System.Collections.Generic;
using Android.Bluetooth;

namespace FindTheMonkey.Droid
{
    [Activity(Label = "DralloBeaconPrototype", MainLauncher = true, LaunchMode = LaunchMode.SingleTask)]
	public class MainActivity : Activity
	{
		View _view;
		TextView _text;

        IndoorLocationController indoorLocationController;
    
		public MainActivity()
		{
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			_view = FindViewById<RelativeLayout>(Resource.Id.findTheMonkeyView);
			_text = FindViewById<TextView>(Resource.Id.monkeyStatusLabel);

            var beaconManager = new BeaconManager(this);

            indoorLocationController = new IndoorLocationController(beaconManager);
            indoorLocationController.BeaconFound += HandleBeaconFound;
            indoorLocationController.BeaconLost += HandleBeaconLost;


            var beacon1 = DummyBeaconCreator.CreateBeaconUrsin();
            indoorLocationController.AddBeaconsAndStart(new List<Beacon>{beacon1});

            
            if (!IsBluetoothAvailable)
            {
                Toast.MakeText(this, "Please activate Bluetooth! iBeacons works only when Bluetooth available.",ToastLength.Long).Show();
            }
		}

        void HandleBeaconFound (Beacon beacon)
        {
            UpdateDisplay(String.Format("Beacon with ID {0} found!",beacon.UniqueId), Color.Blue);
        }

        void HandleBeaconLost (Beacon beacon)
        {
            UpdateDisplay(String.Format("Beacon with ID {0} lost!",beacon.UniqueId), Color.Beige);  
        }


		private void UpdateDisplay(string message, Color color)
		{
			RunOnUiThread(() =>
			{
				_text.Text = message;
				_view.SetBackgroundColor(color);
			});
		}


		protected override void OnDestroy()
		{
			base.OnDestroy();

            indoorLocationController.StopObservingBeacons();

            indoorLocationController.BeaconFound -= HandleBeaconFound;
            indoorLocationController.BeaconLost -= HandleBeaconLost;
		}

        public static bool IsBluetoothAvailable
        {
            get
            {
                return BluetoothAdapter.DefaultAdapter != null && BluetoothAdapter.DefaultAdapter.IsEnabled;
            }
        }
	}
}