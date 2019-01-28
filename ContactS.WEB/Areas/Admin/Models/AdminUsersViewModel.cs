using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactS.WEB.Areas.Admin.Models
{
    public class AdminUsersViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}