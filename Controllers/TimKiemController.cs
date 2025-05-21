using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;
using PagedList;
using PagedList.Mvc;

namespace BanSach.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: TimKiem
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f,int? page)
        {
            String sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<Thit> lstKQTK = db.Thits.Where(n=>n.TenThit.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 8;
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(db.Thits.OrderBy(n=>n.TenThit).ToPagedList(pageNumber,pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả!";
            return View(lstKQTK.OrderBy(n=>n.TenThit).ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {

            ViewBag.TuKhoa = sTuKhoa;
            List<Thit> lstKQTK = db.Thits.Where(n => n.TenThit.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 8;
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(db.Thits.OrderBy(n => n.TenThit).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả!";
            return View(lstKQTK.OrderBy(n => n.TenThit).ToPagedList(pageNumber, pageSize));
        }
    }
}