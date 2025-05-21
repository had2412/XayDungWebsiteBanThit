using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class ThitController : Controller
    {
        // GET: Sach
        QuanLyThitEntities db = new QuanLyThitEntities();
        public PartialViewResult SachMoiPartial()
        {
            var lstSachMoi = db.Thits.Take(3).ToList();
            return PartialView(lstSachMoi);
        }

        public ViewResult XemChiTiet(int MaThit=0)
        {
            Thit thit = db.Thits.SingleOrDefault(n=>n.MaThit==MaThit);
            if(thit == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            ViewBag.TenChuDe = db.ChuDes.Single(n => n.MaChuDe == thit.MaChuDe).TenChuDe;
            
            return View(thit);
        }

        [HttpPost]
        public ActionResult ThemDanhGia(DanhGia danhGia)
        {
            if (Session["KhachHang"] == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đánh giá.";
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