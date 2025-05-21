using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSach.Models
{
    public class OrderViewModel
    {
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string GhiChu { get; set; }

        public string MaVoucher { get; set; }
        public decimal GiaTriGiam { get; set; }

        public List<GioHang> GioHangItems { get; set; }
    }

}