using AutoMapper;
using SLAPScheduling.Application.DTOs.LocationDTOs;
using SLAPScheduling.Application.DTOs.WarehouseDTOs;

namespace SLAPScheduling.Application.Mapping
{
    public class ModelToViewModelProfile : Profile
    {
        public ModelToViewModelProfile()
        {
            MapLocationViewModel();
            MapLocationPropertyViewModel();
            MapWarehouseViewModel();

        }
        public void MapLocationViewModel()
        {
            CreateMap<Location, LocationDTO>()
                .ForMember(s => s.LocationPropertyDTOs, s => s.MapFrom(s => s.properties));
        }

        public void MapLocationPropertyViewModel()
        {
            CreateMap<LocationProperty, LocationPropertyDTO>()
                .ForMember(s => s.UnitOfMeasure, s => s.MapFrom(s => s.unitOfMeasure.ToString()));
        }

        public void MapWarehouseViewModel()
        {
            CreateMap<Warehouse, WarehouseDTO>()
                .ForMember(s => s.Locations, s => s.MapFrom(s => s.locations));
        }



    }
}
