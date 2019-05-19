using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThucPhamSach.Models;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using PagedList;

namespace WebsiteThucPhamSach.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public static int masp = 1;
        public ActionResult TrangChu()
        {
            return View();
        }
        public PartialViewResult ContentPartial(int MADM = 1)
        {
            var SanPham = db.SANPHAMs.Where(m => m.MADM == MADM).ToList();
            if (SanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(SanPham);
        }

        public ActionResult SanPham(int MADM = 0,int? LocGia = 0)
        {
         
            var SanPham = db.SANPHAMs.Where(m => m.MADM == MADM).ToList();         
            DANHMUC  dm= db.DANHMUCs.SingleOrDefault(m=>m.MADM==MADM);
            //if(LocGia==1)
            //{
            //     SanPham = db.SANPHAMs.Where(m => m.GIASP < 100000).ToList();
            //}
            var DanhMuc = db.DANHMUCs.ToList();
            if (SanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.TENDM = dm.TENDM.ToString();
            ViewBag.DanhMuc = DanhMuc;
            return View(SanPham);
        }
        public ActionResult ChiTietSanPham(int MASP = 0)
        {
            var SanPham = db.SANPHAMs.SingleOrDefault(m=>m.MASP==MASP);
            if (SanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham.LUOTXEM = SanPham.LUOTXEM + 1;
            UpdateModel(SanPham);
            db.SaveChanges();
            masp = SanPham.MASP;
            return View(SanPham);
        }
        public string TrangThai(int sl)
        {
            string trangthai = "Còn hàng";
            if (sl <= 0)
            {
                trangthai = "Hết hàng";
            }
            return trangthai;
        }
        public PartialViewResult DanhMucPartial()
        {
            var DanhMuc = db.DANHMUCs.Where(n=>n.TRANGTHAI==true).ToList();
            return PartialView(DanhMuc);
        }
        public PartialViewResult NoiDungPartial()
        {
            var SanPham = db.SANPHAMs.ToList();
            if (SanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.GiamGia = db.SANPHAMs.Take(12).ToList();
            return PartialView(SanPham);
        }
        public ActionResult TimKiem(string txttimkiem)
        {
            var SanPham = db.SANPHAMs.SqlQuery("select * from SANPHAM Where TENSP like '%" + txttimkiem + "%'").ToList();
            return View(SanPham);
        }
        public string FormatPrice(string _strInput)
        {
            string strInput = _strInput;
            int Length = 0;
            if (strInput.IndexOf('.') > 0)
                Length = strInput.Length - (strInput.Length - strInput.IndexOf('.'));
            else
                Length = strInput.Length;
            string afterFormat = "";
            if (Length <= 3)
                afterFormat = strInput;
            else if (Length > 3)
            {
                afterFormat = strInput.Insert(Length - 3, ".");
                Length = afterFormat.IndexOf(".");
                while (Length > 3)
                {
                    afterFormat = afterFormat.Insert(Length - 3, ".");
                    Length = Length - 3;
                }
            }
            return afterFormat;
        }
        // Giỏ hàng khởi tạo
        public ActionResult GioHang()
        {
            var cart = Session["GioHang"];
            var list = new List<GioHang>();
            if (cart != null)
            {
                list = (List<GioHang>)cart;
            }
            return View(list);
        }
        //thêm vào giỏ hàng 
        public ActionResult AddItem(int soluong = 1)
        {
            int MASP = masp;
            if (Session["GioHang"] == null) // Nếu giỏ hàng chưa được khởi tạo
            {
                Session["GioHang"] = new List<GioHang>();  // Khởi tạo Session["giohang"] là 1 List<GioHang>
            }
            List<GioHang> giohang = Session["GioHang"] as List<GioHang>;  // Gán qua biến giohang dễ code

            // Kiểm tra xem sản phẩm khách đang chọn đã có trong giỏ hàng chưa
            if (giohang.FirstOrDefault(m => m.MASP == MASP) == null) // ko co sp nay trong gio hang
            {
                SANPHAM sp = db.SANPHAMs.Find(MASP);
                // tim sp theo MaHoa
                GioHang sanpham = new GioHang()
                {
                    MASP = MASP,
                    TENSP = sp.TENSP,
                    ANHSP = sp.ANHSP,               
                    SoLuong = soluong,
                    GIASP = Decimal.Parse(sp.GIASP.ToString())

                };  // Tạo ra 1 sản phẩm mới

                giohang.Add(sanpham);  // Thêm sản phẩm vào giỏ 
            }
            else
            {
                // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                GioHang sanpham = giohang.FirstOrDefault(m => m.MASP == MASP);
                sanpham.SoLuong++;
            }
            return RedirectToAction("GioHang");
        }


        public JsonResult AddGioHang(int MASP)
        {
            //int MASP = masp;
            if (Session["GioHang"] == null) // Nếu giỏ hàng chưa được khởi tạo
            {
                Session["GioHang"] = new List<GioHang>();  // Khởi tạo Session["giohang"] là 1 List<GioHang>
            }
            List<GioHang> giohang = Session["GioHang"] as List<GioHang>;  // Gán qua biến giohang dễ code

            // Kiểm tra xem sản phẩm khách đang chọn đã có trong giỏ hàng chưa
            if (giohang.FirstOrDefault(m => m.MASP == MASP) == null) // ko co sp nay trong gio hang
            {
                SANPHAM sp = db.SANPHAMs.Find(MASP);
                // tim sp theo MaHoa
                GioHang sanpham = new GioHang()
                {
                    MASP = MASP,
                    TENSP = sp.TENSP,
                    ANHSP = sp.ANHSP,
                    SoLuong = 1,
                    GIASP = Decimal.Parse(sp.GIASP.ToString())

                };  // Tạo ra 1 sản phẩm mới

                giohang.Add(sanpham);  // Thêm sản phẩm vào giỏ 
            }
            else
            {
                // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                GioHang sanpham = giohang.FirstOrDefault(m => m.MASP == MASP);
                sanpham.SoLuong++;
            }
            return Json(new
            {
                status = true
            });
        }
        //Xóa giỏ hàng
        public JsonResult Delete(int MASP)
        {
            var GioHang = (List<GioHang>)Session["GioHang"];
            GioHang.RemoveAll(k => k.MASP == MASP);
            return Json(new
            {
                status = true
            });
        }
        //Cập nhật số lượng giỏ hàng
        public JsonResult Update(string CartModel)
        {
            var GioHang = new JavaScriptSerializer().Deserialize<List<GioHang>>(CartModel);
            var session = (List<GioHang>)Session["GioHang"];
            foreach (var item in session)
            {
                var jsonItem = GioHang.SingleOrDefault(n => n.MASP == item.MASP);
                if (jsonItem != null)
                {
                    item.SoLuong = jsonItem.SoLuong;
                }
            }
            return Json(new
            {
                status = true
            });
        }

        //Trang giới thiệu
        public ActionResult GioiThieu()
        {
            return View();
        }

        //Trang Tin Tức 
        public ActionResult TinTuc(int ?page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var ListTinTuc = db.TINTUCs.ToList().OrderBy(n => n.NGAYDANG).ToPagedList(pageNumber, pageSize);
            var DanhMuc = db.DANHMUCs.Where(n=>n.TRANGTHAI==true).ToList();
            ViewBag.DanhMuc = DanhMuc;
            return View(ListTinTuc);
        }

        // Tin Tức Partial
        public PartialViewResult TinTucPartial()
        {
            var TinTuc = db.TINTUCs.Where(m => m.NOIBAT == 1).ToList();
            return PartialView(TinTuc);
        }
        public PartialViewResult ChiTietTinTucPartial(int MATT=1)
        {
            var CTTinTuc = db.TINTUCs.SingleOrDefault(n=>n.MATT==MATT);
            return PartialView(CTTinTuc);
        }
        // Trang Liên Hệ
        public ActionResult LienHe()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ThanhToan()
        {
            var cart = Session["GioHang"];
            var list = new List<GioHang>();
            if (cart != null)
            {
                list = (List<GioHang>)cart;
            }
            try
            {
                int MaKH = int.Parse(Session["MAKH"].ToString());
                var KH = db.KHACHHANGs.SingleOrDefault(n => n.MAKH == MaKH);
                ViewBag.Name = KH.HOTEN;
                ViewBag.SDT = KH.DIENTHOAI;
                ViewBag.Email = KH.EMAIL;
                ViewBag.DiaChi = KH.DIACHI;
            }
            catch (Exception ex)
            {

            }
            return View(list);
        }
        [HttpPost]
        public ActionResult ThanhToan(string name, string email, string sdt, string diachi, DropDownList drop)
        {
            var donhang = new HOADON();
            donhang.MAKH = int.Parse(Session["MAKH"].ToString());
            donhang.TENKH = name;
            donhang.DIENTHOAI = sdt;
            donhang.DIACHI = diachi;
            //  donhang.HTTHANHTOAN = drop.SelectedItem.ToString();
            donhang.HTGIAOHANG = drop.SelectedValue;
            donhang.DONGIA = 1000;
            try
            {
                var MaKH = them(donhang);
                var GioHang = (List<GioHang>)Session["GioHang"];
                foreach (var item in GioHang)
                {
                    var CTHOADON = new CTHOADON();
                    CTHOADON.MAHD = donhang.MAHD;
                    CTHOADON.MASP = item.MASP;
                    CTHOADON.SOLUONG = item.SoLuong;
                    CTHOADON.DONGIA = item.GIASP;
                    CTHOADON.THANHTIEN = item.ThanhTien;
                    db.CTHOADONs.Add(CTHOADON);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Redirect("~/Home/TrangChu");
        }
        public long them(HOADON hoadon)
        {
            db.HOADONs.Add(hoadon);
            db.SaveChanges();
            return int.Parse(hoadon.MAKH.ToString());
        }
    }
}