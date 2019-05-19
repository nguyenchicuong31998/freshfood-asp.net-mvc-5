using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThucPhamSach.Models;
using PagedList;
using PagedList.Mvc;
namespace WebsiteThucPhamSach.Areas.Admin.Controllers
{
    public class QLAdminController : Controller
    {
        // GET: Admin/QLAdmin
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public static int LuuMAAD;
        public ActionResult Admin(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var Admin = db.ADMINs.ToList().OrderBy(n => n.MAAD).ToPagedList(pageNumber, pageSize);
            return View(Admin);
        }
        public ActionResult ThemADPartial()
        {
            return View();
        }
        // Action thêm admin
        public ActionResult ThemAdmin(FormCollection f, ADMIN Admin)
        {
            try
            {
                string TENAD = f["TENAD"].ToString();
                DateTime NGAYSINH = DateTime.Parse(f["NGAYSINH"].ToString());
                string GIOITINH = f["GIOITINH"].ToString();
                string DIACHI = f["DIACHI"].ToString();
                string EMAIL = f["EMAIL"].ToString();
                string MATKHAU = f["MATKHAU"].ToString();
                Admin.TENAD = TENAD;
                Admin.NGAYSINH = NGAYSINH;
                Admin.GIOITINH = GIOITINH;
                Admin.DIACHI = DIACHI;
                Admin.EMAIL = EMAIL;
                Admin.MATKHAU = MATKHAU;
                Admin.TRANGTHAI = true;
                db.ADMINs.Add(Admin);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Admin", "QLAdmin");
        }
        // Action Xóa Admin
        public ActionResult Xoa(int MAAD)
        {
            var IndexTinTuc = db.TINTUCs.SingleOrDefault(n => n.MAAD == MAAD);
            db.TINTUCs.Remove(IndexTinTuc);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        // Action Sửa Sản Phẩm
        public PartialViewResult SuaADPartial(int MAAD)
        {

            var Admin = db.ADMINs.SingleOrDefault(n => n.MAAD == MAAD);
            LuuMAAD = MAAD;
            return PartialView(Admin);
        }
        public ActionResult SuaAdmin(FormCollection f)
        {
            try
            {
                string TENAD = f["TENAD"].ToString();
                DateTime NGAYSINH = DateTime.Parse(f["NGAYSINH"].ToString());
                string GIOITINH = f["GIOITINH"].ToString();
                string DIACHI = f["DIACHI"].ToString();
                string EMAIL = f["EMAIL"].ToString();
                string MATKHAU = f["MATKHAU"].ToString();
                var Admin = db.ADMINs.FirstOrDefault(n => n.MAAD == LuuMAAD);
                Admin.TENAD = TENAD;
                Admin.NGAYSINH = NGAYSINH;
                Admin.GIOITINH = GIOITINH;
                Admin.DIACHI = DIACHI;
                Admin.EMAIL = EMAIL;
                Admin.MATKHAU = MATKHAU;
                UpdateModel(Admin);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Admin", "QLAdmin");
        }
        public JsonResult Bat(int MAAD)
        {
            var admin = db.ADMINs.FirstOrDefault(n => n.MAAD == MAAD);
            if (admin.TRANGTHAI == true)
            {
                admin.TRANGTHAI = false;
                UpdateModel(admin);
                db.SaveChanges();
            }
            else
            {
                admin.TRANGTHAI = true;
                UpdateModel(admin);
                db.SaveChanges();
            }
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}