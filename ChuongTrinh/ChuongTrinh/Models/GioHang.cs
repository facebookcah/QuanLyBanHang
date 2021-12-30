namespace ChuongTrinh.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GioHang")]
    public partial class GioHang
    {
        [Key]
        public int MaGioHang { get; set; }

        public int MaTK { get; set; }

        public int MaSP { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "money")]
        public decimal Gia { get; set; }

        public int TrangThai { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
