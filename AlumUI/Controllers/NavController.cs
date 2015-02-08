using AlumUI.BaseUI.Entities;
using DomainLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumUI.Controllers
{
    public class NavController : Controller
    {
        public IRepo myrepo;

        public NavController(IRepo repo)
        {
            this.myrepo = repo;
        }
        //
        // GET: /Nav/
        //list all menu
        public PartialViewResult Menu()
        {
            //replace by repo by another repo. IE categories

            MenuTree myparent = new MenuTree(myrepo);




            return PartialView("FlexMenu", myparent);
        }
        public PartialViewResult BottomMenu()
        {
            //replace by repo by another repo. IE categories

            MenuTree myparent = new MenuTree(myrepo);




            return PartialView("BottomFlexMenu", myparent);
        }
    }
}