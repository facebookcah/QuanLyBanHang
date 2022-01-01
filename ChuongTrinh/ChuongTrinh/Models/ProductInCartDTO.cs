using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuongTrinh.Models
{
    public class ProductInCartDTO
    {
        public int MaSP { get; set; }

        public string TenSP { get; set; }


        public decimal GiaBan { get; set; }


        public string HinhAnh { get; set; }


        public int SoLuong { get; set; }
        public ProductInCartDTO(SanPham product, int quantity)
        {
            this.MaSP = product.MaSP;
            this.TenSP = product.TenSP;
            this.GiaBan = product.GiaBan;
            this.HinhAnh = product.HinhAnh;
            this.SoLuong = quantity;

        }
    }
}