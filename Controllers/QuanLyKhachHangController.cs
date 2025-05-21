using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;
using System.Data.Entity.Validation;

namespace BanSach.Controllers
{
    public class QuanLyKhachHangController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();

        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }

            return View(db.KhachHangs.ToList());
        }

        [HttpGet]
        public ActionResult ThemMoi()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoi(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                if (kh.MatKhau != kh.NhapLaiMatKhau)
                {
                    ModelState.AddModelError("NhapLaiMatKhau", "Mật khẩu nhập lại không khớp");
                    return View(kh);
                }

                try
                {
                    db.KhachHangs.Add(kh);
                    db.SaveChanges();
                    TempData["Success"] = "Thêm khách hàng thành công!";
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return View(kh);

        }
        [HttpGet]
        public ActionResult Edit(int MaKH)
        {
            if (MaKH == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            KhachHang kh = db.KhachHangs.Find(MaKH);
            if (kh == null)
            {
                return HttpNotFound();
            }

            return View(kh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                if (kh.MatKhau != kh.NhapLaiMatKhau)
                {
                    ModelState.AddModelError("NhapLaiMatKhau", "Mật khẩu nhập lại không khớp");
                    return View(kh);
                }

                db.Entry(kh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Cập nhật khách hàng thành công!";
                return RedirectToAction("Index");
            }

            return View(kh);
        }
        public ActionResult Details(int MaKH)
        {
            KhachHang khachhang = db.KhachHangs.SingleOrDefault(n => n.MaKH == MaKH);

            if (khachhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachhang);
        }
        [HttpGet]
        public ActionResult Delete(int MaKH)
        {
            if (MaKH == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            KhachHang kh = db.KhachHangs.Find(MaKH);
            if (kh == null)
            {
                return HttpNotFound();
            }

            return View(kh);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int MaKH)
        {
            KhachHang kh = db.KhachHangs.Find(MaKH);
            if (kh != null)
            {
                db.KhachHangs.Remove(kh);
                db.SaveChanges();
                TempData["Success"] = "Xóa khách hàng thành công!";
            }

            return RedirectToAction("Index");
        }

    }
}
