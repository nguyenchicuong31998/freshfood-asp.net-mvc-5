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
    public class QLTinTucController : Controller
    {
        // GET: Admin/QLTinTuc
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public static int LuuMATT;
        public ActionResult TinTuc(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var TinTuc = db.TINTUCs.OrderBy(n => n.MATT).ToPagedList(pageNumber, pageSize);
            return View(TinTuc);
        }
        public ActionResult ThemTTPartial()
        {
            return View();
        }
        // Action thêm tin tức
        public ActionResult ThemTinTuc(FormCollection f, TINTUC TinTuc)
        {
            try
            {
                int MAAD = int.Parse(f["MAAD"].ToString());
                string TIEUDE = f["TIEUDE"].ToString();
                string ANH = f["ANH"].ToString();
                //DateTime NGAYDANG = DateTime.Parse(f["NGAYDANG"].ToString());
                string MOTA = f["MOTA"].ToString();
                string NOIDUNG = f["NOIDUNG"].ToString();
                int NOIBAT = int.Parse(f["NOIBAT"].ToString());
                int LUOTXEM = int.Parse(f["LUOTXEM"].ToString());
                TinTuc.MAAD = MAAD;
                TinTuc.TIEUDE = TIEUDE;
                TinTuc.ANH = ANH;
                TinTuc.NGAYDANG = DateTime.Parse(DateTime.Now.Date.ToString("dd/MM/yyyy"));
                TinTuc.MOTA = MOTA;
                TinTuc.NOIDUNG = NOIDUNG;
                TinTuc.NOIBAT = NOIBAT;
                TinTuc.LUOTXEM = LUOTXEM;
                db.TINTUCs.Add(TinTuc);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("TinTuc", "QLTinTuc");
        }
        // Action Xóa tin tức
        public ActionResult Xoa(int MATT)
        {
            var IndexTinTuc = db.TINTUCs.SingleOrDefault(n => n.MATT == MATT);
            db.TINTUCs.Remove(IndexTinTuc);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        // Action Sửa tin tức
        public PartialViewResult SuaTTPartial(int MATT)
        {

            var TinTuc = db.TINTUCs.SingleOrDefault(n => n.MATT == MATT);
            LuuMATT = MATT;
            return PartialView(TinTuc);
        }
        [ValidateInput(false)]
        public ActionResult SuaTinTuc(FormCollection f)
        {
            try
            {
                int MAAD = int.Parse(f["MAAD"].ToString());
                string TIEUDE = f["TIEUDE"].ToString();
                string ANH = f["ANH"].ToString();
                DateTime NGAYDANG = DateTime.Parse(f["NGAYDANG"].ToString());
                string MOTA = f["MOTA"].ToString();
                string NOIDUNG = f["NOIDUNG"].ToString();
                int NOIBAT = int.Parse(f["NOIBAT"].ToString());
                int LUOTXEM = int.Parse(f["LUOTXEM"].ToString());
                var TinTuc = db.TINTUCs.FirstOrDefault(n => n.MATT == LuuMATT);
                TinTuc.MAAD = MAAD;
                TinTuc.TIEUDE = TIEUDE;
                TinTuc.ANH = ANH;
                TinTuc.NGAYDANG = NGAYDANG;
                TinTuc.MOTA = MOTA;
                TinTuc.NOIDUNG = NOIDUNG;
                TinTuc.NOIBAT = NOIBAT;
                TinTuc.LUOTXEM = LUOTXEM;
                UpdateModel(TinTuc);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("TinTuc", "QLTinTuc");
        }
    }
}