using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chirst_Temple_Kid_Finder.Models;
using System.Net;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Chirst_Temple_Kid_Finder.Controllers
{
    public class LocationController : Controller
    {
        private dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3 db = new dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3();
        // GET: Location
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult KidLocation()
        {
            //Sets cat = entry with the correct email
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var email = user.Email;
                CodeAssignTable cat = db.CodeAssignTables.First(x => x.Email == email);
                //Sets cc = the childcode of the specific entry we pulled above
                var cc = cat.ChildCode;
                //sets ct = entry with correct child code
                CodeTable ct = db.CodeTables.First(x => x.ChildCode == cc);
                //sets room = the room number of the specific child code we pulled above
                var room = ct.Room_Number;
                //Stores room in ViewData
                if(room == null)
                {
                    ViewData["RoomNumber"] = "Not Checked In";
                }
                else
                {
                    ViewData["RoomNumber"] = room;
                }
            } catch (InvalidOperationException e) { }
            return View(ViewData);
        }

        


    }
}