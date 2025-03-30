using SLAPScheduling.Algorithm.Utilities;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;
using SLAPScheduling.Domain.Enum;

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
        public static bool TryGetPropertyValue(this List<WarehouseProperty> properties, string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var property = properties.FirstOrDefault(p => p.propertyName == propertyName);
                if (property != null)
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
        /// Retrieve the value as Meter of Length, Width, or Height.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static double GetSizeParameter(this List<LocationProperty> properties, string propertyName = "Length")
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
        public static bool TryGetPropertyValue(this List<LocationProperty> properties, string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var property = properties.FirstOrDefault(p => p.propertyName == propertyName);
                if (property != null)
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
        /// Retrieve the value as Meter of Length, Width, or Height.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static double GetSizeParameter(this List<MaterialProperty> properties, string propertyName = "Length")
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
        public static bool TryGetPropertyValue(this List<MaterialProperty> properties, string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var property = properties.FirstOrDefault(p => p.propertyName == propertyName);
                if (property != null)
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
