using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CurdwithImg.Models;

namespace CurdwithImg.Controllers
{
    public class StudentsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        // POST: Students/Create        
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,Name,Age,DateOfBirth,Genders,Image,File")] Student student)
        {
            if (ModelState.IsValid)
            {
                string filename = Path.GetFileName(student.File.FileName);
                string file = DateTime.Now.ToString("hhmmssfff") + filename;
                string path = Path.Combine(Server.MapPath("~/Content/Images/")) + file;
                student.Image = "~/Content/Images/" + file;

                db.Students.Add(student);
                if(student.File.ContentLength <= 1000000) { 
                    if (db.SaveChanges() > 0)
                    {
                        student.File.SaveAs(path);
                    }
                    return RedirectToAction("Index");
                }
                
                else
                {
                    ViewBag.Msg = "Image size must be less than or equal to 1MB";
                }
                
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            Session["ImgPath"] = student.Image;
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,Name,Age,DateOfBirth,Genders,Image,File")] Student student)
        {
            if (ModelState.IsValid)
            {
                if(student.File != null)
                {
                    string filename = Path.GetFileName(student.File.FileName);
                    string file = DateTime.Now.ToString("hhmmssfff") + filename;
                    string path = Path.Combine(Server.MapPath("~/Content/Images/")) + file;
                    student.Image = "~/Content/Images/" + file;

                    db.Entry(student).State = EntityState.Modified;
                    string oldImg = Request.MapPath(Session["imgPath"].ToString());
                    if (student.File.ContentLength <= 1000000)
                    {
                        if (db.SaveChanges() > 0)
                        {
                            student.File.SaveAs(path);
                            if (System.IO.File.Exists(oldImg))
                            {
                                System.IO.File.Delete(oldImg);
                            }
                        }
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ViewBag.Msg = "Image size must be less than or equal to 1MB";
                    }
                }
                else
                {
                    student.Image = Session["ImgPath"].ToString();
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            string CurrentImg = Request.MapPath(student.Image);
            db.Students.Remove(student);

            if(db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(CurrentImg))
                {
                    System.IO.File.Delete(CurrentImg);
                }
            }
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
