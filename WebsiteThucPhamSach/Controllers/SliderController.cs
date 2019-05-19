using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThucPhamSach.Models;
namespace WebsiteThucPhamSach.Controllers
{
    public class SliderController : Controller
    {
        // GET: Slider
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public PartialViewResult SliderPartial()
        {
            return PartialView();
        }
        public string GetQC(int ThuTuQC)
        {
            var qc = db.QUANGCAOs.SingleOrDefault(n=>n.THUTUQC==ThuTuQC && n.TRANGTHAI==true);
            string Anh="";
            Anh += qc.ANHQC.ToString();
            return Anh;
        }
    }
}