using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;
using PagedList;
using PagedList.Mvc;

namespace BanSach.Controllers
{
    public class HomeController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Menu(int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            // Lấy tất cả sản phẩm và kết hợp với sản phẩm giảm giá
            var thits = db.Thits.OrderBy(n => n.GiaBan);

            var thitGiamGia = db.Thits
                .Where(t => t.PhanTramGiam.HasValue && t.PhanTramGiam > 0)
                .OrderByDescending(t => t.PhanTramGiam)
                .ToList();

            var allProducts = thits.ToList();  // Tất cả sản phẩm thông thường

            // Gộp cả sản phẩm giảm giá và sản phẩm thông thường
            var allProductsWithDiscounts = allProducts.Concat(thitGiamGia).Distinct().ToList();

            return View(allProductsWithDiscounts.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            var existingUser = db.KhachHangs.SingleOrDefault(x => x.TaiKhoan == kh.TaiKhoan);
            
            if (string.IsNullOrEmpty(kh.HoTen) || string.IsNullOrEmpty(kh.TaiKhoan) || string.IsNullOrEmpty(kh.MatKhau) || string.IsNullOrEmpty(kh.Email) || string.IsNullOrEmpty(kh.DiaChi) || string.IsNullOrEmpty(kh.DienThoai) || string.IsNullOrEmpty(kh.GioiTinh))
            {
                ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin bắt buộc.");
                return View(kh);
            }
            else if (kh.NgaySinh == null)
            {
                ModelState.AddModelError("","Vui lòng chọn ngày sinh.");
                return View(kh);
            }
            else if (kh.MatKhau != kh.NhapLaiMatKhau)
            {
                ModelState.AddModelError("", "Mật khẩu không trùng nhau vui lòng nhập lại!");
                return View(kh);
            }

            else
            {
                // Kiểm tra xem tài khoản đã tồn tại trong cơ sở dữ liệu chưa
                
                if(existingUser != null)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                    return View(kh);
                }
                
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
        }


        public ActionResult DangNhap()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(DangNhapModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userCheck = db.KhachHangs.SingleOrDefault(x => x.TaiKhoan.Equals(model.TaiKhoan) && x.MatKhau.Equals(model.MatKhau));
            var adminCheck = db.Admins.SingleOrDefault(x => x.TaiKhoan.Equals(model.TaiKhoan) && x.MatKhau.Equals(model.MatKhau));

            if (userCheck != null)
            {
                Session["KhachHang"] = userCheck;
                
                return RedirectToAction("Index", "Home");
            }
            else if (adminCheck != null) 
            {
                Session["Admin"] = adminCheck;

                return RedirectToAction("Index", "QuanLySanPham");
            }
            else if(string.IsNullOrWhiteSpace(model.TaiKhoan) )
            {
                ViewBag.LoginFail = "Tài khoản không được bỏ trống";
                return View("DangNhap");
            }
            else if (string.IsNullOrWhiteSpace(model.MatKhau))
            {
                ViewBag.LoginFail = "Mật khẩu không được bỏ trống";
                return View("DangNhap");
            }
            else
            {
                ViewBag.LoginFail = "Tài khoản hoặc mật khẩu không đúng";
                return View("DangNhap");
            }

        }

        public class DangNhapModel
        {
            public string TaiKhoan { get; set; }
            public string MatKhau { get; set; }
        }

        public ActionResult DangXuat()
        {
            Session["KhachHang"] = null;
            return RedirectToAction("Index","Home");
        }

        public ActionResult AboutUs()
        {

            return View();
        }

        public ActionResult DatHang()
        {

            return View();
        }

        public PartialViewResult SachMoiPartial()
        {
            var lstSachMoi = db.Thits.Take(3).ToList();
            return PartialView(lstSachMoi);
        }




        public ViewResult ThitTheoChuDe(int MaChuDe = 0)
        {
            ChuDe cd = db.ChuDes.SingleOrDefault(n => n.MaChuDe == MaChuDe);
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<Thit> lstthit = db.Thits.Where(n => n.MaChuDe == MaChuDe).OrderBy(n => n.GiaBan).ToList();

            if (lstthit.Count == 0)
            {
                ViewBag.Sach = "Không có thịt nào thuộc chủ đề này";
            }

            // Load danh sách chủ đề cho trang Index
            ViewBag.ChuDes = db.ChuDes.ToList();

            return View(lstthit);
        }


        public ViewResult DanhMucChuDe()
        {

            return View(db.ChuDes.ToList());
        }
        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LienHe(LienHe lh)
        {
            if (ModelState.IsValid)
            {
                db.LienHes.Add(lh);
                db.SaveChanges();
                ViewBag.ThongBao = "Gửi liên hệ thành công!";
                ModelState.Clear(); // xóa form sau khi gửi
            }
            else
            {
                ViewBag.ThongBao = "Vui lòng nhập đầy đủ thông tin.";
            }

            return View();
        }

        public ActionResult TinTuc() {


            return View();
        }



    }
}