using SLAP.AggregateModels.MaterialAggregate;
using SLAP.Enum;
using SLAPScheduling.AggregateModels.Properties;
using SLAPScheduling.Utilities;

namespace SLAP.AggregateModels.StorageAggregate
{
    public class Location
    {
        public string LocationId { get; set; }
        public string WarehouseName { get; set; }
        public List<Property> Properties { get; set; }
        public List<MaterialSubLot> MaterialSubLots { get; set; }

        public Location()
        {
        }

        public Location(string locationId, string warehouseName, List<MaterialSubLot> materialSubLots) : this(locationId, warehouseName)
        {
            MaterialSubLots = materialSubLots;
        }

        public Location(string locationId, string warehouseName)
        {
            LocationId = locationId;
            WarehouseName = warehouseName;
            Properties = new List<Property>();
            MaterialSubLots = new List<MaterialSubLot>();
        }

        #region Calculate Volume Size

        /// <summary>
        /// Calculate the Volume of Material as Cubic Meter.
        /// </summary>
        /// <returns></returns>
        public double GetVolumeSize()
        {
            double length = GetBoxParameter("Length");
            double width = GetBoxParameter("Width");
            double height = GetBoxParameter("Height");

            return length * width * height;
        }

        #endregion

        #region Retrieve Properties

        /// <summary>
        /// Retrieve the value as Meter of Length, Width, or Height.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public double GetBoxParameter(string propertyName = "Length")
        {
            if (GetPropertyValue(propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure) && double.TryParse(propertyValue, out double sizeValue))
            {
                return sizeValue * Utility.GetMeterMultiplier(unitOfMeasure);
            }

            return 0;
        }

        /// <summary>
        /// Retrieve the property value from property name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool GetPropertyValue(string propertyName, out string propertyValue, out UnitOfMeasure unitOfMeasure)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var property = Properties.FirstOrDefault(p => p.PropertyName == propertyName);
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
