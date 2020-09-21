using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class Album
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTime UploadDate { get; set; }
        public int NumberOfLikes { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public Album()
        {
            Images = new List<Image>();
        }
    }
}