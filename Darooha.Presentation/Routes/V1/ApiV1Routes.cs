
namespace Darooha.Presentation.Routes.V1
{
    public static class ApiV1Routes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Site = "site";

        public const string Panel = "panel";

        public const string App = "app";

        public const string BaseSitePanel = Root + "/" + Version + "/" + Site + "/" + Panel;
        public const string BaseSiteApp = Root + "/" + Version + "/" + Site + "/" + App;

        #region AdminRoutes

        public static class AdminUsers
        {
            //api/v1/site/panel/AdminUsers
            //GET
            public const string GetUsers = BaseSitePanel + "/adminusers";
            //api/v1/site/panel/AdminUsers
            //Post
            public const string EditRoles = BaseSitePanel + "/adminusers/editroles/{userName}";
        }

        #endregion

        #region UsersRoutes

        public static class Users
        {
            //api/v1/site/admin/Users
            //GET
            public const string GetUsers = BaseSitePanel + "/User";
            //api​/v1​/site​/admin​/Users​/{id}
            //GET
            public const string GetUser = BaseSitePanel + "/User/{id}";
            //​api​/v1​/site​/admin​/Users​/{id}
            //PUT
            public const string UpdateUser = BaseSitePanel + "/User/{id}";
            //​api​/v1​/site​/admin​/Users​/ChangeUserPassword​/{id}
            //PUT
            public const string ChangeUserPassword = BaseSitePanel + "/ChangeUserPassword/{id}";
        }

        #endregion

        #region PhotosRoutes
        public static class Photos
        {
            //api/v1/site/admin/users/{userId}/photos/{id}
            //GET
            public const string GetPhoto = BaseSitePanel + "/users/{userId}/photos/{id}";
            //api/v1/site/admin/users/{userId}/photos
            //GET
            public const string ChangeUserPhoto = BaseSitePanel + "/users/{userId}/photos/{id}";
        }
        #endregion

        #region AuthRoutes
        public static class Auth
        {
            //api/v1/site/admin/auth/register
            //GET
            public const string Register = BaseSitePanel + "/Auth/register";
            //api/v1/site/admin/auth/login
            //GET
            public const string Login = BaseSitePanel + "/Auth/login";
        }
        #endregion

        #region NotificationRoutes
        public static class Notification
        {
            //api/v1/site/panel/{userId}/notifications
            //Put
            public const string UpdateUserNotify = BaseSitePanel + "/users/{userId}/notifications";
            //api/v1/site/panel/notifications/{userId}
            //Get
            public const string GetUserNotify = BaseSitePanel + "/notifications/{userId}";
        }
        #endregion

        #region UserAddressRoutes
        public static class UserAddress
        {
            //api/v1/site/panel/users/{userId}/useraddress/{id}
            //GET
            public const string GetUserAddress = BaseSitePanel + "/users/{userId}/useraddress/{id}";
            //api/v1/site/panel/users/{userId}/alluseraddress
            //GET
            public const string GetAllUserAddress = BaseSitePanel + "/users/{userId}/alluseraddress";
            //api/v1/site/panel/useraddress/{id}
            //Put
            public const string UpdateUserAddress = BaseSitePanel + "/useraddress/{id}";
            //api/v1/site/panel/useraddress/{id}
            //Delete
            public const string DeleteUserAddress = BaseSitePanel + "/useraddress/{id}";
            //api/v1/site/panel/users/{userId}/useraddress
            //post
            public const string AddUserAddress = BaseSitePanel + "/users/{userId}/useraddress";
        }
        #endregion

        #region WalletRoutes
        public static class Wallet
        {
            //api/v1/site/panel/users/{userId}/wallet
            //GET
            public const string GetWallet = BaseSitePanel + "/wallet/{userId}";
            //api/v1/site/panel/wallet/{userId}
            //Put
            public const string AddToWallet = BaseSitePanel + "/wallet/{userId}";
        }
        #endregion

        #region TicketRoutes
        public static class Ticket
        {
            //api/v1/site/panel/users/{userId}/tickets
            //POST
            public const string AddTicket = BaseSitePanel + "/users/{userId}/tickets";
            //api/v1/site/panel/users/{userId}/tickets
            //GET
            public const string GetTickets = BaseSitePanel + "/users/{userId}/tickets/page/{page}";
            //api/v1/site/panel/users/{userId}/tickets/{id}
            //GET
            public const string GetTicket = BaseSitePanel + "/users/{userId}/tickets/{id}";
            //---------------------------------------------------------------------------------------------------------
            //api/v1/site/panel/users/{userId}/tickets/{id}/ticketContents
            //POST
            public const string AddTicketContent = BaseSitePanel + "/users/{userId}/tickets/{id}/ticketcontents";
            //api/v1/site/panel/users/{userId}/tickets/{ticketId}/ticketContents/{id}
            //GET
            public const string GetTicketContent = BaseSitePanel + "/users/{userId}/tickets/{ticketId}/ticketContents/{id}";
            //api/v1/site/panel/users/{userId}/tickets/{id}/ticketContents
            //GET
            public const string GetTicketContents = BaseSitePanel + "/users/{userId}/tickets/{id}/ticketcontents";
        }
        #endregion

        #region OrderRoutes
        public static class Order
        {
            //api/v1/site/panel/{userId}/Orders
            //GET
            public const string GetOrders = BaseSitePanel + "/{userId}/Orders";
            //api/v1/site/panel/{userId}/Orders/{id}
            //GET
            public const string GetOrder = BaseSitePanel + "/{userId}/Orders/{id}";
            //---------------------------------------------------------------------------------------------------
            //api/v1/site/panel/{userId}/Orders/{id}/OrderItems
            //GET
            public const string GetOrderItems = BaseSitePanel + "/{userId}/Orders/{id}/OrderItems";
        }
        #endregion

        #region MenuSiteRoutes
        public static class MenuSite
        {
            //api/v1/site/app/Menu
            //GET
            public const string GetMenu = BaseSiteApp + "/Menu";
            //api/v1/site/app/SubMenu/{id}
            //GET
            public const string GetSubMenu = BaseSiteApp + "/SubMenu/{id}";
        }
        #endregion

        #region HomePageRoutes
        public static class Home
        {
            //api/v1/site/app/Home/NewProduct
            //GET
            public const string GetNewProduct = BaseSiteApp + "/Home/NewProduct";
            //api/v1/site/app/Home/Slider
            //GET
            public const string GetSliderItems = BaseSiteApp + "/Home/Slider";
            //api/v1/site/app/Home/SingleProduct
            //GET
            public const string GetSingleProduct = BaseSiteApp + "/Home/SingleProduct";
            //api/v1/site/app/Home/NewMenus
            //GET
            public const string GetNewMenus = BaseSiteApp + "/Home/NewMenus";
        }
        #endregion

        #region ProductRoutes
        public static class Product
        {
            //api/v1/site/app/Home/ProductList/{id}
            //GET
            public const string GetProductList = BaseSiteApp + "/Home/ProductList/{id}";
            //api/v1/site/app/Home/Product/{id}
            //GET
            public const string GetSingleProduct = BaseSiteApp + "/Home/Product/{id}";
            //api/v1/site/app/Home/ProductImages/{id}
            //GET
            public const string GetProductImages = BaseSiteApp + "/Home/ProductImages/{id}";
        }
        #endregion

        #region CommentRoutes
        public static class Comment
        {
            //api/v1/site/app/Home/Comment/{id}
            //GET
            public const string GetCommentList = BaseSiteApp + "/Home/Comment/{id}";
            //api/v1/site/app/Home/{userId}/Comment/AddComment/{id}
            //Post
            public const string AddComment = BaseSiteApp + "/Home/{userId}/Comment/{id}";
            //api/v1/site/app/Home/{userId}/Comment/GetComment/{id}
            //GET
            public const string GetComment = BaseSiteApp + "/Home/{userId}/Comment/GetComment/{id}";
        }
        #endregion

        #region UserAddressRoutes
        public static class AdminProduct
        {
            //api/v1/site/panel/{userId}/Admin/SingleProduct/{id}
            //GET
            public const string GetAdminProduct = BaseSitePanel + "/{userId}/Admin/SingleProduct/{id}";
            //api/v1/site/panel/{userId}/Admin/AllProductList
            //GET
            public const string AllProductList = BaseSitePanel + "/{userId}/Admin/AllProductList";
            //api/v1/site/panel/{userId}/Admin/UpdateProduct/{id}
            //Put
            public const string UpdateProduct = BaseSitePanel + "/{userId}/Admin/UpdateProduct/{id}";
            //api/v1/site/panel/{userId}/Admin/DeleteProduct/{id}
            //Delete
            public const string DeleteProduct = BaseSitePanel + "/{userId}/Admin/DeleteProduct/{id}";
            //api/v1/site/panel/{userId}/Admin/AddProduct
            //post
            public const string AddProduct = BaseSitePanel + "/{userId}/Admin/AddProduct";
        }
        #endregion
    }
}
