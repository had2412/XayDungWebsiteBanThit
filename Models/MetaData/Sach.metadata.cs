using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BanSach.Models.MetaData
{
    [MetadataTypeAttribute(typeof(SachMetadata))]
    public partial class Sach
    {
        internal sealed class SachMetadata
        {
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường hợp này.")]
            [Display(Name ="Mã sách")]

            public int MaSach {  get; set; }
            [Display(Name = "Tên sách")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường hợp này.")]

            public string TenSach { get; set; }
            [Display(Name = "Giá bán")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường hợp này.")]
            public Nullable<decimal> GiaBan { get; set; }
            [Display(Name = "Mô tả")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường hợp này.")]
            public string MoTa { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
            [Display(Name = "Ngày cập nhật")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường hợp này.")]
            public Nullable<System.DateTime> NgayCapNhat { get; set; }

            [Display(Name = "Ảnh bìa")]
            
            public string AnhBia { get; set; }

            [Display(Name = "Số lượng tồn")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường hợp này.")]
            public Nullable<int> SoLuongTon { get; set; }

            [Display(Name = "Mã chủ đề")]
            
            public Nullable<int> MaChuDe { get; set; }

            [Display(Name = "Mã NXB")]
            
            public Nullable<int> MaNXB { get; set; }

            [Display(Name = "Mới")]
            [Required(ErrorMessage = "Vui lòng nhập dữ liệu cho trường hợp này.")]
            public Nullable<int> Moi { get; set; }
        }
    }
}