using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Parcel : IComparable<Parcel>
{
    // Precondition:  None
    // Postcondition: The parcel is created with the specified values for
    //                origin address and destination address
    public Parcel(Address originAddress, Address destAddress)
    {
        OriginAddress = originAddress;
        DestinationAddress = destAddress;
    }

    public Address OriginAddress
    {
        // Precondition:  None
        // Postcondition: The parcel's origin address has been returned
        get;

        // Precondition:  None
        // Postcondition: The parcel's origin address has been set to the
        //                specified value
        set;
    }

    public Address DestinationAddress
    {
        // Precondition:  None
        // Postcondition: The parcel's destination address has been returned
        get;

        // Precondition:  None
        // Postcondition: The parcel's destination address has been set to the
        //                specified value
        set;
    }

    // Precondition:  None
    // Postcondition: The parcel's cost has been returned
    public abstract decimal CalcCost();

    // Precondition:  None
    // Postcondition: A String with the parcel's data has been returned
    public override String ToString()
    {
        return String.Format("Origin Address:{3}{0}{3}{3}Destination Address:{3}{1}{3}Cost: {2:C}",
            OriginAddress, DestinationAddress, CalcCost(), Environment.NewLine);
    }

    // Precondition:  None
    // Postcondition: Returns 0 if this.CalcCost() = p2.CalcCost()
    //                      neg if this.CalcCost() < p2.CalcCost()
    //                      pos if this.CalcCost() > p2.CalcCost()
    public int CompareTo(Parcel p2)
    {
        // Implements correct handling of null values (in .NET, null less than anything)
        if (this == null && p2 == null) // Both null?
            return 0;

        if (this == null) // only this is null?
            return -1;

        if (p2 == null) // only p2 is null?
            return 1;

        decimal cost1; // Cost of this parcel
        decimal cost2; // Cost of p2

        cost1 = this.CalcCost();
        cost2 = p2.CalcCost();

        // if/else logic determines which of 3 possibilities
        if (cost1 == cost2)
            return 0;
        else if (cost1 < cost2)
            return -1;
        else
            return 1;

        // -- OR --
        //return decimal.Compare(cost1, cost2);
    }
}
