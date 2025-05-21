using BanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class ThongKeController : Controller
{
    QuanLyThitEntities db = new QuanLyThitEntities();

    public ActionResult DoanhThu(DateTime? fromDate, DateTime? toDate)
    {
        var db = new BanSach.Models.QuanLyThitEntities();

        var chiTietDonHangs = db.ChiTietDonHangs.AsQueryable();

        if (fromDate.HasValue)
        {
            chiTietDonHangs = chiTietDonHangs.Where(ct => ct.DonHang.NgayDat >= fromDate);
        }

        if (toDate.HasValue)
        {
            chiTietDonHangs = chiTietDonHangs.Where(ct => ct.DonHang.NgayDat <= toDate);
        }

        var thongKe = chiTietDonHangs
            .GroupBy(ct => ct.MaThit)
            .Select(g => new ThongKeDoanhThuViewModel
            {
                MaThit = g.Key,
                TenThit = g.FirstOrDefault().Thit.TenThit,
                SoLuongBan = g.Sum(ct => ct.SoLuong) ?? 0,
                DoanhThu = g.Sum(ct => ct.SoLuong * ct.DonGia) ?? 0
            }).ToList();

        ViewBag.TongDoanhThu = thongKe.Sum(t => t.DoanhThu);

        return View(thongKe);
    }

}
