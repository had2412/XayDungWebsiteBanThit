using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSach.Models
{
    public class ThongKeDoanhThuViewModel
    {
        public int MaThit { get; set; }
        public string TenThit { get; set; }
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }
}