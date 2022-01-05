namespace ChuongTrinh.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        [Key]
        [Display(Name = "Mã SP")]

        public int MaSP { get; set; }
        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống !")]
        [StringLength(255)]
        public string TenSP { get; set; }
        [Display(Name = "Trạng thái")]

        public bool? TrangThai { get; set; }
        [Display(Name = "Khối lượng")]
        [Required(ErrorMessage = "Khối lượng không được để trống !")]
        public double KhoiLuong { get; set; }
        [Display(Name = "Giá bán")]

        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:#,###}" + " VNĐ")]
        public decimal GiaBan { get; set; }
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]

        [Column(TypeName = "ntext")]
        public string MoTa { get; set; }
        [Display(Name = "Hình ảnh")]

        [StringLength(100)]
        public string HinhAnh { get; set; }
        [DisplayName("Danh mục")]


        public int? MaDanhMuc { get; set; }
        [Display(Name = "Danh mục")]

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }
    }
}