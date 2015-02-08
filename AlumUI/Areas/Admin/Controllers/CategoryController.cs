using DomainLib;
using DomainLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlumUI.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
    
            IRepo myrepo;
            public CategoryController(IRepo repopara)
            {
                myrepo = repopara;
            }
            //
            // GET: /Admin/Category/
            public ActionResult Index()
            {
                return View(myrepo.Categories);

            }

            public ActionResult Edit(int idcategory)
            {
                var cate = myrepo.Categories.Where(x => x.Idcategory == idcategory).FirstOrDefault();


                return View(cate);
            }
            [HttpPost]
            public ActionResult Edit(Category catepara)
            {
                if (ModelState.IsValid)
                {
                    myrepo.SaveCategory(catepara);
                    return RedirectToAction("Index", "Category");

                }
                else
                    return View(catepara);

            }
            public ActionResult Delete(int idcategory)
            {
                myrepo.DeleteCategory(idcategory);
                return RedirectToAction("Index");

            }
            public IEnumerable<Category> GetCategory()
            {

                return myrepo.Categories;

            }
            public IEnumerable<Subcategory> GetSubcategory()
            {
                return myrepo.Subcategories;
            }
            public IEnumerable<CatSubCat> GetCatSubCat()
            {
                return myrepo.CatSubCats;
            }
            public ActionResult Manage(int idcategory)
            {
                int?[] idsub = myrepo.CatSubCats.Where(x => x.Idcategory == idcategory).Select(x => x.IdSubcategory).ToArray();


                IEnumerable<Subcategory> mysubcate = myrepo.Subcategories.Where(x => idsub.Contains(x.IdSubcategory));

                //subcategoryname = mysubcate.Select(x => x.Subcatename).ToList();

                Session["idcategory"] = idcategory;


                return View(mysubcate);
            }

            public ActionResult EditSub(int idsubcategory)
            {
                var cate = myrepo.Subcategories.Where(x => x.IdSubcategory == idsubcategory).FirstOrDefault();

                SelectList listcategory = new SelectList(myrepo.Categories, "Idcategory", "Catename", Convert.ToInt32(Session["idcategory"]));
                ViewBag.list = listcategory;

                return View(cate);

            }
            [HttpPost]
            public ActionResult EditSub(Subcategory subpara, int idcategory)
            {
                if (ModelState.IsValid)
                {
                    myrepo.SaveSubcategory(subpara, idcategory);
                    return RedirectToAction("Index", "Category");
                }
                else
                    return View(subpara);

            }
            //create 
            public ActionResult Create()
            {
                return View("Edit", new Category());
            }
            public ActionResult CreateSub()
            {
                SelectList listcategory = new SelectList(myrepo.Categories, "Idcategory", "Catename", 1);
                ViewBag.list = listcategory;
                return View("EditSub", new Subcategory());
            }

        }
    }
