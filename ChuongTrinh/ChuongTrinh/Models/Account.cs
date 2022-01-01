using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChuongTrinh.Models
{
   
    public class Account
    {
        [Required(ErrorMessage ="Tên đăng nhập là bắt buộc !")]
        public string UserName{get;set;}
        [Required(ErrorMessage = "Mật khẩu là bắt buộc !")]

        public string Password{get;set;}
       
    }
}