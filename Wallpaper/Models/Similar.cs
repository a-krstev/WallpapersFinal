using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class Similar
    {
        [Key]
        [Column(Order =1)]
        public int Image1Id { get; set; }
        public virtual Image Image1 { get; set; }

        [Key]
        [Column(Order = 2)]
        public int Image2Id { get; set; }
        public virtual Image Image2 { get; set; }

        public int Score { get; set; }
    }
}