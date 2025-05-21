using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;
using PagedList;
using PagedList.Mvc;


namespace BanSach.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: QuanLySanPham
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            var thits = db.Thits.Include(t => t.ChuDe).OrderBy(t => t.MaThit);
            return View(db.Thits.ToList().OrderBy(n=>n.MaThit).ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        public ActionResult ThemMoi() 
        {
           
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Thit thit, HttpPostedFileBase fileUpload)
        {
           
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                return View();
            }
            else
            { 
                    if (ModelState.IsValid)
                    {
                    var fileName = Path.GetFileName(fileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/HinhAnhSp"), fileName);
                    {
                        fileUpload.SaveAs(path);
                    }
                    thit.AnhBia = fileUpload.FileName;
                    db.Thits.Add(thit);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Thit thit = db.Thits.SingleOrDefault(n => n.MaThit == id);
            ViewBag.Id = thit.MaThit;
            if(thit == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thit);
        }
        public ActionResult Delete(int id) 
        {
            Thit thit = db.Thits.SingleOrDefault(n => n.MaThit == id);
            ViewBag.Id = thit.MaThit;
            if (thit == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thit);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult AcceptDetele(int id)
        {
            Thit thit = db.Thits.SingleOrDefault(n => n.MaThit == id);
            if (thit == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Xóa tất cả ChiTietDonHang liên quan trước
            var chiTietDonHangs = db.ChiTietDonHangs.Where(ct => ct.MaThit == id).ToList();
            foreach (var item in chiTietDonHangs)
            {
                db.ChiTietDonHangs.Remove(item);
            }

            db.Thits.Remove(thit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int MaThit)
        {
            Thit thit = db.Thits.SingleOrDefault(n => n.MaThit == MaThit);
            if (thit == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaChuDe = new SelectList(db.ChuDes.OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", thit.MaChuDe);
            return View(thit);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Thit thit)
        {
            if (thit == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                db.Entry(thit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, gán lại ViewBag và trả về View để hiển thị lỗi
            ViewBag.MaChuDe = new SelectList(db.ChuDes.OrderBy(n => n.TenChuDe), "MaChuDe", "TenChuDe", thit.MaChuDe);
            return View(thit);
        }

    }

}
