using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThucPhamSach.Models;

namespace WebsiteThucPhamSach.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string Email = f["EMAIL"].ToString();
            string Passwork = f["MATKHAU"].ToString();
            try
            {
                var Admin = db.ADMINs.SingleOrDefault(n=>n.EMAIL==Email&&n.MATKHAU==Passwork);
                if (Admin != null)
                {
                    Session["ADMIN"] = Admin.MAAD;
                    Session["TENAD"] = Admin.TENAD;
                    return RedirectToAction("TrangChu", "Admin");
                }
            }
            catch (Exception e)
            {

            }
            return View();
        }
        public ActionResult TrangChu()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("DangNhap", "Admin");
            }else
            {
                ViewBag.NameAD = Session["ADMIN"].ToString();
            }
            return View();
        }
        public PartialViewResult HeaderADPartial()
        {
            return PartialView();
        }
        public ActionResult DangXuat()
        {
            Session["ADMIN"] = null;
            Session["TENAD"] = null;
            return RedirectToAction("DangNhap", "Admin");
        }
    }
}