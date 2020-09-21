using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class CarBrand
    {
        [Key]
        public string Name { get; set; }
        public virtual ICollection<Image> Images { get; set; }

        public CarBrand()
        {
            Images = new List<Image>();
        }
    }
}