using SLAP.Enum;
using SLAPScheduling.AggregateModels.Properties;
using SLAPScheduling.Utilities;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class Material
    {
        public string MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialClassId { get; set; }
        public List<Property> Properties { get; set; }

        public Material()
        {
        }

        public Material(string materialId, string materialName, string materialClassId)
        {
            MaterialId = materialId;
            MaterialName = materialName;
            MaterialClassId = materialClassId;
            Properties = new List<Property>();
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
