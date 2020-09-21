using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wallpaper.Models;

namespace Wallpaper.ViewModels
{
    public class ImageInputViewModel
    {
        public Image Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}