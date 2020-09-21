using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wallpaper.Models;

namespace Wallpaper.ViewModels
{
    public class TrendingImageViewModel
    {
        public Image Image { get; set; }
        public int NumberOfLikes { get; set; }
    }
}