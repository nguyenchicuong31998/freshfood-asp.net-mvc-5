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
    public class QLDanhMucController : Controller
    {
        // GET: Admin/QLDanhMuc
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        private static int LuuMADM;
        public ActionResult DanhMuc(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var DanhMuc = db.DANHMUCs.ToList().OrderBy(n => n.MADM).ToPagedList(pageNumber, pageSize);
            return View(DanhMuc);
        }
        public ActionResult ThemDMPartial()
        {
            List<MENU> cate = db.MENUs.ToList();

            // Tạo SelectList
            SelectList cateList = new SelectList(cate, "MAMN", "TENMN");

            // Set vào ViewBag
            ViewBag.Menu = cateList;
            return View();
        }
        // Action thêm Danh Mục
        public ActionResult ThemDanhMuc(FormCollection f, DANHMUC DanhMuc)
        {
            try
            {
                string TENDM = f["TENDM"].ToString();
                int MAMN = int.Parse(f["MAMN"].ToString());
                DanhMuc.TENDM = TENDM;
                DanhMuc.MAMN = MAMN;
                DanhMuc.TRANGTHAI = true;
                db.DANHMUCs.Add(DanhMuc);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("DanhMuc", "QLDanhMuc");
        }
        // Action Xóa Menu
        public ActionResult Xoa(int MADM)
        {
            var IndexDanhMuc = db.DANHMUCs.SingleOrDefault(n => n.MADM == MADM);
            db.DANHMUCs.Remove(IndexDanhMuc);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        // Public Sửa Menu
        public PartialViewResult SuaDMPartial(int MADM)
        {
            var DanhMuc = db.DANHMUCs.SingleOrDefault(n => n.MADM == MADM);
            LuuMADM = MADM;
            List<MENU> cate = db.MENUs.ToList();

            // Tạo SelectList
            SelectList cateList = new SelectList(cate, "MAMN", "TENMN");

            // Set vào ViewBag
            ViewBag.Menu = cateList;
            return PartialView(DanhMuc);
        }
        public ActionResult SuaDanhMuc(FormCollection f)
        {
            try
            {
                string TENDM = f["TENDM"].ToString();
                int MAMN = int.Parse(f["MAMN"].ToString());
                var DanhMuc = db.DANHMUCs.SingleOrDefault(n => n.MADM == LuuMADM);
                DanhMuc.TENDM = TENDM;
                DanhMuc.MAMN = MAMN;
                UpdateModel(DanhMuc);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("DanhMuc", "QLDanhMuc");
        }
        public JsonResult Bat(int MADM)
        {
            var DanhMuc = db.DANHMUCs.FirstOrDefault(n => n.MADM == MADM);
            if (DanhMuc.TRANGTHAI == true)
            {
                DanhMuc.TRANGTHAI = false;
                UpdateModel(DanhMuc);
                db.SaveChanges();
            }
            else
            {
                DanhMuc.TRANGTHAI = true;
                UpdateModel(DanhMuc);
                db.SaveChanges();
            }
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}