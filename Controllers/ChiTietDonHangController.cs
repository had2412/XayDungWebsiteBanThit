using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class ChiTietDonHangController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: ChiTietDonHang
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }
            return View(db.ChiTietDonHangs.ToList());
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            ViewBag.MaDonHang = new SelectList(db.DonHangs.ToList(), "MaDonHang", "MaDonHang");
            ViewBag.MaThit = new SelectList(db.Thits.ToList(), "MaThit", "TenThit");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoi(ChiTietDonHang model)
        {
            if (ModelState.IsValid)
            {
                // Giả sử bạn có đối tượng context tên là db
                db.ChiTietDonHangs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index"); // hoặc về danh sách chi tiết đơn hàng
            }

            return View(model); // Trả về view nếu có lỗi
        }

        public ActionResult Details(int id)
        {
            ChiTietDonHang chitietdonhang = db.ChiTietDonHangs.SingleOrDefault(n => n.MaDonHang == id);

            if (chitietdonhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitietdonhang);
        }
        public ActionResult Delete(int MaDonHang)
        {
            // Lấy chi tiết đơn hàng đầu tiên với MaDonHang
            ChiTietDonHang chitietdonhang = db.ChiTietDonHangs.FirstOrDefault(n => n.MaDonHang == MaDonHang);

            if (chitietdonhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitietdonhang);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult AcceptDelete(int MaDonHang)
        {
            // Lấy tất cả chi tiết đơn hàng có MaDonHang
            var chiTietDonHang = db.ChiTietDonHangs.Where(ct => ct.MaDonHang == MaDonHang).ToList();

            if (chiTietDonHang == null || chiTietDonHang.Count == 0)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Xóa tất cả chi tiết đơn hàng liên quan
            db.ChiTietDonHangs.RemoveRange(chiTietDonHang);

            // Xóa đơn hàng (nếu cần)
            DonHang donhang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            if (donhang != null)
            {
                db.DonHangs.Remove(donhang);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int MaDonHang)
        {
            ChiTietDonHang chitietdonhang = db.ChiTietDonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);



            return View(chitietdonhang);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ChiTietDonHang chitietdonhang, FormCollection f)
        {

            if (chitietdonhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                if (ModelState.IsValid)
                {



                    db.Entry(chitietdonhang).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }



            return RedirectToAction("Index");
        }

    }
}