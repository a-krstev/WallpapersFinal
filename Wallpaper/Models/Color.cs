using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class Color
    {
        [Key]
        public string Val { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public Color()
        {
            Images = new List<Image>();
        }
    }
}