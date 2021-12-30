using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuongTrinh.Models
{
    public class ProductDTO
    {
        public int MaSP { get; set; }

        public string TenSP { get; set; }

        public bool? TrangThai { get; set; }

        public double KhoiLuong { get; set; }


        public decimal GiaBan { get; set; }


        public string MoTa { get; set; }


        public string HinhAnh { get; set; }

        public string TenDanhMuc { get; set; }
        public int SoLuong { get; set; }
        public double TongTien { get; set; }
        public ProductDTO(SanPham product, int quantity)
        {
            this.MaSP = product.MaSP;
            this.TenSP = product.TenSP;
            this.TrangThai = product.TrangThai;
            this.KhoiLuong = product.KhoiLuong;
            this.GiaBan = product.GiaBan;
            this.MoTa = product.MoTa;
            this.HinhAnh = product.HinhAnh;
            this.TenDanhMuc = product.DanhMuc.TenDanhMuc;
            this.SoLuong = quantity;
            this.TongTien = Convert.ToDouble(this.GiaBan * quantity);
        }
    }
}