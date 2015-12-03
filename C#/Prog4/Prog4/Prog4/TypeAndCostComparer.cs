using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TypeAndCostComparer : IComparer<Parcel>
{
    // Precondition:  None
    // Postcondition: Returns 0 if p1 type and cost = p2 type and cost
    //                      neg if p1 type then cost(desc) < p2 type then cost(desc)
    //                      pos if p1 type then cost(desc) > p1 type then cost(desc)
    public int Compare(Parcel p1, Parcel p2)
    {
        string type1; // p1's type
        string type2; // p2's type

        // Implements correct handling of null values (in .NET, null less than anything)
        if (this == null && p2 == null) // Both null?
            return 0;

        if (this == null) // only this is null?
            return -1;

        if (p2 == null) // only p2 is null?
            return 1;

        type1 = p1.GetType().ToString();
        type2 = p2.GetType().ToString();

        if (type1 == type2) // Same type
            return p2.CompareTo(p1); // Descending by Cost
                                     // Reverse of natural order

        // else different types

        return type1.CompareTo(type2);
    }
}
