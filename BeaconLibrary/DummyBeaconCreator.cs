using System;

namespace BeaconLibrary
{
    public static class DummyBeaconCreator
    {
        public static Beacon CreateBeaconUrsin()
        {
            var uuid = new Guid("f7826da6-4fa2-4e98-8024-bc5b71e0893e");

            return new Beacon(uuid);
        }
    }
}

