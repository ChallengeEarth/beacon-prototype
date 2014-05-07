using System;

namespace BeaconLibrary
{
    public class Beacon
    {
        public Guid UniqueId { get; private set;}

        public Beacon(Guid uniqueId)
        {   
            UniqueId = uniqueId;
        }
    }
}

