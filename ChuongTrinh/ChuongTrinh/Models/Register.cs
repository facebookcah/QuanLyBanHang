using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChuongTrinh.Models
{
    public partial class Register
    {
       
        [Display(Name = "Họ tên")]

        [Required(ErrorMessage = "Họ tên là bắt buộc!")]
        [StringLength(100, ErrorMessage = "Họ tên không quá 100 kí tự !")]
        public string TenKH { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Giới tính là bắt buộc!")]

        public bool? GioiTinh { get; set; }
        [Display(Name = "Số điện thoại")]

        [Required(ErrorMessage = "Số điện thoại là bắt buộc!")]
        [StringLength(10, ErrorMessage = "Số điện thoại không quá 10 kí tự")]
        public string SoDienThoai { get; set; }
        [Display(Name = "Ngày sinh")]

        public DateTime? NgaySinh { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage ="Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email không đúng định dạng!")]
        [StringLength(30,ErrorMessage ="Địa chỉ email không qua 30 kí tự")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]

        [Required(ErrorMessage = "Địa chỉ là bắt buộc!")]
        [StringLength(200, ErrorMessage = "Đại chỉ không quá 200 kí tự !")]
        public string DiaChi { get; set; }
        [Display(Name = "Tên đăng nhập")]

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc !")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc !")]
        [Display(Name = "Mật khẩu")]

        public string Password { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]

        [Compare("Password",ErrorMessage ="Mật khẩu không khớp !")]

        public string RePassword { get; set; }
    }
}