using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteThucPhamSach.Models
{
    [Serializable]
    public class HTThanhToanIteam
    {
       
        public int Id { get; set; }
        public string Name { get; set; }

        public HTThanhToanIteam(int iD,string name) {
            this.Id = iD;
            this.Name = name;
        }
    }
}