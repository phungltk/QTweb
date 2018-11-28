using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QTweb.Models;
using System.Transactions;

namespace QTweb.Controllers
{
    public class BangSanPhamController : Controller
    {
        private CS4PEEntities db = new CS4PEEntities();

        // GET: /BangSanPham/
        public ActionResult Index()
        {
            var bangsanphams = db.BangSanPhams.Include(b => b.LoaiSanPham);
            return View(bangsanphams.ToList());
        }

        // GET: /BangSanPham/Details/5
        public FileResult Details(string id)
        {
            var path = Server.MapPath("~/App_Data");
            path = System.IO.Path.Combine(path, id);
            return File(path, "images");
        }

        // GET: /BangSanPham/Create
        public ActionResult Create()
        {
            
            ViewBag.Loai_id = new SelectList(db.LoaiSanPhams, "id", "TenLoai");
            return View();
        }

        // POST: /BangSanPham/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BangSanPham model)
        {
            CheckBangSanPham(model);
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                { 
                db.BangSanPhams.Add(model);
                db.SaveChanges();
                if (Request.Files["HinhAnh"] != null && Request.Files["HinhAnh"].ContentLength > 0)
                {
                    var path = Server.MapPath("~/App_Data");
                    path = System.IO.Path.Combine(path, model.id.ToString());
                    Request.Files["HinhAnh"].SaveAs(path);
                    scope.Complete();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("HinhAnh", "Chua chon hinh anh");
                }
                }
                
            }

            ViewBag.Loai_id = new SelectList(db.LoaiSanPhams, "id", "TenLoai", model.Loai_id);
            return View(model);
        }
        private void CheckBangSanPham(BangSanPham model)
        {
            if (model.GiaGoc < 0)
                ModelState.AddModelError("GiaGoc", "Gia goc phai lon hon 0");
            if (model.GiaGoc > model.GiaBan)
                ModelState.AddModelError("GiaBan", "Gia ban phai lon hon gia goc");
            if (model.SoLuongTon < 0)
                ModelState.AddModelError("SoLuongTon", "So luong ton phai lon hon 0");

        }

        // GET: /BangSanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangSanPham bangsanpham = db.BangSanPhams.Find(id);
            if (bangsanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.Loai_id = new SelectList(db.LoaiSanPhams, "id", "TenLoai", bangsanpham.Loai_id);
            return View(bangsanpham);
        }

        // POST: /BangSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,MaSP,TenSP,Loai_id,GiaBan,GiaGoc,GiaGop,SoLuongTon")] BangSanPham bangsanpham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bangsanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Loai_id = new SelectList(db.LoaiSanPhams, "id", "TenLoai", bangsanpham.Loai_id);
            return View(bangsanpham);
        }

        // GET: /BangSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BangSanPham bangsanpham = db.BangSanPhams.Find(id);
            if (bangsanpham == null)
            {
                return HttpNotFound();
            }
            return View(bangsanpham);
        }

        // POST: /BangSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BangSanPham bangsanpham = db.BangSanPhams.Find(id);
            db.BangSanPhams.Remove(bangsanpham);
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
