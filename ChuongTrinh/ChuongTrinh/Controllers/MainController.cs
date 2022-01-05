using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using ChuongTrinh.Models;
using PagedList;
using System.Net;

namespace ChuongTrinh.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        // này là controller main client nha là màn Home á
        QuanLyBanHangDB db = new QuanLyBanHangDB();
        public ActionResult About()
        {
            Session["categories"] = db.DanhMucs.ToList();
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Main(int? page, int? madm)
        {

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var products = db.SanPhams.Select(p => p).ToList();
            var categories = db.DanhMucs.ToList();
            Session["categories"] = categories;
            if (madm > 0)
            {
                var products1 = products.Where(i => i.MaDanhMuc == madm).ToList();
                return View(products1.ToPagedList(pageNumber, pageSize));
            }
            else
            {

                return View(products.ToPagedList(pageNumber, pageSize));
            }

        }
        public ActionResult Cart(int? masp)
        {

            var product = db.SanPhams.Find(masp);
            var userName = Session["UserName"] as string;
            if (userName != null)
            {
                var account = GetAccount();
                if (account != null)
                {
                    var cart = db.GioHangs.Where(i => i.MaSP == masp && i.MaTK == account.MaTK).FirstOrDefault();
                    if (cart == null)
                    {
                        GioHang cartOfAccount = new GioHang();
                        cartOfAccount.MaTK = account.MaTK;
                        cartOfAccount.MaSP = (int)masp;
                        cartOfAccount.SoLuong = 1;
                        cartOfAccount.Gia = product.GiaBan;
                        cartOfAccount.TrangThai = 1;
                        db.GioHangs.Add(cartOfAccount);
                        db.SaveChanges();
                        

                        return RedirectToAction("CartOfAccount",GetProductsInCart());
                    }
                    else
                    {
                        db.Entry(cart).State = EntityState.Modified;
                        cart.SoLuong += 1;
                        db.SaveChanges();
                        return RedirectToAction("CartOfAccount",GetProductsInCart());
                    }
                }
            }
            ViewBag.Message = "Vui lòng đăng nhập để tiếp tụ mua sắm ^^";
            return View("NoProductInCart");
        }
        public ActionResult CartOfAccount()
        {
            var userName = Session["UserName"] as string;
            if (userName == null)
            {
                ViewBag.Message = "Vui lòng đăng nhập để tiếp tụ mua sắm ^^";
                return View("NoProductInCart");
            }

            else
            {
                return View("CartOfAccount", GetProductsInCart());
            }
         
            
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }
            var isExist = db.TaiKhoans.Any(i => i.TenDangNhap.Equals(account.UserName) && i.MatKhau.Equals(account.Password));
            if (!isExist)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác";
                return View(account);
            }
            else
            {
                var acc = db.TaiKhoans.Where(i => i.TenDangNhap.Equals(account.UserName)).FirstOrDefault();
                if (acc.MaQuyen != 1)
                {
                    ViewBag.Error = "Tài khoản không có quyền truy cập !!";
                    return View(account);
                }
                else
                {
                    Session["UserName"] = acc.TenDangNhap;
                    return RedirectToAction("Main");
                }
            }


        }

        public ActionResult Order()
        {
            //kiểm tra đăng nhập 
            var userName = Session["UserName"] as string;
            var account = db.TaiKhoans.Where(i => i.TenDangNhap.ToLower() == userName.ToLower()).FirstOrDefault();

            //tạo hóa đơn cho khách hàng
            HoaDon order = new HoaDon();
            order.MaKH = account.MaKH;
            order.NgayLap = DateTime.Now;
            order.TinhTrang = 1;
            order.GhiChu = "";
            db.HoaDons.Add(order);
            db.SaveChanges();
            Session["MaHD"] = order.MaHD;
            //lấy thông tin từ giỏ hàng của khách hàng
            //chuyển thông tin từ giở hàng vào đơn hàng
            List<ProductInCartDTO> products = new List<ProductInCartDTO>();
            var productsInCart = db.GioHangs.Where(i => i.MaTK == account.MaTK).ToList();

            foreach (var i in productsInCart)
            {
                //thêm vào bảng chi tiết hóa đơn
                ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
                chiTietHoaDon.MaHD = order.MaHD;
                chiTietHoaDon.MaSP = i.MaSP;
                chiTietHoaDon.Gia = i.Gia;
                chiTietHoaDon.SoLuong = i.SoLuong;
                db.ChiTietHoaDons.Add(chiTietHoaDon);
            }
            db.SaveChanges();
            ViewBag.Change = "change";
            return View(GetProductsInCart());
        }


        public ActionResult RemoveFromCart(int? productCode)
        {
            var account = GetAccount();

            if (account != null)
            {
                var productInCart = db.GioHangs.Where(i => i.MaTK == account.MaTK && i.MaSP == productCode).FirstOrDefault();
                db.GioHangs.Remove(productInCart);
                db.SaveChanges();
                
            }
            return RedirectToAction("CartOfAccount",GetProductsInCart());
        }


        public ActionResult Up(int? code)
        {
            var account = GetAccount();
            var cart = db.GioHangs.Where(i => i.MaSP == code && i.MaTK == account.MaTK).FirstOrDefault();
            db.Entry(cart).State = EntityState.Modified;
            cart.SoLuong += 1;
            db.SaveChanges();
            return View("CartOfAccount",GetProductsInCart());
        }
        public ActionResult Down(int? code)
        {
            var account = GetAccount();
            var cart = db.GioHangs.Where(i => i.MaSP == code && i.MaTK == account.MaTK).FirstOrDefault();
            if (cart.SoLuong == 1)
            {
                db.GioHangs.Remove(cart);
                db.SaveChanges();
            }
            else
            {
                db.Entry(cart).State = EntityState.Modified;
                cart.SoLuong -= 1;
                db.SaveChanges();
            }
            return View("CartOfAccount", GetProductsInCart());
        }

        public TaiKhoan GetAccount()
        {
            var userName = Session["UserName"] as string;
            if (userName != null)
            {
                return db.TaiKhoans.Where(i => i.TenDangNhap.ToLower().Equals(userName.ToLower())).FirstOrDefault();
            }
            return new TaiKhoan();
        }
        public List<ProductInCartDTO> GetProductsInCart()
        {
            var account = GetAccount();
            var productsInCart = db.GioHangs.Where(i => i.MaTK == account.MaTK).ToList();
            List<ProductInCartDTO> products = new List<ProductInCartDTO>();
            foreach (var i in productsInCart)
            {
                var productInCart = db.SanPhams.Find(i.MaSP);
                ProductInCartDTO productInCartDTO = new ProductInCartDTO(productInCart, i.SoLuong);
                products.Add(productInCartDTO);
            }
            return products;
        }

        public ActionResult DetailAccount()
        {
            var account = GetAccount();
            return View(account);
        }
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            return RedirectToAction("Main");
        }


        public ActionResult EditInfor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfor(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DetailAccount", "Main");
            }
            return View(khachHang);
        }

        public ActionResult EditAccount(int? id)
        {
            var account = db.TaiKhoans.Find(id);

            return View(account);
        }
        [HttpPost]
        public ActionResult EditAccount(TaiKhoan account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DetailAccount", "Main");
        }

        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Register(Register register)
        {
            if (register.TenKH != null && register.GioiTinh != null && register.SoDienThoai!=null && register.DiaChi!=null && register.UserName!=null && register.Password!=null &&register.Password==register.RePassword && register.Email!=null)
            {
                KhachHang client = new KhachHang();
                client.TenKH = register.TenKH;
                client.GioiTinh = register.GioiTinh;
                client.SoDienThoai = register.SoDienThoai;
                client.DiaChi = register.DiaChi;
                db.KhachHangs.Add(client);
                db.SaveChanges();
                TaiKhoan account = new TaiKhoan();
                account.TenDangNhap = register.UserName;
                account.MatKhau = register.Password;
                account.MaKH = client.MaKH;
                account.MaQuyen = 1;
                db.TaiKhoans.Add(account);
                db.SaveChanges();
                
            }
            else
            {
                return View(register);
            }
            return RedirectToAction("Login");
        }
        public ActionResult CancleOrder()
        {
            var maHD = Session["MaHD"] ;
            var order = db.HoaDons.Find(Convert.ToInt32(maHD));
                db.Entry(order).State = EntityState.Modified;
            order.TinhTrang = 3;
            db.SaveChanges();
            ViewBag.Success = "Hủy đơn hàng thành công";
            return RedirectToAction("Main");
           
        }

        [HttpPost]
        public ActionResult Search(int? from,int? to,int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var products = db.SanPhams.Where(i => i.GiaBan >= from && i.GiaBan <= to).ToList().ToPagedList(pageNumber,pageSize);
            return View("Main",products);
        }


    }
}