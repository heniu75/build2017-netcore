using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace build2017.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
            Message = "Hello this is the message from IndexModel()";
        }
        public string Message { get; set; }
        public void OnGet()
        {

        }
    }
}