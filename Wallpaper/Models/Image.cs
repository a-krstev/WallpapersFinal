using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wallpaper.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string ImageEncoding { get; set; }
        public string ImageExtension { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Size
        {
            get { return Width.ToString() + "x" + Height.ToString(); }
        }

        [Required]
        [ForeignKey("CarBrand")]
        [Display(Name ="Brand")]
        public string BrandId { get; set; }
        public virtual CarBrand CarBrand { get; set; }

        [Required]
        [Display(Name ="Car name")]
        public string CarName { get; set; }

        [ForeignKey("BodyType")]
        [Display(Name ="Body Type")]
        public string BodyTypeId { get; set; }
        public virtual BodyType BodyType { get; set; }


        [ForeignKey("Color")]
        [Display(Name = "Color")]
        public string ColorId { get; set; }
        public virtual Color Color { get; set; }

        [Required]
        [ForeignKey("Year")]
        [Display(Name = "Year")]
        [RegularExpression(@"^\d{2,4}$")]
        public string YearId { get; set; }
        public virtual Year Year { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<UserAction> TakenActions { get; set; }
        public virtual ICollection<Similar> SimilarImages { get; set; }

        public Image()
        {
            Albums = new List<Album>();
            TakenActions = new List<UserAction>();
            SimilarImages = new List<Similar>();
        }

        public int SimilarityScore(Image image)
        {
            int score = 0;

            if (image.BrandId == BrandId)
            {
                score += 25;
            }
            if (BodyTypeId != null && image.BodyTypeId != null && image.BodyTypeId == BodyTypeId)
            {
                score += 25;
            }
            if (ColorId != null && image.ColorId != null && image.ColorId == ColorId)
            {
                score += 25;
            }
            int yearsDifference = Math.Abs(Convert.ToInt32(image.YearId) - Convert.ToInt32(YearId));
            if (yearsDifference <= 5)
            {
                score += 25;
            }
            else if(yearsDifference > 5 && yearsDifference < 10)
            {
                score += 10;
            }

            return score;
        }

        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1.AddDays(-1)));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2.AddDays(-1)));
            return d1 == d2;
        }

        //will see if this should be deleted
        public int NumberOfLikes(string timeFrame, DateTime time)
        {
            int likes = 0;
            foreach(UserAction ua in TakenActions)
            {
                if(ua.Action == ActionType.Like)
                {
                    if(String.IsNullOrEmpty(timeFrame) || timeFrame == "day")
                    {
                        if(ua.Time.Year == time.Year && ua.Time.Month == time.Month && ua.Time.Day == time.Day)
                        {
                            likes++;
                        }
                    }
                    else if(timeFrame == "week")
                    {
                        if(DatesAreInTheSameWeek(ua.Time, time))
                        {
                            likes++;
                        }
                    }
                    else if(timeFrame == "month")
                    {
                        if(ua.Time.Year == time.Year && ua.Time.Month == time.Month)
                        {
                            likes++;
                        }
                    }
                    else //ever
                    {
                        likes++;
                    }
                }
            }
            return likes;
        }
    }
}