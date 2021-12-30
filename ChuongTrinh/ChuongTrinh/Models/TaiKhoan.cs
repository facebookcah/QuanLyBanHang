namespace ChuongTrinh.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            GioHangs = new HashSet<GioHang>();
        }

        [Key]
        [Display(Name = "Mã tài khoản")]

        public int MaTK { get; set; }

        [Required(ErrorMessage ="Tên đăng nhập không dược để trống !")]
        [Display(Name = "Tên đăng nhập")]

        [StringLength(50,ErrorMessage ="Không quá 50 kí tự !")]
        public string TenDangNhap { get; set; }
        [Display(Name = "Mật khẩu")]

        [Required(ErrorMessage ="Mật khẩu là bắt buộc !")]
        [StringLength(50)]
        public string MatKhau { get; set; }
        [Display(Name = "Quyền")]

        public int? MaQuyen { get; set; }
        [Display(Name = "Khách hàng")]

        public int MaKH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual Quyen Quyen { get; set; }
    }
}
