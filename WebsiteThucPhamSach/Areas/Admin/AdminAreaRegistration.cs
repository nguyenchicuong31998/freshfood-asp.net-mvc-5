using System.Web.Mvc;

namespace WebsiteThucPhamSach.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // Đường dẫn trang khách hàng
            context.MapRoute(
                "Admin_KhachHang",
                "Admin/{controller}/KhachHang/{id}",
                new { action = "KhachHang", controller = "QLKhachHang", id = UrlParameter.Optional },
                namespaces: new string[] { "WebsiteThucPhamSach.Areas.Admin.Controllers" }
            );
            //đường dẫn trang sản phẩm
            context.MapRoute(
                    "Admin_SanPham",
                    "Admin/{controller}/SanPham/{id}",
                    new { action = "SanPham", controller = "QLSanPham", id = UrlParameter.Optional },
                    namespaces: new string[] { "WebsiteThucPhamSach.Areas.Admin.Controllers" }
              );
            // đường dẫn trang chủ
            context.MapRoute(
                    "Admin_default",
                    "Admin/{controller}/{action}/{id}",
                    new { action = "TrangChu", controller = "Admin", id = UrlParameter.Optional },
                    namespaces: new string[] { "WebsiteThucPhamSach.Areas.Admin.Controllers" }
              );
            //context.MapRoute(
            //    "Admin_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new { action = "TrangChu", Controller = "Admin", id = UrlParameter.Optional },
            //    namespaces: new string[] { "WebsiteThucPhamSach.Areas.Admin.Controllers" }
            //);
        }
    }
}