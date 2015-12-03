using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DescendingByDestZipComparer : IComparer<Parcel>
{
    // Precondition:  None
    // Postcondition: Returns 0 if p1.DestinationAddress.Zip = p2.Destination.Address.Zip
    //                      neg if p1.Destination.Address.Zip < p2.Destination.Address.Zip
    //                      pos if p1.Destination.Address.Zip > p2.Destination.Address.Zip
    public int Compare(Parcel p1, Parcel p2)
    {
        // Implements correct handling of null values (in .NET, null less than anything)
        if (this == null && p2 == null) // Both null?
            return 0;

        if (this == null) // only this is null?
            return -1;

        if (p2 == null) // only p2 is null?
            return 1;

        int zipDifference; // Numeric difference between destination zips

        zipDifference = p2.DestinationAddress.Zip - p1.DestinationAddress.Zip;

        return zipDifference; // Works fine, since ints

        // -- OR --
        //if (p1.DestinationAddress.Zip == p2.DestinationAddress.Zip)
        //    return 0;
        //else if (p1.DestinationAddress.Zip < p2.DestinationAddress.Zip)
        //    return 1;
        //else
        //    return -1;
    }
}
