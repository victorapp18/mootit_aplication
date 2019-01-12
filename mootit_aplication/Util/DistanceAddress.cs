using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace mootit_aplication.Util
{
    public class DistanceAddress
    {
        public static double DistanceBetween(double latitudeA, double latituteB, double longitudeA, double longitudeB)
        {
            double latA = latitudeA * 0.017453292519943295;
            double longA = longitudeA * 0.017453292519943295;
            double longAB = longA - longitudeB;
            double latAB = latA - longitudeB;
            double num8 = Math.Pow(Math.Sin(latAB / 2.0), 2.0) + ((Math.Cos(latitudeA) * Math.Cos(latA)) * Math.Pow(Math.Sin(longAB / 2.0), 2.0));
            double num9 = 2.0 * Math.Atan2(Math.Sqrt(num8), Math.Sqrt(1.0 - num8));

            return (6376500.0 * num9);
        }

    }
}