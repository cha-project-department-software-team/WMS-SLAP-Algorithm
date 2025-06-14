﻿using Newtonsoft.Json;
using SLAPScheduling.Domain.Enum;

namespace SLAPScheduling.Algorithm.Utilities
{
    public class Utility
    {
        public static double GetMeterMultiplier(UnitOfMeasure unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case UnitOfMeasure.Meter:
                    return 1;
                case UnitOfMeasure.MET:
                    return 1;
                case UnitOfMeasure.Centimeter:
                    return 0.01;
                case UnitOfMeasure.Millimeter:
                    return 0.001;
                case UnitOfMeasure.Inch:
                    return 0.0254;
                default:
                    return 0;
            }
        }

        public static void WriteJson(object obj, string jsonPath)
        {
            using (TextWriter writer = File.CreateText(jsonPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);
            }
        }
    }
}
