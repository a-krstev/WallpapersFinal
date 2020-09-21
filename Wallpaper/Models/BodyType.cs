using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class BodyType
    {
        [Key]
        public string Val { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public BodyType()
        {
            Images = new List<Image>();
        }
    }
}