using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;
using PagedList;

namespace BanSach.Controllers
{
    public class GiamGiaController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();

        // GET: GiamGia
        public ActionResult Menu(int? page)
        {

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var danhSachGiamGia = db.Thits
               .Where(t => t.PhanTramGiam.HasValue && t.PhanTramGiam > 0)
                .OrderByDescending(t => t.PhanTramGiam) // Ưu tiên giảm nhiều trước
                .ToPagedList(pageNumber, pageSize);

            return View(danhSachGiamGia);
        }
        public ActionResult QuanLy()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }
            var list = db.Thits.Select(t => new ThitGiamGiaViewModel
            {
                MaThit = t.MaThit,
                TenThit = t.TenThit,
                GiaBan = t.GiaBan ?? 0,
                PhanTramGiam = t.PhanTramGiam
            }).ToList();

            return View(list);
        }

        [HttpPost]
        public ActionResult CapNhatGiamGia(List<ThitGiamGiaViewModel> list)
        {
            foreach (var item in list)
            {
                var thit = db.Thits.FirstOrDefault(t => t.MaThit == item.MaThit);
                if (thit != null)
                {
                    thit.PhanTramGiam = item.PhanTramGiam;
                }
            }
            db.SaveChanges();
            TempData["ThongBao"] = "Cập nhật thành công!";
            return RedirectToAction("QuanLy");
        }

    }
}
