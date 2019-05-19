using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using WebsiteThucPhamSach.Models;

namespace WebsiteThucPhamSach.Controllers
{
    public class HeaderController : Controller
    {
        // GET: Header
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        [ChildActionOnly]
        public PartialViewResult HeaderPartial()
        {
            var cart = Session["GioHang"];
            var list = new List<GioHang>();
            if (cart != null)
            {
                list = (List<GioHang>)cart;
            }
            return PartialView(list);
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string Email = f["EMAIL"];
            string Passwork = f["MATKHAU"];
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.EMAIL == Email && n.MATKHAU == Passwork && n.TRANGTHAI == true);
            try
            {
                if (kh != null)
                {
                    Session["MAKH"] = kh.MAKH.ToString();
                    Session["HoTen"] = kh.HOTEN.ToString(); ;
                    return Redirect("~/Trang-Chu");
                }
                else
                    {
                        if (Email == "" || Passwork == "")
                        {
                            ModelState.AddModelError("", "Email và mật khẩu không được bỏ trống");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email hoặc mật khẩu sai! Xin vui lòng kiểm tra lại");
                        }
                    }
                
            }
            catch (DbEntityValidationException dbEx)
            {
                ErorrException(dbEx);
            }
             return View(kh);
        }
        public ActionResult DangXuat()
        {
            Session["MAKH"] = null;
            Session["HoTen"] = null;
            return Redirect("~/Trang-Chu");
        }
        public ActionResult DangKy()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KHACHHANG kh)
        {
            try
            {
                    if (kh.GIOITINH.ToString() == "1")
                    {
                        kh.GIOITINH = "Nam";
                    }
                    if(kh.GIOITINH.ToString()=="0")
                    {
                        kh.GIOITINH = "Nữ";
                    }
                    kh.TRANGTHAI = true;
                    db.KHACHHANGs.Add(kh);
                    db.SaveChanges();
                    return RedirectToAction("DangNhap","Header");
            }
            catch (DbEntityValidationException dbEx)
            {     
                ErorrException(dbEx);
            }
            return View();
        }
        public void ErorrException(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }
            }
        }
        public string GetQC(int ThuTuQC)
        {
            var qc = db.QUANGCAOs.SingleOrDefault(n => n.THUTUQC == ThuTuQC && n.TRANGTHAI == true);
            string Anh = "";
            Anh += qc.ANHQC.ToString();
            return Anh;
        }
        public ActionResult ThongTinCaNhan()
        {
            int MAKH = int.Parse(Session["MAKH"].ToString());
            var TTKhachHang = db.KHACHHANGs.SingleOrDefault(n=>n.MAKH==MAKH);
            ViewBag.GIOITINH = TTKhachHang.GIOITINH;
            return View(TTKhachHang);
        }
        [HttpPost]
        public ActionResult ThongTinCaNhan(FormCollection fc)
        {
            try
            {
                int MAKH = int.Parse(Session["MAKH"].ToString());
                var UpdateKH = db.KHACHHANGs.SingleOrDefault(n=>n.MAKH==MAKH);
                UpdateKH.MAKH = MAKH;
                UpdateKH.HOTEN = fc["HOTEN"].ToString();
                UpdateKH.NGAYSINH = DateTime.Parse(fc["NGAYSINH"].ToString());
                string GioiTinh = fc["GIOITINH"].ToString();
                if (GioiTinh == "1")
                {
                    UpdateKH.GIOITINH = "Nam";
                }
                else
                {
                    UpdateKH.GIOITINH = "Nữ";
                }
                UpdateKH.DIENTHOAI = fc["DIENTHOAI"].ToString();
                UpdateKH.DIACHI = fc["DIACHI"].ToString();
                UpdateModel(UpdateKH);
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                ErorrException(dbEx);
            }

            return RedirectToAction("ThongTinCaNhan","Header");
        }
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection f)
        {

            const string EmailAdmin = "chicuong031998@gmail.com";
            string EmailClient = f["EMAIL"].ToString();
            var SetMauKhat = db.KHACHHANGs.SingleOrDefault(n => n.EMAIL == EmailClient);
            if (SetMauKhat != null)
            {
                SetMauKhat.MATKHAU = "123";
                UpdateModel(SetMauKhat);
                db.SaveChanges();
                MailMessage mail = new MailMessage();
                mail.To.Add(EmailClient);
                mail.From = new MailAddress(EmailAdmin);
                mail.Subject = " Đặt lại mật khẩu website FreshFood";
                mail.Body = "Mật khẩu của bạn đã được đặt lại là: <strong>123<strong>";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(EmailAdmin, "01693703781");// tài khoản Gmail của bạn
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return RedirectToAction("DangNhap", "Header");
            }
            return RedirectToAction("DangNhap", "Header");
        }
    }
}