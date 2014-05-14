using System;

namespace BeaconLibrary
{
    public interface IBeaconHandler
    {
        event Action<Beacon> EnterBeaconRegion;
        event Action<Beacon> ExitBeaconRegion;
        event Action<String> DebugInfo;

        bool StartObserveBeacon(Beacon beacon);
        void StopObserveBeacon(Beacon beacon);
        void InitiateFullStop();
    }
}

