using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevCodeCore.Models;
using DevCodeCore.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DevCodeWeb.Pages
{
    public class IndexModel : PageModel
    {
        public Snippet[] snippets;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return RedirectToPage("DevCode");
        }
    }
}
