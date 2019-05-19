using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThucPhamSach.Models;

namespace WebsiteThucPhamSach.Areas.Admin.Controllers
{
    public class QLThongKeController : Controller
    {
        // GET: Admin/QLThongKe
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();
        public ActionResult ThongKe()
        {
            List<int> ItemMonth = new List<int>();
            for(int i=1;i<13;i++)
            {
                ItemMonth.Add(i);
            }
            List<int> ItemYear = new List<int>();
            for (int i = 1998; i <= 2030; i++)
            {
                ItemYear.Add(i);
            }
            SelectList itemMonth = new SelectList(ItemMonth);
            SelectList itemYear = new SelectList(ItemYear);
            // Set vào ViewBag
            ViewBag.ItemMonth = itemMonth;
            ViewBag.ItemYear = itemYear;
            return View();
        }
        public PartialViewResult DSThongKePartial(int IdMonth=5, int IdYear =2019)
        {
           // SELECT* FROM KHACHHANG WHERE MONTH(NGAYSINH) = 2 AND YEAR(NGAYSINH)= 2000
            var DSThongKeThang = db.HOADONs.Where(n=>n.NGAYDAT.Value.Month == IdMonth && n.NGAYDAT.Value.Year==IdYear).ToList();
            ViewBag.DS = DSThongKeThang;
            return PartialView();
        }
    }
}