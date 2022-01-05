using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChuongTrinh.Models;
using System.IO;
using PagedList;

namespace ChuongTrinh.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private QuanLyBanHangDB db = new QuanLyBanHangDB();

        // GET: Admin/Product
        public ActionResult Index(string searchString, string sortOrder, string currentFilter, int? page, int? size)
        {
            /*if (Session["Username"] != null)
            {*/
                ViewBag.CurrentSort = sortOrder; // Lấy biến yêu cầu sắp xếp hiện tại
                ViewBag.SapTheoTen = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.SapTheoGiaBan = sortOrder == "GiaBan" ? "giaban_desc" : "GiaBan";

                if (searchString != null)
                {
                    page = 1; //trang đầu tiên
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;

                var sanPhams = db.SanPhams.Include(p => p.DanhMuc);
                //Lọc theo tên
                if (!String.IsNullOrEmpty(searchString))
                {
                    sanPhams = sanPhams.Where(p => p.TenSP.Contains(searchString));
                }
                //Sắp xếp
                switch (sortOrder)
                {
                    case "name_desc":
                        sanPhams = sanPhams.OrderByDescending(s => s.TenSP);
                        break;
                    case "giaban_desc":
                        sanPhams = sanPhams.OrderByDescending(s => s.GiaBan);
                        break;
                    default:
                        sanPhams = sanPhams.OrderBy(s => s.TenSP);
                        break;
                }

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "10", Value = "10" });
                items.Add(new SelectListItem { Text = "20", Value = "20" });
                items.Add(new SelectListItem { Text = "50", Value = "50" });

                foreach (var item in items)
                {
                    if (item.Value == size.ToString()) item.Selected = true;
                }

                ViewBag.size = items; // ViewBag DropDownList
                ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại

                int pageSize = (size ?? 10);
                int pageNumber = (page ?? 1);

                return View(sanPhams.ToPagedList(pageNumber, pageSize));
         /*   }
            else
            {
                return RedirectToAction("Index", "Account");
            }*/           
        }
        
        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSP,TenSP,TrangThai,KhoiLuong,GiaBan,MoTa,HinhAnh,MaDanhMuc")] SanPham sanPham,
           HttpPostedFileBase file )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Kiểm tra nếu có ảnh thì lưu ảnh vào server và tên ảnh vào database
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string pathServer = Path.Combine(Server.MapPath("~/wwwroot/Client/images/products"), fileName);
                        file.SaveAs(pathServer);
                        sanPham.HinhAnh = fileName;
                    }
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                return View(sanPham);
            }          
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,TrangThai,KhoiLuong,GiaBan,MoTa,HinhAnh,MaDanhMuc")] SanPham sanPham,
            HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiem tra neu co anh thi luu anh vao server va ten anh vao database
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string pathServer = Path.Combine(Server.MapPath("/wwwroot/Client/images/products"), fileName);

                        file.SaveAs(pathServer);
                        sanPham.HinhAnh = fileName;

                    }
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu!" + ex.Message;
                ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                return View(sanPham);
            }
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            try
            {   //lấy danh sách hóa đơn 
                ICollection<ChiTietHoaDon> chiTietHoaDons = sanPham.ChiTietHoaDons;
                //Kiểm tra nếu sản phẩm tồn tại trong hóa đơn thì thông báo lỗi không thể xóa
                if (chiTietHoaDons != null && chiTietHoaDons.Count > 0)
                {
                    ModelState.AddModelError("messages", "Sản phẩm này tồn tại trong hóa đơn, không thể xóa");
                    return View(sanPham);
                }
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không xoá được bản ghi này !" + ex.Message;
                return View("Delete", sanPham);
            }
         
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
