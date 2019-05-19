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
    public class QLKhachHangController : Controller
    {
        // GET: Admin/QLKhachHang
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public static int LuuMAKH;
        public ActionResult KhachHang(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var KhachHang = db.KHACHHANGs.ToList().OrderBy(n => n.MAKH).ToPagedList(pageNumber, pageSize);
            return View(KhachHang);
        }
        public ActionResult ThemKHPartial()
        {
            return View();
        }

        // Action thêm khách hàng
        public ActionResult ThemKhachHang(FormCollection f, KHACHHANG KhachHang)
        {
            try
            {
                string HOTEN = f["HOTEN"].ToString();
                string DIENTHOAI = f["DIENTHOAI"].ToString();
                string EMAIL = f["EMAIL"].ToString();
                string MATKHAU = f["MATKHAU"].ToString();
                string GIOITINH = f["GIOITINH"].ToString();
                DateTime NGAYSINH = DateTime.Parse(f["NGAYSINH"].ToString());
                string DIACHI = f["DIACHI"].ToString();
                KhachHang.HOTEN = HOTEN;
                KhachHang.DIENTHOAI = DIENTHOAI;
                KhachHang.EMAIL = EMAIL;
                KhachHang.MATKHAU = MATKHAU;
                KhachHang.GIOITINH = GIOITINH;
                KhachHang.NGAYSINH = NGAYSINH;
                KhachHang.DIACHI = DIACHI;
                KhachHang.TRANGTHAI = true;
                db.KHACHHANGs.Add(KhachHang);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("KhachHang", "QLKhachHang");
        }
        // Action Xóa sản phẩm
        public ActionResult Xoa(int MAKH)
        {
            var IndexKhachHang = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == MAKH);
            db.KHACHHANGs.Remove(IndexKhachHang);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        // Action Sửa Sản Phẩm
        public PartialViewResult SuaKHPartial(int MAKH)
        {
        
            var KhachHang = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == MAKH);
            LuuMAKH = MAKH;
            return PartialView(KhachHang);
        }
        public ActionResult SuaKhachHang(FormCollection f)
        {
            try
            {
                //int MAKH = int.Parse(f["MAKH"].ToString());
                string HOTEN = f["HOTEN"].ToString();
                string DIENTHOAI = f["DIENTHOAI"].ToString();
                string EMAIL = f["EMAIL"].ToString();
                string MATKHAU = f["MATKHAU"].ToString();
                string GIOITINH = f["GIOITINH"].ToString();
                DateTime NGAYSINH = DateTime.Parse(f["NGAYSINH"].ToString());
                string DIACHI = f["DIACHI"].ToString();
                var KhachHang = db.KHACHHANGs.FirstOrDefault(n => n.MAKH == LuuMAKH);
                KhachHang.HOTEN = HOTEN;
                KhachHang.DIENTHOAI = DIENTHOAI;
                KhachHang.EMAIL = EMAIL;
                KhachHang.MATKHAU = MATKHAU;
                KhachHang.GIOITINH = GIOITINH;
                KhachHang.NGAYSINH = NGAYSINH;
                KhachHang.DIACHI = DIACHI;
                UpdateModel(KhachHang);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("KhachHang", "QLKhachHang");
        }
        public JsonResult Bat(int MAKH)
        {
            var KhachHang = db.KHACHHANGs.FirstOrDefault(n => n.MAKH == MAKH);
            if (KhachHang.TRANGTHAI == true)
            {
                KhachHang.TRANGTHAI = false;
                UpdateModel(KhachHang);
                db.SaveChanges();
            }
            else
            {
                KhachHang.TRANGTHAI = true;
                UpdateModel(KhachHang);
                db.SaveChanges();
            }
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
    }
}