using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThucPhamSach.Models;
namespace WebsiteThucPhamSach.Controllers
{
    public class NavbarController : Controller
    {
        // GET: Navbar
        QLTHUCPHAMEntities1 db = new QLTHUCPHAMEntities1();

        public PartialViewResult NavbarPartial()
        {
            string navbar = "";
            var Menu = (from p in db.MENUs select p).Where(p => p.TRANGTHAI == true).ToList();
            var danhmuc = (from c in db.DANHMUCs select c).Where(c => c.TRANGTHAI == true).ToList();
            int dem = 0;
            if (Menu.Count() > 0)
            {
                navbar += "<ul class='nav navbar-nav custom-nav-bg'>";
                for (int i = 0; i < Menu.Count(); i++)
                {
                    if(Menu[i].MAMN != danhmuc[dem].MAMN)
                    {
                        dem++;
                        navbar += "<li class='parent'><a class='child' href=" + Menu[i].HREF + " ref='nofollow'>" + Menu[i].TENMN + "</a></li>";
                    } 
                    else
                    {
                        if (danhmuc.Count() > 0)
                        {
                            navbar += "<li class='dropdown parent khung'><a class='dropdown-toggle child' data-toggle='dropdown' href='javascript:0'>" + Menu[i].TENMN + "<span class='caret tamgiac'></span></a>";
                            navbar += "<ul class='dropdown-menu kethua'>";
                            int MADM = 0;
                            for (int j = 0; j < danhmuc.Count(); j++)
                            {
                                MADM = danhmuc[j].MADM;
                                navbar += "<li><a href='San-Pham?MADM=" + MADM + "'>" + danhmuc[j].TENDM + "</a></li>";
                            }
                            navbar += "</ul>";
                            navbar += "</li>";
                        }
                    }
                }
                navbar += "</ul>";
            }
            ViewBag.Navbar = navbar;
            return PartialView();
        }
    }
}