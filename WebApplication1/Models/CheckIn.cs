using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CheckIn
    {
        public string mathoikhoabieu { get; set; }
        public string MASINHVIEN { get; set; }

        public string TENSINHVIEN { get; set; }
        public string Lop { get; set; }
        
        public string tengiangvien { get; set; }
        public string tenmonhoc { get; set; }

        public int SoBuoiDiemDanh { get; set; }
    }
}