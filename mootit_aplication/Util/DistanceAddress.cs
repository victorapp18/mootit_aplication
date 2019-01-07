using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mootit_aplication.Util
{
    public class DistanceAddress
    {
        public static decimal DistanceBetween(float latA, float longA, float latB, float longB)
        {
            var RadianLatA = Math.PI * latA / 180;
            var RadianLatb = Math.PI * latB / 180;
            var RadianLongA = Math.PI * longA / 180;
            var RadianLongB = Math.PI * longB / 180;

            double theDistance = (Math.Sin(RadianLatA)) *
                    Math.Sin(RadianLatb) +
                    Math.Cos(RadianLatA) *
                    Math.Cos(RadianLatb) *
                    Math.Cos(RadianLongA - RadianLongB);

            return Convert.ToDecimal(((Math.Acos(theDistance) * (180.0 / Math.PI)))) * 69.09M * 1.6093M;
        }

    }
}