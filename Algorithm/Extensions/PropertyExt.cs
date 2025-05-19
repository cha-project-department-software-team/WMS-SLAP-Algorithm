using SLAPScheduling.Algorithm.Utilities;

namespace SLAPScheduling.Algorithm.Extensions
{
    public static class PropertyExt
    {
        #region Retrieve Properties for Warehouse Property

        /// <summary>
        /// Retrieve the value as Meter of Length, Width, or Height.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static double GetSizeParameter(this List<WarehouseProperty> properties, string propertyName = "Length")
        {
            if (properties?.Count > 0 && properties.TryGetPropertyValue(propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure))
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
        public static bool TryGetPropertyValue(this List<WarehouseProperty> properties, string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (properties?.Count > 0 && !string.IsNullOrEmpty(propertyName))
            {
                var property = properties.FirstOrDefault(p => p.propertyName == propertyName);
                if (property is not null)
                {
                    propertyValue = property.propertyValue;
                    unitOfMeasure = property.unitOfMeasure;
                    return true;
                }
            }

            propertyValue = string.Empty;
            unitOfMeasure = UnitOfMeasure.None;
            return false;
        }

        #endregion

        #region Retrieve Properties for Location Property

        /// <summary>
        /// Retrieve the property value from property name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool TryGetPropertyValue(this List<LocationProperty> properties, string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (!string.IsNullOrEmpty(propertyName) && properties?.Count > 0)
            {
                var property = properties.FirstOrDefault(p => p.propertyName == propertyName);
                if (property is not null)
                {
                    propertyValue = property.propertyValue;
                    unitOfMeasure = property.unitOfMeasure;
                    return true;
                }
            }

            propertyValue = string.Empty;
            unitOfMeasure = UnitOfMeasure.None;
            return false;
        }

        #endregion

        #region Retrieve Properties for Material Property

        /// <summary>
        /// Retrieve the property value from property name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool TryGetPropertyValue(this List<MaterialProperty> properties, string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (!string.IsNullOrEmpty(propertyName) && properties?.Count > 0)
            {
                var property = properties.FirstOrDefault(p => p.propertyName == propertyName);
                if (property is not null)
                {
                    propertyValue = property.propertyValue;
                    unitOfMeasure = property.unitOfMeasure;
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
