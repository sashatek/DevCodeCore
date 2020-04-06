using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevCodeCore.Model;
using DevCodeCore.Models;
using DevCodeCore.Pages;
using DevCodeCore.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

            var set = new RestCrudSet();
            snippets = set.getCode(genMode, defs);
        }
    }
}