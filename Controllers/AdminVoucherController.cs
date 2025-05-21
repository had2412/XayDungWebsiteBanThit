using BanSach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanSach.Controllers
{
    public class AdminVoucherController : Controller
    {
        private QuanLyThitEntities db = new QuanLyThitEntities();
        // GET: AdminVoucher
        public ActionResult Index()
        {
            return View(db.Vouchers.ToList());
        }

        // GET: AdminVoucher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminVoucher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Vouchers.Add(voucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(voucher);
        }

        // GET: AdminVoucher/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
                return HttpNotFound();

            return View(voucher);
        }

        // POST: AdminVoucher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voucher).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(voucher);
        }

        // GET: AdminVoucher/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
                return HttpNotFound();

            return View(voucher);
        }

        // POST: AdminVoucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Voucher voucher = db.Vouchers.Find(id);
            db.Vouchers.Remove(voucher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}