using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class MyUser
    {
        public int ID { get; set; }

        [Required]
        public string UserName { get; set; }

        [StringLength(128)]
        public string IdentityID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<UserAction> Actions { get; set; }

        public MyUser()
        {
            Actions = new List<UserAction>();
        }
    }
}