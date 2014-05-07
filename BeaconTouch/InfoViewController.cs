using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BeaconLibrary;
using System.Collections.Generic;

namespace BeaconTouch
{
    public partial class InfoViewController : UIViewController
    {
        IndoorLocationController indoorLocationController;

        public InfoViewController() : base("InfoViewController", null)
        {
        }
            

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            var beaconManager = new BeaconManager();
            indoorLocationController = new IndoorLocationController(beaconManager);
            indoorLocationController.BeaconFound += HandleBeaconFound;
            indoorLocationController.BeaconLost += HandleBeaconLost;

            var beacon1 = DummyBeaconCreator.CreateBeaconUrsin();
            indoorLocationController.AddBeaconsAndStart(new List<Beacon>{beacon1});
        }

        void HandleBeaconFound (Beacon beacon)
        {
            UpdateDisplay(String.Format("Beacon with ID {0} found!",beacon.UniqueId), UIColor.Blue);
        }

        void HandleBeaconLost (Beacon beacon)
        {
            UpdateDisplay(String.Format("Beacon with ID {0} lost!",beacon.UniqueId), UIColor.DarkGray);
        }
        
        void UpdateDisplay(string message, UIColor color)
        {
            InvokeOnMainThread(delegate {
                FullView.BackgroundColor = color;
                InfoLabel.Text = message;
            });
        }
    }
}

