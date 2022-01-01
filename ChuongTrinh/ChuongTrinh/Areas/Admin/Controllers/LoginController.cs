using ChuongTrinh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuongTrinh.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly QuanLyBanHangDB db = new QuanLyBanHangDB();

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Account account)
        {

            if (ModelState.IsValid)
            {
                var isExist = db.TaiKhoans.Where(i => i.TenDangNhap == account.UserName && i.MatKhau == account.Password).FirstOrDefault();
                if (isExist != null)
                {
                    if (isExist.MaQuyen == 2)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ViewBag.Message = "Tài khoản không có quyền truy cập!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Tên tài khoản hoặc mật khẩu không chính xác!";
                    return View();
                }
            }
            return View();

        }
    }
}