namespace SLAPScheduling.Application.Mapping
{
    public class ModelToViewModelProfile : Profile
    {
        public ModelToViewModelProfile()
        {
            MapLocationViewModel();
            MapLocationPropertyViewModel();
            MapWarehouseViewModel();
            MapMaterialPropertyViewModel();
            MapMaterialViewModel();
            MapReceiptLotViewModel();
            MapReceiptSublotViewModel();
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

        public void MapMaterialPropertyViewModel()
        {
            CreateMap<MaterialProperty, MaterialPropertyDTO>()
                .ForMember(s => s.UnitOfMeasure, s => s.MapFrom(s => s.unitOfMeasure.ToString()));
        }

        public void MapMaterialViewModel()
        {
            CreateMap<Material, MaterialDTO>()
                .ForMember(s => s.Properties, s => s.MapFrom(s => s.properties));
        }

        public void MapReceiptLotViewModel()
        {
            CreateMap<ReceiptLot, ReceiptLotDTO>()
                .ForMember(s => s.ReceiptSublots, s => s.MapFrom(s => s.receiptSublots))
                .ForMember(s => s.ReceiptLotStatus, s => s.MapFrom(s => s.receiptLotStatus.ToString()));
        }

        public void MapReceiptSublotViewModel()
        {
            CreateMap<ReceiptSublot, ReceiptSubLotDTO>()
                .ForMember(s => s.UnitOfMeasure, s => s.MapFrom(s => s.unitOfMeasure.ToString()))
                .ForMember(s => s.SubLotStatus, s => s.MapFrom(s => s.subLotStatus.ToString()));
        }
    }
}
