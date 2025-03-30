using SLAP.Enum;
using SLAPScheduling.AggregateModels.Properties;
using SLAPScheduling.Utilities;

namespace SLAPScheduling.Extensions
{
    public static class PropertyExt
    {
        #region Retrieve Properties

        /// <summary>
        /// Retrieve the value as Meter of Length, Width, or Height.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static double GetSizeParameter(this List<Property> properties, string propertyName = "Length")
        {
            if (properties.TryGetPropertyValue(propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure))
            {
                return double.TryParse(propertyValue, out double sizeValue) ? sizeValue * Utility.GetMeterMultiplier(unitOfMeasure) : 0;
            }

            return 0;
        }

        /// <summary>
        /// Retrieve the property value from property name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool TryGetPropertyValue(this List<Property> properties, string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var property = properties.FirstOrDefault(p => p.PropertyName == propertyName);
                if (property != null)
                {
                    propertyValue = property.PropertyValue;
                    unitOfMeasure = property.UnitOfMeasure;
                    return true;
                }
            }

            propertyValue = string.Empty;
            unitOfMeasure = UnitOfMeasure.None;
            return false;
        }

        #endregion
    }
}
