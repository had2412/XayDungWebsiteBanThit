using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BanSach.Models
{
    public class ThitGiamGiaViewModel
    {
        public int MaThit { get; set; }

        public string TenThit { get; set; }

        public decimal GiaBan { get; set; }

        [Display(Name = "Phần trăm giảm (%)")]
        [Range(0, 100, ErrorMessage = "Giá trị từ 0 đến 100")]
        public int? PhanTramGiam { get; set; }
    }
}