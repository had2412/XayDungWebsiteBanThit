using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class QuanLyLienHeController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: QuanLyLienHe
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("DangNhap", "LoginAdmin");
            }

            return View(db.LienHes.ToList());
        }
    }
}