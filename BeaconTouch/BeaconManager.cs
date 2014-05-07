using System;
using BeaconLibrary;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;

namespace BeaconTouch
{
    public class BeaconManager : IBeaconHandler
    {
        public event Action<Beacon> EnterBeaconRegion;

        public event Action<Beacon> ExitBeaconRegion;

        CLLocationManager locationMgr;

        public BeaconManager()
        {
            locationMgr = new CLLocationManager ();

            locationMgr.RegionEntered += HandleRegionEntered;
            locationMgr.RegionLeft += HandleRegionLeft;


        }

        void HandleRegionEntered (object sender, CLRegionEventArgs e)
        {
            if (EnterBeaconRegion != null)
            {
                EnterBeaconRegion(RegionBeaconConverter.ConvertRegionToBeacon((CLBeaconRegion)e.Region));
            }
        }   

        void HandleRegionLeft (object sender, CLRegionEventArgs e)
        {
            if (ExitBeaconRegion != null)
            {
                ExitBeaconRegion(RegionBeaconConverter.ConvertRegionToBeacon((CLBeaconRegion)e.Region));
            }
        }
            
        public bool StartObserveBeacon(Beacon beacon)
        {
            locationMgr.StartMonitoring(RegionBeaconConverter.ConvertBeaconToRegion(beacon));
            return true;
        }

        public void StopObserveBeacon(Beacon beacon)
        {
            locationMgr.StopMonitoring(RegionBeaconConverter.ConvertBeaconToRegion(beacon));
        }

        public void InitiateFullStop()
        {
            //Nothing to do here on iOS...
        }
    }

    public static class RegionBeaconConverter
    {
        public static Beacon ConvertRegionToBeacon(CLBeaconRegion region)
        {
            return new Beacon(new Guid(region.ProximityUuid.AsString()));
        }

        public static CLBeaconRegion ConvertBeaconToRegion(Beacon beacon)
        {
            return new CLBeaconRegion(new NSUuid(beacon.UniqueId.ToString()), beacon.UniqueId.ToString());
        }
    }
}

