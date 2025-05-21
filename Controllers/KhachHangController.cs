using BanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BanSach.Controllers
{
    public class KhachHangController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: KhachHang
        public ActionResult ThongTin()
        {
            var khachHang = Session["KhachHang"] as BanSach.Models.KhachHang;
            if (khachHang == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }

            var donHangs = db.DonHangs
                .Where(d => d.MaKH == khachHang.MaKH)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            // Lấy chi tiết đơn hàng cho tất cả đơn hàng đã lấy, kèm luôn thông tin sản phẩm (Thit)
            var maDonHangs = donHangs.Select(d => d.MaDonHang).ToList();
            var chiTietDonHangs = db.ChiTietDonHangs
                .Include(ct => ct.Thit) // load dữ liệu sản phẩm (Thit)
                .Where(ct => maDonHangs.Contains(ct.MaDonHang))
                .ToList();

            ViewBag.DonHangs = donHangs;
            ViewBag.ChiTietDonHangs = chiTietDonHangs;

            return View(khachHang);
        }


        // GET: Chỉnh sửa
        public ActionResult ChinhSua()
        {
            var khachHang = Session["KhachHang"] as KhachHang;
            if (khachHang == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            return View(khachHang);
        }

        // POST: Lưu chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                var kh = db.KhachHangs.FirstOrDefault(k => k.MaKH == model.MaKH);
                if (kh != null)
                {
                    kh.HoTen = model.HoTen;
                    kh.TaiKhoan = model.TaiKhoan;
                    kh.MatKhau = model.MatKhau;
                    kh.Email = model.Email;
                    kh.DiaChi = model.DiaChi;
                    kh.DienThoai = model.DienThoai;
                    kh.GioiTinh = model.GioiTinh;
                    kh.NgaySinh = model.NgaySinh;

                    db.SaveChanges();

                    Session["KhachHang"] = kh; // Cập nhật lại session
                    return RedirectToAction("ThongTin");
                }
            }
            return View(model);
        }


    }
}