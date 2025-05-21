using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: /QuanLyDonHang/Index
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }

            return View(db.DonHangs.ToList());
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoi(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                // Lưu đơn hàng vào CSDL
                db.DonHangs.Add(donHang);
                db.SaveChanges();

                // Chuyển hướng về danh sách đơn hàng hoặc trang xác nhận
                return RedirectToAction("Index"); // Nếu có trang Index hiển thị đơn hàng
            }

            // Nếu không hợp lệ, giữ nguyên form để hiển thị lỗi
            return View(donHang);
        }

        public ActionResult Delete(int MaDonHang)
        {
            DonHang donhang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);
            
            if (donhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donhang);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult AcceptDetele(int MaDonHang)
        {
            DonHang donhang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);

            if (donhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Xóa tất cả chi tiết đơn hàng liên quan trước
            var chiTietDonHang = db.ChiTietDonHangs.Where(ct => ct.MaDonHang == MaDonHang).ToList();
            db.ChiTietDonHangs.RemoveRange(chiTietDonHang);

            // Sau đó xóa đơn hàng
            db.DonHangs.Remove(donhang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int MaDonHang)
        {
            DonHang donhang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == MaDonHang);

            

            return View(donhang);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DonHang donhang, FormCollection f)
        {
            
            if (donhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                if (ModelState.IsValid)
                {



                    db.Entry(donhang).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            


            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            DonHang donhang = db.DonHangs.SingleOrDefault(n => n.MaDonHang == id);
            
            if (donhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(donhang);
        }



    }
}
