using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevCodeCore.Models;
using DevCodeCore.Pages;
using DevCodeCore.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevCodeWeb
{
    public class DevCodeModel : PageModel
    {
        public Snippet[] snippets;
        [BindProperty]
        public string scrText { get; set; }
        [BindProperty]
        public GenMode genMode { get; set; }
        public bool upperCase { get; set; }
        [BindProperty]
        public int controlId { get; set; }
        public List<SelectListItem> controls { get; set; }
        public void OnGet()
        {
            var set = new RestCrudSet();
            scrText = set.getAsText();
        }

        public void OnPost()
        {
            var parser = new SqlDesignParser();
            var defs = new EntityModel();
            parser.parse(scrText, defs);
            controls = defs.controls.Select((c, i)=>new SelectListItem
            {
                Value = i.ToString(),
                Text  = c.controlName
            }).ToList();
            defs.control = defs.controls.Count > 0 ? defs.controls[controlId] : null;
            var set = new RestCrudSet();
            snippets = set.getCode(genMode, defs);
            scrText = "";
        }
    }
}