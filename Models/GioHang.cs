using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BanSach.Models
{
    public class GioHang
    {
       // private int iMaSP;

       // public int IMaSP { get => iMaSP; set => iMaSP = value; }
       QuanLyThitEntities db = new QuanLyThitEntities();

        public int iMaThit {  get; set; }

        public string sTenThit {  get; set; }

        public string sAnhBia { get; set; }

        public double dDonGia { get; set; }

        public int iSoLuong { get; set;}

        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        public GioHang(int MaSach)
        {
            iMaThit = MaSach;
            Thit sach = db.Thits.Single(n=>n.MaThit == iMaThit);
            sTenThit = sach.TenThit;
            sAnhBia = sach.AnhBia;
            dDonGia = double.Parse(sach.GiaBan.ToString());
            iSoLuong = 1;
        }

    }
}