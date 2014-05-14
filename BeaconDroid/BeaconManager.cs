using System;
using BeaconLibrary;
using Android.App;
using System.Collections.Generic;
using RadiusNetworks.IBeaconAndroid;

namespace FindTheMonkey.Droid
{
    public class BeaconManager :  Java.Lang.Object, IBeaconHandler, IMonitorNotifier, IBeaconConsumer
    {
        public event Action<Beacon> EnterBeaconRegion;

        public event Action<Beacon> ExitBeaconRegion;

        public event Action<string> DebugInfo;

        IBeaconManager beaconManager;

        readonly Activity activity;

        private List<Beacon> beaconsToStart;

        public BeaconManager(Activity activity)
        {
            beaconsToStart = new List<Beacon>();

            this.activity = activity;

            beaconManager = IBeaconManager.GetInstanceForApplication(activity);
            beaconManager.Bind(this);
        }

        public bool StartObserveBeacon(Beacon beacon)
        {
            if (beaconManager.IsBound(this))
            {
                beaconManager.StartMonitoringBeaconsInRegion(RegionBeaconConverter.ConvertBeaconToRegion(beacon));
                return true;
            }
            else
            {
                beaconsToStart.Add(beacon);
                return false;
            }
        }

        public void StopObserveBeacon(Beacon beacon)
        {
            beaconManager.StopMonitoringBeaconsInRegion(RegionBeaconConverter.ConvertBeaconToRegion(beacon));
        }
       
        public void DidEnterRegion(Region region)
        {
            if (EnterBeaconRegion != null)
            {
                EnterBeaconRegion(RegionBeaconConverter.ConvertRegionToBeacon(region));
            }
        }

        public void DidExitRegion(Region region)
        {
            if (ExitBeaconRegion != null)
            {
                ExitBeaconRegion(RegionBeaconConverter.ConvertRegionToBeacon(region));
            }
        }
       

        public void OnIBeaconServiceConnect()
        {
            beaconManager.SetMonitorNotifier(this);

            foreach (Beacon beacon in beaconsToStart)
            {
                StartObserveBeacon(beacon);
            }

            beaconsToStart.Clear();
        }

        public void InitiateFullStop()
        {
            beaconManager.UnBind(this);
        }

        #region not important

        public void DidDetermineStateForRegion(int state, Region region) {}

        public bool BindService(Android.Content.Intent intent, Android.Content.IServiceConnection serviceConnection, Android.Content.Bind flags)
        {
            return activity.BindService(intent, serviceConnection, flags);
        }

        public void UnbindService(Android.Content.IServiceConnection p0)
        {
            activity.UnbindService(p0);
        }

        public Android.Content.Context ApplicationContext
        {
            get
            {
                return activity.ApplicationContext;
            }
        }
        #endregion
    }

    public static class RegionBeaconConverter
    {
        public static Beacon ConvertRegionToBeacon(Region region)
        {
            return new Beacon(new Guid(region.ProximityUuid));
        }

        public static Region ConvertBeaconToRegion(Beacon beacon)
        {
            return new Region(beacon.UniqueId.ToString(), beacon.UniqueId.ToString(), null, null);
        }
    }
}

