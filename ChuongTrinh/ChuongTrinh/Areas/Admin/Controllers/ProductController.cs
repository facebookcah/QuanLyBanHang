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
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.DanhMuc);
            return View(sanPhams.ToList());
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
            if (ModelState.IsValid)
            {
                //Kiểm tra nếu có ảnh thì lưu ảnh vào server và tên ảnh vào database
                if (file != null && file.ContentLength>0 )
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string pathServer = Path.Combine(Server.MapPath("~/wwwroot/Client/images/products"), fileName);
                    file.SaveAs(pathServer);
                    sanPham.HinhAnh = fileName;
                }    
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
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
                return RedirectToAction("Index");
            }
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
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
            //lấy danh sách hóa đơn 
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
