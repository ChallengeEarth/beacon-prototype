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

        public event Action<string> DebugInfo;

        CLLocationManager locationManager;

        public BeaconManager()
        {
            locationManager = new CLLocationManager ();

            locationManager.DidStartMonitoringForRegion += HandleDidStartMonitoringForRegion;
            locationManager.DidDetermineState += HandleDidDetermineState;


            locationManager.MonitoringFailed += HandleMonitoringFailed;
        }


        void HandleMonitoringFailed (object sender, CLRegionErrorEventArgs e)
        {
            OnDebugInfo(String.Format("HandleMonitoringFailed {0}",e.Error));
        }

        void HandleDidStartMonitoringForRegion (object sender, CLRegionEventArgs e)
        {
            locationManager.RequestState(e.Region);
        }

        void HandleDidDetermineState (object sender, CLRegionStateDeterminedEventArgs e)
        {
            OnDebugInfo(String.Format("State for region {0} is {1}", e.Region.Identifier, e.State));

            if (e.State == CLRegionState.Inside)
            {
                OnEnterRegion(e.Region);
            }

            if (e.State == CLRegionState.Outside)
            {
                OnExitRegion(e.Region);
            }
        }
            
        public bool StartObserveBeacon(Beacon beacon)
        {
            locationManager.StartMonitoring(RegionBeaconConverter.ConvertBeaconToRegion(beacon));
            return true;
        }

        public void StopObserveBeacon(Beacon beacon)
        {
            locationManager.StopMonitoring(RegionBeaconConverter.ConvertBeaconToRegion(beacon));
        }

        public void InitiateFullStop()
        {
            //Nothing to do here on iOS...
        }

        void OnExitRegion(CLRegion region)
        {
            if (ExitBeaconRegion != null)
            {
                var beaconRegion = region as CLBeaconRegion;

                if (beaconRegion != null)
                    ExitBeaconRegion(RegionBeaconConverter.ConvertRegionToBeacon(beaconRegion));
                else
                    throw new FormatException("Beacon has wrong format!");
            }
        }

        void OnEnterRegion(CLRegion region)
        {
            if (EnterBeaconRegion != null)
            {
                var beaconRegion = region as CLBeaconRegion;

                if (beaconRegion != null)
                    EnterBeaconRegion(RegionBeaconConverter.ConvertRegionToBeacon(beaconRegion));
                else
                    throw new FormatException("Beacon has wrong format!");
            }
        }

        void OnDebugInfo(String info)
        {
            if (EnterBeaconRegion != null)
            {
                DebugInfo(info);
            }
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
            var region = new CLBeaconRegion(new NSUuid(beacon.UniqueId.ToString()), beacon.UniqueId.ToString());

            region.NotifyOnEntry = true;
            region.NotifyOnExit = true;
            region.NotifyEntryStateOnDisplay = true;

            return region;
        }
    }
}

