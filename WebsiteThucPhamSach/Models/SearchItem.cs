using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteThucPhamSach.Models
{
    public class SearchItem
    {
       
        public int Id { get; set; }
        public string Name { get; set; }

        public SearchItem(int iD,string name) {
            this.Id = iD;
            this.Name = name;
        }
    }
}