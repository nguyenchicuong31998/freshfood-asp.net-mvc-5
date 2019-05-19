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
    public class QLQuangCaoController : Controller
    {
        // GET: Admin/QLQuangCao
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        private static int LuuMAQC;
        public ActionResult QuangCao(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var QuangCao = db.QUANGCAOs.ToList().OrderBy(n => n.MAQC).ToPagedList(pageNumber, pageSize);
            return View(QuangCao);
        }
        public ActionResult ThemQCPartial()
        {
            return View();
        }
        // Action thêm Quảng cáo
        public ActionResult ThemQuangCao(FormCollection f, QUANGCAO QuangCao)
        {
            try
            {
                string TENQC = f["TENQC"].ToString();
                string ANHQC = f["file"].ToString();
                string HREF = f["HREF"].ToString();
                short THUTUQC = short.Parse(f["THUTUQC"].ToString());
                QuangCao.TENQC = TENQC;
                QuangCao.ANHQC = "~/Image/"+ANHQC;
                QuangCao.HREF = HREF;
                QuangCao.THUTUQC = THUTUQC;
                QuangCao.TRANGTHAI = true;
                db.QUANGCAOs.Add(QuangCao);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("QuangCao", "QLQuangCao");
        }
        // Action Xóa Quảng cáo
        public ActionResult Xoa(int MAQC)
        {
            var IndexQuangCao = db.QUANGCAOs.SingleOrDefault(n => n.MAQC == MAQC);
            db.QUANGCAOs.Remove(IndexQuangCao);
            db.SaveChanges();
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        // Action Sửa Quảng Cáo
        public PartialViewResult SuaQCPartial(int MAQC)
        {
            var QuangCao = db.QUANGCAOs.SingleOrDefault(n => n.MAQC == MAQC);
            LuuMAQC = MAQC;
            return PartialView(QuangCao);
        }
        public JsonResult Bat(int MAQC)
        {
            var QuangCao = db.QUANGCAOs.FirstOrDefault(n => n.MAQC == MAQC);
            if (QuangCao.TRANGTHAI == true)
            {
                QuangCao.TRANGTHAI = false;
                UpdateModel(QuangCao);
                db.SaveChanges();
            }
            else
            {
                QuangCao.TRANGTHAI = true;
                UpdateModel(QuangCao);
                db.SaveChanges();
            }
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuaQuangCao(FormCollection f)
        {
            try
            {
                string TENQC = f["TENQC"].ToString();
                string ANHQC = f["file"].ToString();
                string HREF = f["HREF"].ToString();
                short THUTUQC = short.Parse(f["THUTUQC"].ToString());
                var QuangCao = db.QUANGCAOs.SingleOrDefault(n => n.MAQC == LuuMAQC);
                QuangCao.TENQC = TENQC;
                QuangCao.ANHQC = "~/Image/" + ANHQC;
                QuangCao.HREF = HREF;
                QuangCao.THUTUQC = THUTUQC;
                UpdateModel(QuangCao);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("QuangCao", "QLQuangCao");
        }
    }
}