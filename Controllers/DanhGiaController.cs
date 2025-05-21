using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class DanhGiaController : Controller
    { 
    QuanLyThitEntities db = new QuanLyThitEntities();

    [HttpPost]
    public ActionResult ThemDanhGia(DanhGia danhGia)
    {
        if (Session["KhachHang"] == null)
        {
            return RedirectToAction("DangNhap", "KhachHang");
        }

        var kh = (KhachHang)Session["KhachHang"];
        danhGia.MaKH = kh.MaKH;
        danhGia.NgayDanhGia = DateTime.Now;

        db.DanhGias.Add(danhGia);
        db.SaveChanges();

        return RedirectToAction("XemChiTiet", "Thit", new { MaThit = danhGia.MaThit });
    }
}
}