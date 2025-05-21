using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class LoginAdminController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: LoginAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DangNhap()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(Admin ad)
        {
            var taikhoanForm = ad.TaiKhoan;
            var matkhauForm = ad.MatKhau;
            var userCheck = db.Admins.SingleOrDefault(x => x.TaiKhoan.Equals(taikhoanForm) && x.MatKhau.Equals(matkhauForm));
            if (userCheck != null)
            {
                Session["Admin"] = userCheck;
                return RedirectToAction("Index", "QuanLySanPham");
            }
            else
            {
                ViewBag.LoginFail = "Tài khoản hoặc mật khẩu không đúng";
                return View("DangNhap");
            }

        }

        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("DangNhap", "LoginAdmin");
        }
    }
}