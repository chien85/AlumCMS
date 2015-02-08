
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLib.Entities;
using AlumUI.BaseUI.Entities;
namespace AlumUI.Models
{
    public class ArticlesForHomeControllerModel
    {
        public IEnumerable<Content> articles { get; set; }
        public string Intro { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}