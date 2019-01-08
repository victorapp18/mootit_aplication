using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace mootit_aplication.Util
{
    public class DistanceAddress
    {
        public static double DistanceBetween(float sLatitude, float sLongitude, float eLatitude, float eLongitude)
        {
            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate(eLatitude, eLongitude);

            return sCoord.GetDistanceTo(eCoord);
        }

    }
}