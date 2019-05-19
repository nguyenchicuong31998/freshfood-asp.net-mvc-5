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
    public class QLMenuController : Controller
    {
        // GET: Admin/QLMenu
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        private static int LuuMAMN;
        public ActionResult Menu(int? page)
        {
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var Menu = db.MENUs.ToList().OrderBy(n => n.MAMN).ToPagedList(pageNumber,pageSize);
            return View(Menu);
        }
        public ActionResult ThemMenuPartial()
        {
            return View();
        }
        // Action thêm Menu
        public ActionResult ThemMenu(FormCollection f, MENU Menu)
        {
            try
            {
                string TENMN = f["TENMN"].ToString();
                string HREF = f["HREF"].ToString();
                Menu.TENMN = TENMN;
                Menu.HREF = HREF;
                Menu.TRANGTHAI = true;
                db.MENUs.Add(Menu);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Menu", "QLMenu");
        }
        // Action Xóa Menu
        public ActionResult Xoa(int MAMN)
        {
            var IndexMenu = db.MENUs.SingleOrDefault(n => n.MAMN == MAMN);
            db.MENUs.Remove(IndexMenu);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        // Public Sửa Menu
        public PartialViewResult SuaMenuPartial(int MAMN)
        {
            var Menu = db.MENUs.SingleOrDefault(n => n.MAMN == MAMN);
            LuuMAMN = MAMN;
            return PartialView(Menu);
        }
        public ActionResult SuaMenu(FormCollection f)
        {
            try
            {
                string TENMN = f["TENMN"].ToString();
                string HREF = f["HREF"].ToString();
                var Menu = db.MENUs.SingleOrDefault(n => n.MAMN == LuuMAMN);
                Menu.TENMN = TENMN;
                Menu.HREF = HREF;
                UpdateModel(Menu);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Menu", "QLMenu");
        }
        public JsonResult Bat(int MAMN)
        {
            var menu = db.MENUs.FirstOrDefault(n => n.MAMN == MAMN);
            if (menu.TRANGTHAI == true)
            {
                menu.TRANGTHAI = false;
                UpdateModel(menu);
                db.SaveChanges();
            }
            else
            {
                menu.TRANGTHAI = true;
                UpdateModel(menu);
                db.SaveChanges();
            }
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}