using SLAP.Enum;

namespace SLAPScheduling.Utilities
{
    public class Utility
    {
        public static double GetMeterMultiplier(UnitOfMeasure unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case UnitOfMeasure.Meter:
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
    }
}
