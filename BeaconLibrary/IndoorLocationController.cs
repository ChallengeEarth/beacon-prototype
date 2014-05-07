using System;
using System.Collections.Generic;

namespace BeaconLibrary
{
    public class IndoorLocationController
    {
        readonly IBeaconHandler beaconHandler;

        public List<Beacon> BeaconsToObserve { get; private set;}

        public event Action<Beacon> BeaconFound;
        public event Action<Beacon> BeaconLost;

        public IndoorLocationController(IBeaconHandler beaconManager)
        {
            BeaconsToObserve = new List<Beacon>();
            this.beaconHandler = beaconManager;

            beaconManager.EnterBeaconRegion += HandleEnterBeaconRegion;
            beaconManager.ExitBeaconRegion += HandleExitBeaconRegion;
        }

        void HandleEnterBeaconRegion (Beacon beacon)
        {
            //TODO: generate here an activityEvent and pass it to the challengeEnginge

            if (BeaconFound != null)
            {
                BeaconFound(beacon);
            }
        }

        void HandleExitBeaconRegion (Beacon beacon)
        {
            //TODO: I think we don't need this event here now, but its nice to test

            if (BeaconLost != null)
            {
                BeaconLost(beacon);
            }
        }
            
        public void AddBeaconsAndStart(List<Beacon> newBeacons)
        {
            BeaconsToObserve.AddRange(newBeacons);

            foreach (Beacon b in newBeacons)
            {
                beaconHandler.StartObserveBeacon(b);
            }
               
        }

        public void RemoveBeacon(Beacon beacon)
        {
            this.BeaconsToObserve.Remove(beacon);
            this.beaconHandler.StopObserveBeacon(beacon);
        }



        public void StopObservingBeacons()
        {
            foreach (Beacon b in BeaconsToObserve)
            {
                beaconHandler.StopObserveBeacon(b);
            }

            BeaconsToObserve.Clear();
            beaconHandler.InitiateFullStop();
        }
    }
}

