using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChuongTrinh.Models
{
    public class OrderDTO
    {
        [Display(Name = "Mã hóa đơn")]

        public int MaHD { get; set; }
        [Display(Name = "Mã khách hàng")]

        public int? MaKH { get; set; }
        [Display(Name = "Ngày lập")]

        public DateTime? NgayLap { get; set; }
        [Display(Name = "Ghi chú")]

        public string GhiChu { get; set; }
        [Display(Name = "Tình trạng")]

        public int? TinhTrang { get; set; }
        public List<ProductDTO> Products { get; set; }

        public OrderDTO(HoaDon order, List<ProductDTO> products)
        {
            this.MaHD = order.MaHD;
            this.MaKH = order.MaKH;
            this.NgayLap = order.NgayLap;
            this.GhiChu = order.GhiChu;
            this.TinhTrang = order.TinhTrang;
            this.Products = products;
        }
    }
}