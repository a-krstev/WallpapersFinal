using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public enum ActionType
    {
        View = 1, Like, Share, Download
    }
    public class UserAction
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public ActionType Action { get; set; }
        public virtual MyUser MyUser { get; set; }
        public virtual Image Image { get; set; }
    }
}