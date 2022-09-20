using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HOSPITAL2.Models;
using Microsoft.Ajax.Utilities;

namespace HOSPITAL2.Controllers
{
    public class data_PatientController : Controller
    {
        private HospitalEntities10 db = new HospitalEntities10();

        // GET: data_Patient
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(int? ser)
        {
            deletebook();
            var element = db.data_Patient.Where(s => s.Serial_Num == ser.ToString()).ToList();
            if (element.Count() > 0)
            {
                ViewBag.ss = "l;,dfl";
            }
            if(element.Count() ==0)
            {
                ViewBag.m = "sdds";
            }
            return View(element);
        }
        //to delet the book after 5 days from the time of book
        private void deletebook()
        {
            //book 23/10/2020 + 5days = 28/10/2020
            //now 7/11/2020
            DateTime time = DateTime.Now;
            foreach (var item in db.data_Patient.ToList())
            {
                if (item.date_time.Value.Date.AddDays(5) < time.Date)
                {
                    int primarykey = item.Id;
                    data_Patient data_patient = db.data_Patient.Find(primarykey);
                    db.data_Patient.Remove(data_patient);
                    db.SaveChanges();
                }
            }
        }

        // GET: data_Patient/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            data_Patient data_Patient = db.data_Patient.Find(id);
            if (data_Patient == null)
            {
                return HttpNotFound();
            }
            return View(data_Patient);
        }
        // GET: data_Patient/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            data_Patient data_Patient = db.data_Patient.Find(id);
            if (data_Patient == null)
            {
                return HttpNotFound();
            }
            return View(data_Patient);
        }

        // POST: data_Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            data_Patient data_Patient = db.data_Patient.Find(id);
            data_Patient.statues = true;
            db.SaveChanges();
            return RedirectToAction("Index","Home");
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
