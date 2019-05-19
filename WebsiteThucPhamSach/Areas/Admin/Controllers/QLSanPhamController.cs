using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebsiteThucPhamSach.Models;
namespace WebsiteThucPhamSach.Areas.Admin.Controllers
{
    public class QLSanPhamController : Controller
    {
        // GET: Admin/QLSanPham
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public static int LuuMASP;
        public ActionResult SanPham(int? page)
        {
            // tạo biến số sản phẩm trên một trang
            int pageSize = 10;
            // Số trang của sản phẩm
            int pageNumber = (page ?? 1);
            var SanPham = db.SANPHAMs.ToList().OrderBy(n => n.MASP).ToPagedList(pageNumber, pageSize);
            return View(SanPham);
        }
        public ActionResult ThemPartial()
        {
            return View();
        }

        // Action thêm sản phẩm
        public ActionResult ThemSanPham(FormCollection f, SANPHAM SanPham)
        {
            try
            {
                string TenSP = f["TENSP"].ToString();
                string AnhSP = f["file"].ToString();
                string XuatSu = f["XUATSU"].ToString();
                string MoTa = f["MOTA"].ToString();
                int GIASP = int.Parse(f["GIASP"].ToString());
                int GIAMGIA = int.Parse(f["GIAMGIA"].ToString());
                short SPMOI = short.Parse(f["SPMOI"].ToString());
                int MADM = int.Parse(f["MADM"].ToString());
                int LUOTXEM = int.Parse(f["LUOTXEM"].ToString());
                int SOLUONG = int.Parse(f["SOLUONG"].ToString());
                SanPham.TENSP = TenSP;
                SanPham.ANHSP = AnhSP;
                SanPham.XUATSU = XuatSu;
                SanPham.MOTA = MoTa;
                SanPham.GIASP = GIASP;
                SanPham.GIAMGIA = GIAMGIA;
                SanPham.SPMOI = SPMOI;
                SanPham.MADM = MADM;
                SanPham.LUOTXEM = LUOTXEM;
                SanPham.SOLUONG = SOLUONG;
                db.SANPHAMs.Add(SanPham);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("SanPham", "QLSanPham");
        }
        // Action Xóa sản phẩm
        public ActionResult Xoa(int MASP)
        {
            var IndexSanPham = db.SANPHAMs.SingleOrDefault(n => n.MASP == MASP);
            db.SANPHAMs.Remove(IndexSanPham);
            db.SaveChanges();
            return Json(new
            {
                status = true
            },JsonRequestBehavior.AllowGet);
        }

        // Public Sửa Sản Phẩm
        public PartialViewResult SuaPartial(int MASP)
        {
            var SanPham = db.SANPHAMs.SingleOrDefault(n=>n.MASP==MASP);
            LuuMASP = MASP;
            return PartialView(SanPham);
        }
        public ActionResult SuaSanPham(FormCollection f)
        {
            try
            {
                string TenSP = f["TENSP"].ToString();
                string AnhSP = f["file"].ToString();
                string XuatSu = f["XUATSU"].ToString();
                string MoTa = f["MOTA"].ToString();
                int GIASP = int.Parse(f["GIASP"].ToString());
                int GIAMGIA = int.Parse(f["GIAMGIA"].ToString());
                short SPMOI = short.Parse(f["SPMOI"].ToString());
                int MADM = int.Parse(f["MADM"].ToString());
                int LUOTXEM = int.Parse(f["LUOTXEM"].ToString());
                int SOLUONG = int.Parse(f["SOLUONG"].ToString());
                var SanPham = db.SANPHAMs.SingleOrDefault(n => n.MASP == LuuMASP);
                SanPham.TENSP = TenSP;
                SanPham.ANHSP = AnhSP;
                SanPham.XUATSU = XuatSu;
                SanPham.MOTA = MoTa;
                SanPham.GIASP = GIASP;
                SanPham.GIAMGIA = GIAMGIA;
                SanPham.SPMOI = SPMOI;
                SanPham.MADM = MADM;
                SanPham.LUOTXEM = LUOTXEM;
                SanPham.SOLUONG = SOLUONG;
                UpdateModel(SanPham);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("SanPham", "QLSanPham");
        }
    }
}