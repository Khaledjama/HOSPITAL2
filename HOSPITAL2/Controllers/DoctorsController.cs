using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HOSPITAL2.Models;

namespace HOSPITAL2.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorsController : Controller
    {
        private HospitalEntities10 db = new HospitalEntities10();

        // GET: Doctors
        public ActionResult Index()
        {
            var doctors = db.Doctor.Include(d => d.Department);
            return View(doctors.ToList());
        }
        //this action for view time of doctors in hospital 
        [Authorize]
        public ActionResult Index2()
        {
            var doctors = db.Doctor;
            return View(doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            ViewBag.Id_dep = new SelectList(db.Department, "Id", "Name");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Adress,telephone,Id_dep,Id,EndAt,startat,number,serial_Num,num_reveal,num_Re_revealed")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                Random R = new Random();
                doctor.serial_num = R.Next().ToString();
                db.Doctor.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_dep = new SelectList(db.Department, "Id", "Name", doctor.Id_dep);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            Session["ID"] = doctor.serial_num;
            ViewBag.Id_dep = new SelectList(db.Department, "Id", "Name", doctor.Id_dep);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Adress,telephone,Id_dep,Id,EndAt,startat,number,serial_Num,num_reveal,num_Re_revealed")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.serial_num = Session["ID"].ToString() ;
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_dep = new SelectList(db.Department, "Id", "Name", doctor.Id_dep);
            return View();
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctor.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctor.Find(id);
            db.Doctor.Remove(doctor);
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
