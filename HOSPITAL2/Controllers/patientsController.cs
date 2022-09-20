using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HOSPITAL2.Models;
using Microsoft.AspNet.Identity;

namespace HOSPITAL2.Controllers
{
    public class patientsController : Controller
    {
        private HospitalEntities10 db = new HospitalEntities10();

        [Authorize(Roles = "Screter Doc")]
        // GET: patients
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string text1)
        {
            //ViewBag.Doctors = new SelectList(db.Doctors, "Id", "Name");
            var use = db.patient.Include(p=>p.Doctor).Where(s => s.Doctor.serial_num == text1).ToList();
            ViewBag.ss = "khaled";
            return View(use);
        }

        // GET: patients/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patient patient = db.patient.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: patients/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.deplist = new SelectList(db.Department, "Id", "Name");
            ViewBag.Id_Doc = new SelectList(db.Doctor, "Id", "Name");
            return View();
        }
        // to make fillter doc by departments
        public JsonResult getdoc(int dep_id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Doctor> list = db.Doctor.Where(s => s.Id_dep == dep_id).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        // POST: patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Name,Adress,age,Id_Doc,Id,dat,number,Id_dep")] patient patient )
        {
            var RandomNumber = new Random().Next();
            var Userid = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                patient.dat = DateTime.Now;
                patient.User_Id = Userid;
                patient.Serial_Num = RandomNumber.ToString();
                patient.number = patient.PutNumberofPook(patient);
                db.patient.Add(patient);
                db.SaveChanges();
                TempData["mydata"] = patient;
                // add Revall for this patient 
                addpatient(patient.Serial_Num, DateTime.Now);
                return RedirectToAction("Gettime");
            }
            else
            {

                return RedirectToAction("Login", "Account");
            }
            ViewBag.Id_Doc = new SelectList(db.Doctor, "Id", "Name", patient.Id_Doc);
            ViewBag.deplist = new SelectList(db.Department, "Id", "Name",patient.Id_dep);
            return View();
        }
        // GET: patients/Edit/5
        //Method to add details of this patient to data_patient table for reavel book
        public void addpatient(string id,DateTime t)
        {
            data_Patient data = new data_Patient();
            data.Serial_Num = id;
            data.date_time = t;
            data.statues = false;
            db.data_Patient.Add(data);
            db.SaveChanges();
        }
        [Authorize]
        public ActionResult Edit(int ?id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patient patient = db.patient.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Doc = new SelectList(db.Doctor, "Id", "Name", patient.Id_Doc);
            return View(patient);
        }

        // POST: patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Name,Adress,age,Id_Doc,Id,dat")] patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Doc = new SelectList(db.Doctor, "Id", "Name", patient.Id_Doc);
            return View(patient);
        }

        // GET: patients/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patient patient = db.patient.Where(s=>s.Id==id).FirstOrDefault();
            if (patient == null)
            {
                return HttpNotFound();
            }
            TempData["id"] = id;
            return View(patient);
        }

        // POST: patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            patient patient = db.patient.Where(s=>s.Id==id).SingleOrDefault();
            TempData["ser"] = patient;
            Delete_serial_number();        // delete serial number and record in data_patient table
            db.patient.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        //action to now the time of the patient
        [Authorize]
        public ActionResult Gettime( )
        {
            var user = TempData["mydata"] as patient;
            return View(user);
        }
        // delete serial number and record in data_patient 
        private void Delete_serial_number()
        {
            var use = TempData["ser"] as patient;
            var row =db.data_Patient.Where(s => s.Serial_Num == use.Serial_Num).SingleOrDefault();
            db.data_Patient.Remove(row);
            db.SaveChanges();
        }

        //action for delet the book
        [HttpGet]
        [Authorize]
        public ActionResult deletebook()
        {

            return View();
            
        }
        [HttpPost]
        [Authorize]
        public ActionResult deletebook(string search)
        {

            if (search == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.patient.Where(s => s.Serial_Num == search).ToList();
            if (user == null)
            {
                return HttpNotFound();
            }
            else if(user.Count()>0)
            {
                ViewBag.ss = "lll";
                return View(user);
            }
            else
            {
                ViewBag.m = "lll";
                return View(user);
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
