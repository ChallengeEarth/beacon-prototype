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
using Android.Content;
using Android.Support.V4.App;
using Android.Media;
using Android.Text.Method;

namespace FindTheMonkey.Droid
{
    [Activity(Label = "Drallo Beacon Finder", 
        MainLauncher = true, 
        LaunchMode = LaunchMode.SingleTask,
        ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
        private const string INITIAL_MESSAGE = "There aren't any Beacons nearby";

        View _view;
		TextView _text;

        IndoorLocationController indoorLocationController;

        bool paused = false;
    
		public MainActivity()
		{
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

            _view = FindViewById<LinearLayout>(Resource.Id.parentLayout);
            _text = FindViewById<TextView>(Resource.Id.logTextView);

            UpdateDisplay(INITIAL_MESSAGE,Color.White);

            var beaconManager = new BeaconManager(this);

            indoorLocationController = new IndoorLocationController(beaconManager);
            indoorLocationController.BeaconFound += HandleBeaconFound;
            indoorLocationController.BeaconLost += HandleBeaconLost;


            var beacon1 = DummyBeaconCreator.CreateBeaconUrsin();
            var beacon2 = DummyBeaconCreator.CreateBeaconMarcel();
            indoorLocationController.AddBeaconsAndStart(new List<Beacon>{beacon1,beacon2});

            
            if (!IsBluetoothAvailable)
            {
                Toast.MakeText(this, "Please activate Bluetooth! iBeacons works only when Bluetooth available.",ToastLength.Long).Show();
            }
		}

        void HandleBeaconFound (Beacon beacon)
        {
            if (paused)
            {
                ShowNotification();
            }

            UpdateDisplay(String.Format("Beacon with ID {0} found!",beacon.UniqueId), Color.Green);
        }

        void HandleBeaconLost (Beacon beacon)
        {
            if (paused)
            {
                ShowNotification();
            }

            UpdateDisplay(String.Format("Beacon with ID {0} lost!",beacon.UniqueId), Color.Red);  
        }


		private void UpdateDisplay(string message, Color color)
		{
			RunOnUiThread(() =>
			{
                string formatedMessage = String.Format("{0}: {1}",DateTime.Now.ToString("T"),message) + System.Environment.NewLine + System.Environment.NewLine;

                if(_text.Text.Equals(INITIAL_MESSAGE))
                {
                    _text.Text = string.Empty;
                }

                _text.Text += formatedMessage;

                _view.SetBackgroundColor(color);
			});
		}

        private void ShowNotification()
        {
            int requestId = DateTime.Now.Millisecond;
          
            var resultIntent = new Intent(this, typeof(MainActivity));
            resultIntent.AddFlags(ActivityFlags.ReorderToFront);
            var pendingIntent = PendingIntent.GetActivity(this, requestId, resultIntent, PendingIntentFlags.UpdateCurrent);
            var notificationId = Resource.String.monkey_notification;
            var alarmSound = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

            var builder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.Xamarin_Icon)
                .SetContentTitle("Drallo Beacon Finder")
                .SetContentText("A new Beacon-Event!")
                .SetContentIntent(pendingIntent)
                .SetSound(alarmSound)
                .SetAutoCancel(true);

            var notification = builder.Build();

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Notify(notificationId, notification);
        }

        protected override void OnStop()
        {
            base.OnStop();
            paused = true;
        }

        protected override void OnResume()
        {
            base.OnResume();
            paused = false;
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