using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"]=lstGioHang;

            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int iMaThit,string strURL) {

            Thit thit= db.Thits.SingleOrDefault(n=>n.MaThit == iMaThit);
            if (thit==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.Find(n=>n.iMaThit == iMaThit);
            if(sanpham == null)
            {
                sanpham = new GioHang(iMaThit);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
           
        }
        public ActionResult CapNhatGioHang(int iMaSP,FormCollection f)
        {
            Thit sach = db.Thits.SingleOrDefault(n => n.MaThit==iMaSP);
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n=>n.iMaThit==iMaSP);
            if(sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int iMaSP)
        {
            Thit thit = db.Thits.SingleOrDefault(n => n.MaThit == iMaSP);
            if (thit == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaThit == iMaSP);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n=>n.iMaThit == iMaSP);
                
            }
            if (lstGioHang.Count ==0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();

            return View(lstGioHang);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if( lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }

        public ActionResult GioHangPartial()
        {
            if(TongSoLuong()==0) 
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();

            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }

            // Kiểm tra xem giỏ hàng có sản phẩm không
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            DonHang ddh = new DonHang();
            KhachHang khhang = (KhachHang)Session["KhachHang"]; // Sử dụng "KhachHang" thay vì "TaiKhoan"
            List<GioHang> gh = LayGioHang();
            ddh.MaKH = khhang.MaKH;
            ddh.NgayDat = DateTime.Now;
            db.DonHangs.Add(ddh);
            db.SaveChanges();

            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaThit = item.iMaThit;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.ChiTietDonHangs.Add(ctdh);
            }
            db.SaveChanges();

            TempData["ThongBao"] = "Bạn đã đặt hàng thành công!";

            // Xóa giỏ hàng sau khi đã đặt hàng
            Session["GioHang"] = null;

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ThanhToan()
        {
            if (Session["GioHang"] == null)
                return RedirectToAction("Index", "Home");

            var cart = LayGioHang();

            var model = new OrderViewModel
            {
                GioHangItems = cart
            };

            if (Session["KhachHang"] != null)
            {
                KhachHang kh = (KhachHang)Session["KhachHang"];
                model.HoTen = kh.HoTen;   // Lấy thông tin từ khách hàng đăng nhập
                model.DienThoai = kh.DienThoai;
                model.DiaChi = kh.DiaChi;
            }

            return View("ThanhToan", model); // View: Views/GioHang/ThanhToan.cshtml
        }
        [HttpPost]
        public ActionResult ThanhToan(OrderViewModel model)
        {
            if (Session["GioHang"] == null)
                return RedirectToAction("Index", "Home");

            var cart = LayGioHang();

            if (ModelState.IsValid)
            {
                decimal tongTien = cart.Sum(x => (decimal)x.ThanhTien);
                decimal giamGia = 0;
                decimal tyLeGiam = 0;
                // Xử lý voucher nếu có
                if (!string.IsNullOrEmpty(model.MaVoucher))
                {
                    var voucher = db.Vouchers.SingleOrDefault(v =>
                        v.MaVoucher == model.MaVoucher &&
                        v.NgayBatDau <= DateTime.Now &&
                        v.NgayKetThuc >= DateTime.Now &&
                        v.SoLuong > 0);

                    if (voucher != null)
                    {
                        tyLeGiam = (decimal)voucher.GiaTriGiam / 100;
                        decimal phanTramGiam = (decimal)voucher.GiaTriGiam; // VD: 10 (%)
                        giamGia = tongTien * (phanTramGiam / 100); // Tính tiền giảm
                        voucher.SoLuong -= 1;
                        db.Entry(voucher).State = EntityState.Modified;
                    }
                }

                decimal tongSauGiam = tongTien - giamGia;
                if (tongSauGiam < 0) tongSauGiam = 0;

                // Lưu đơn hàng
                DonHang ddh = new DonHang
                {
                    NgayDat = DateTime.Now,
                    GhiChu = model.GhiChu,
                    HoTen = model.HoTen,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,

                    TongTien = tongSauGiam,
                    MaVoucher = model.MaVoucher
                };

                if (Session["KhachHang"] != null)
                {
                    KhachHang kh = (KhachHang)Session["KhachHang"];
                    ddh.MaKH = kh.MaKH;
                }

                db.DonHangs.Add(ddh);
                db.SaveChanges();

                foreach (var item in cart)
                {
                    var donGiaSauGiam = (decimal)item.dDonGia;

                    if (tyLeGiam > 0)
                    {
                        donGiaSauGiam = (decimal)item.dDonGia * (1 - tyLeGiam);
                    }
                    ChiTietDonHang ctdh = new ChiTietDonHang
                    {
                        MaDonHang = ddh.MaDonHang,
                        MaThit = item.iMaThit,
                        SoLuong = item.iSoLuong,
                        DonGia = (decimal)item.dDonGia,
                        DonGiaSauGiam = (decimal)donGiaSauGiam
                    };
                    db.ChiTietDonHangs.Add(ctdh);
                }

                db.SaveChanges();
                TempData["ThongBao"] = "Bạn đã đặt hàng thành công!";
                Session["GioHang"] = null;

                // Lưu thông tin voucher cho trang xác nhận (tuỳ chọn)
                TempData["Voucher"] = model.MaVoucher;
                TempData["GiamGia"] = giamGia;
                TempData["TongTien"] = tongSauGiam;
                return RedirectToAction("XacNhan");
            }

            model.GioHangItems = cart;
            return View("ThanhToan", model);
        }


        public ActionResult XacNhan()
        {
            return View();
        }
    }
}