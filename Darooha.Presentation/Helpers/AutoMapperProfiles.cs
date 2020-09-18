using AutoMapper;
using Darooha.Data.Dtos.Site.Panel.User;
using Darooha.Data.Dtos.Site.Panel.Photo;
using Darooha.Data.Models;
using Darooha.Common.Helpers.Helpers;
using Darooha.Data.Dtos.Site.Panel.Notification;
using Darooha.Data.Dtos.Site.Panel.UserAddress;
using Darooha.Data.Dtos.Site.Panel.Wallet;
using Darooha.Data.Dtos.Site.Panel.Ticket;
using Darooha.Data.Dtos.Site.Panel.Order;
using Darooha.Data.Dtos.Site.App.Menu;
using Darooha.Data.Dtos.Site.App.Product;
using Darooha.Data.Dtos.Site.App.Slider;
using Darooha.Data.Dtos.Site.App.Comment;
using Darooha.Data.Dtos.Site.Panel.Product;

namespace Darooha.Presentation.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //User
            CreateMap<Tbl_User, UserForListDTO>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.BirthDate.ToAge());
                });
            CreateMap<Tbl_User, UserForDetailedDTO>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.BirthDate.ToAge());
                });
            CreateMap<UserForUpdateDTO, Tbl_User>();
            CreateMap<PasswordForChangeDTO, Tbl_User>();
            CreateMap<PhotoForUserProfileDTO, Tbl_User>();
            CreateMap<Tbl_User, PhotoForReturnProfileDTO>();
            //Notification
            CreateMap<NotificationForUpdateDTO, Notification>();
            //User Address
            CreateMap<Tbl_UserAddress, UserAddressForReturnDTO>();
            CreateMap<UserAddressForUpdateDTO, Tbl_UserAddress>();
            CreateMap<UserAddressForCreateDTO, Tbl_UserAddress>();
            CreateMap<UserAddressForDetaileDTO, Tbl_UserAddress>();
            CreateMap<Tbl_UserAddress, UserAddressForDetaileDTO>();
            //Wallet
            CreateMap<Tbl_Wallet, WalletForReturnDTO>();
            //ticket
            CreateMap<TicketForCreateDTO, Ticket>();
            //order
            CreateMap<Tbl_Order, OrderToReturnDto>()
                .ForMember(dest => dest.DateModified, opt =>
                {
                    opt.MapFrom(src => src.DateModified.ToShortDateString());
                })
                .ForMember(dest => dest.AddressID, opt =>
                {
                    opt.MapFrom(src => src.OrderAddress);
                });
            CreateMap<Tbl_OrderItem, OrderItemToReturnDto>();
            //Menu
            CreateMap<Tbl_Menu, MenuForReturnDto>();
            CreateMap<Tbl_Menu, NewMenuForReturnDto>();
            CreateMap<Tbl_SubMenu, SubMenuForReturnDto>();
            // Product
            CreateMap<Tbl_Product, ProductForReturnDto>();
            CreateMap<Tbl_ProductImage, ProductImagesForReturnDto>();
            CreateMap<Tbl_Product, SpecialProductForReturnDto>();
            //Image
            CreateMap<Tbl_HomeFirstSlider, SliderItemToReturnDto>();
            //Image
            CreateMap<Tbl_Comment, CommentForReturnDto>()
                .ForMember(dest => dest.DateModified, opt =>
                {
                    opt.MapFrom(src => src.DateModified.ToShortDateString());
                });
            CreateMap<CommentForCreateDto, Tbl_Comment>();
            // Product
            CreateMap<Tbl_Product, AdminProductForReturnDto>();
            CreateMap<Tbl_Product, AdminProductListForReturnDto>();
            CreateMap<AdminProductForCreateDto, Tbl_Product>();
            CreateMap<AdminProductForUpdateDto, Tbl_Product>();
        }
    }
}
