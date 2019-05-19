using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThucPhamSach.Models;
using PagedList.Mvc;
using PagedList;
namespace WebsiteThucPhamSach.Areas.Admin.Controllers
{
    public class QLHoaDonController : Controller
    {
        // GET: Admin/QLHoaDon
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public ActionResult HoaDon(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var HoaDon = db.HOADONs.ToList().OrderBy(n => n.TRANGTHAI==false).ToPagedList(pageNumber, pageSize);
            return View(HoaDon);
        }
        // Huy hóa đơn
        public ActionResult Huy(int MAHD)
        {
            var CTHoaDon = db.CTHOADONs.Where(n => n.MAHD == MAHD).ToList();
            foreach(var item in CTHoaDon)
            {
                db.CTHOADONs.Remove(item);
                var SanPham = db.SANPHAMs.SingleOrDefault(n => n.MASP == item.MASP);
                SanPham.SOLUONG += item.SOLUONG;
                UpdateModel(SanPham);
                db.SaveChanges();
            }
            var indexHoaDon = db.HOADONs.SingleOrDefault(n => n.MAHD == MAHD);
            db.HOADONs.Remove(indexHoaDon);
            db.SaveChanges();
            return Json(new{
                status=true
            });
        }
        // Chi tiết hóa đơn
        public PartialViewResult CTHoaDonPartial(int MAHD)
        {
            var HoaDon = db.HOADONs.First(n => n.MAHD == MAHD);
            ViewBag.MAHD = HoaDon.MAHD;
            ViewBag.TENKH = HoaDon.TENKH;
            var CTHoaDon=db.CTHOADONs.Where(n => n.MAHD == MAHD).ToList();
            return PartialView(CTHoaDon);
        }
        // Xác nhận hóa đơn
        public ActionResult Bat(int MAHD)
        {
            var HoaDon = db.HOADONs.FirstOrDefault(n => n.MAHD == MAHD);
            if (HoaDon.TRANGTHAI == true)
            {
                HoaDon.TRANGTHAI = false;
                UpdateModel(HoaDon);
                db.SaveChanges();
            }
            else
            {
                HoaDon.TRANGTHAI = true;
                UpdateModel(HoaDon);
                db.SaveChanges();
            }
            return Json(new
            {
                status = true
            },JsonRequestBehavior.AllowGet);
        }
    }
}