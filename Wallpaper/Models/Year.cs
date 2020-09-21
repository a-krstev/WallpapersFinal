using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class Year
    {
        [Key]
        [RegularExpression(@"^\d{2,4}$")]
        public string Val { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public Year()
        {
            Images = new List<Image>();
        }
    }
}