namespace ChuongTrinh.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            HoaDons = new HashSet<HoaDon>();
            TaiKhoans = new HashSet<TaiKhoan>();
        }

        [Key]
        [Display(Name = "Mã khách hàng")]

        public int MaKH { get; set; }
        [Display(Name = "Họ tên")]

        [Required(ErrorMessage ="Họ tên là bắt buộc!")]
        [StringLength(100, ErrorMessage = "Họ tên không quá 100 kí tự !")]
        public string TenKH { get; set; }

        [Display(Name = "Giới tính")]
        public bool? GioiTinh { get; set; }
        [Display(Name = "Số điện thoại")]

        [Required(ErrorMessage ="Số điện thoại là bắt buộc!")]
        [StringLength(10,ErrorMessage ="Số điện thoại không quá 10 kí tự")]
        public string SoDienThoai { get; set; }
        [Display(Name = "Ngày sinh")]

        public DateTime? NgaySinh { get; set; }
        [Display(Name ="Email")]
        [EmailAddress(ErrorMessage ="Địa chỉ Email không đúng định dạng!")]
        [StringLength(25)]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]

        [Required(ErrorMessage ="Địa chỉ là bắt buộc!")]
        [StringLength(200,ErrorMessage ="Đại chỉ không quá 200 kí tự !")]
        public string DiaChi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaiKhoan> TaiKhoans { get; set; }
    }
}
