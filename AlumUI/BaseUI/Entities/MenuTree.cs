using DomainLib;
using DomainLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumUI.BaseUI.Entities
{
    
        public class MenuTree
        {
            public IRepo myrepo;

            private IEnumerable<Subcategory> sub;


            public IEnumerable<Category> AllCategory;

            public Dictionary<Category, IEnumerable<Subcategory>> tree = new Dictionary<Category, IEnumerable<Subcategory>>();


            public MenuTree(IRepo myrepopara)
            {

                myrepo = myrepopara;

                AllCategory = myrepo.Categories;

                foreach (var cate in AllCategory)
                {
                    sub = GetIDSpecifiedCategories(cate.Idcategory);
                    tree.Add(cate, sub);


                }


            }

            public IEnumerable<Subcategory> GetIDSpecifiedCategories(int categorypara)
            {
                int?[] idsub = myrepo.CatSubCats.Where(x => x.Idcategory == categorypara).Select(x => x.IdSubcategory).ToArray();


                IEnumerable<Subcategory> mysubcate = myrepo.Subcategories.Where(x => idsub.Contains(x.IdSubcategory));

                //subcategoryname = mysubcate.Select(x => x.Subcatename).ToList();
                return mysubcate;
            }
        }
    
}