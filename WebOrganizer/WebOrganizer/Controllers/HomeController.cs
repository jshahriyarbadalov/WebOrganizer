using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOrganizer.Models;

namespace WebOrganizer.Controllers
{
    public class HomeController : Controller
    {
        private object status;

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            using ( MyDataEntities dc = new MyDataEntities()) {
                var events = dc.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            using (MyDataEntities dc = new MyDataEntities()) {

                if (e.EventID > 0)
                {
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                    else
                    {
                        dc.Events.Add(e);
                    }
                    dc.SaveChanges();
                    status = true;
                }
                else
                {
                    dc.Events.Add(e);
                    dc.SaveChanges();
                }
            }

            return new JsonResult { Data = new { status = status, e} };
        }

        [HttpPost]

        public JsonResult DeleteEvent(int eventID) {

            using (MyDataEntities dc = new MyDataEntities()) {
                var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status} };
        }
    }
}