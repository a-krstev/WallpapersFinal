using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wallpaper.Models;
using Wallpaper.ViewModels;
using PagedList;
using Microsoft.Ajax.Utilities;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;

namespace Wallpaper.Controllers
{
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Images
        public ActionResult Index(string Brand, string Color, string Year, string BodyType, 
            string sortOrder, string currentFilter, string searchString, int? page,
            string currentBrand, string currentColor, string currentYear, string currentBodyType)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.BrandSortParm = String.IsNullOrEmpty(sortOrder) ? "brand_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.YearSortParm = sortOrder == "Year" ? "year_desc" : "Year";
            ViewBag.ColorSortParm = sortOrder == "Color" ? "color_desc" : "Color";
            ViewBag.BodyTypeSortParm = sortOrder == "BodyType" ? "bodytype_desc" : "BodyType";

            ViewBag.Brands = db.CarBrands.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.Years = db.Years.ToList();
            ViewBag.BodyTypes = db.BodyTypes.ToList();
            //ViewBag.UserId = CurrentUser().ID;

            if (Brand != null || Color != null || Year != null || BodyType != null)
            {
                page = 1;
            }
            else
            {
                Brand = currentBrand;
                Color = currentColor;
                Year = currentYear;
                BodyType = currentBodyType;
            }
            ViewBag.CurrentBrand = Brand;
            ViewBag.CurrentColor = Color;
            ViewBag.CurrentYear = Year;
            ViewBag.CurrentBodyType = BodyType;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var Images = from i in db.Images select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                Images = Images.Where(i => i.BrandId.Contains(searchString)
                                        || i.CarName.Contains(searchString)
                                        || i.ColorId.Contains(searchString)
                                        || i.YearId.Contains(searchString)
                                        || i.BodyTypeId.Contains(searchString));
            }
            else if (!String.IsNullOrEmpty(Brand) || !String.IsNullOrEmpty(Year) 
                || !String.IsNullOrEmpty(Color) || !String.IsNullOrEmpty(BodyType))
            {
                Images = Images
                    .Where(i => String.IsNullOrEmpty(Brand) || i.BrandId == Brand)
                    .Where(i => String.IsNullOrEmpty(Year) || i.YearId == Year)
                    .Where(i => String.IsNullOrEmpty(Color) || i.ColorId == Color)
                    .Where(i => String.IsNullOrEmpty(BodyType) || i.BodyTypeId == BodyType);
            }


            switch (sortOrder)
            {
                case "brand_desc":
                    Images = Images.OrderByDescending(i => i.BrandId);
                    break;
                case "Name":
                    Images = Images.OrderBy(i => i.CarName);
                    break;
                case "name_desc":
                    Images = Images.OrderByDescending(i => i.CarName);
                    break;
                case "Year":
                    Images = Images.OrderBy(i => i.YearId);
                    break;
                case "year_desc":
                    Images = Images.OrderByDescending(i => i.YearId);
                    break;
                case "Color":
                    Images = Images.OrderBy(i => i.ColorId);
                    break;
                case "color_desc":
                    Images = Images.OrderByDescending(i => i.ColorId);
                    break;
                case "BodyType":
                    Images = Images.OrderBy(i => i.BodyTypeId);
                    break;
                case "bodytype_desc":
                    Images = Images.OrderByDescending(i => i.BodyTypeId);
                    break;
                default:
                    Images = Images.OrderBy(i => i.BrandId);
                    break;
            }

            ViewBag.UserId = -1;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserId = CurrentUser().ID;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var model = Images.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Images", model);
            }

            return View(model);
        }

        private MyUser CurrentUser()
        {
            UserAction view = new UserAction();
            var userId = User.Identity.GetUserId();
            return db.MyUsers.Single(mu => mu.IdentityID == userId);
        }

        private void RegisterAction(Image image, ActionType actionType)
        {
            var myUser = CurrentUser();
            UserAction viewAction = new UserAction();
            viewAction.MyUser = myUser;
            viewAction.Image = image;
            viewAction.Time = DateTime.Now;
            viewAction.Action = actionType;
            db.UserActions.Add(viewAction);
            db.SaveChanges();
        }

        // GET: Images/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = CurrentUser().ID;
            RegisterAction(image, ActionType.View);
            return View(image);
        }

        private void ToggleMyAction(int id, Image image, ActionType actionType)
        {
            MyUser myUser = CurrentUser();
            var userActions = myUser.Actions.Where(a => a.Image.ID == id && a.Action == actionType);
            if (userActions.Count() == 0)
            {
                RegisterAction(image, actionType);
            }
            else
            {
                db.UserActions.Remove(userActions.First());
                db.SaveChanges();
            }
        }

        // GET: Images/Like/5
        [Authorize]
        public ActionResult Like(int? id, string TimeFrame)
        {
            DateTime time = DateTime.Now;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ToggleMyAction(id.Value, image, ActionType.Like);

            int numOfLikes = db.UserActions
                               .ToList()
                               .Where(ua => ua.Image.ID == id &&
                                      ua.Action == ActionType.Like &&
                                      CheckDate(TimeFrame, ua.Time, time))
                               .Count();

            return Content(numOfLikes.ToString());
        }

        // GET: Images/Share/5
        [Authorize]
        public ActionResult Share(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ToggleMyAction(id.Value, image, ActionType.Share);
            return new EmptyResult();
        }

        // GET: Images/Download/5
        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                RegisterAction(image, ActionType.Download);
            }

            byte[] thePictureAsBytes = Convert.FromBase64String(image.ImageEncoding);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = image.BrandId + " " + image.CarName + "." + image.ImageExtension,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(thePictureAsBytes, image.ImageExtension);
        }

        // GET: Images/Create
        [Authorize(Roles ="Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        private void ImageToBase64(HttpPostedFileBase imageFile, out byte[] thePictureAsBytes, out string thePictureDataAsString)
        {
            thePictureAsBytes = new byte[imageFile.ContentLength];
            using (BinaryReader theReader = new BinaryReader(imageFile.InputStream))
            {
                thePictureAsBytes = theReader.ReadBytes(imageFile.ContentLength);
            }
            thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ImageInputViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    string imgExtension = Path.GetFileName(model.ImageFile.FileName).Split('.')[1];
                    byte[] thePictureAsBytes;
                    string thePictureDataAsString;
                    ImageToBase64(model.ImageFile, out thePictureAsBytes, out thePictureDataAsString);

                    model.Image.ImageEncoding = thePictureDataAsString;
                    using (Stream memStream = new MemoryStream(thePictureAsBytes))
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(memStream))
                        {
                            model.Image.Width = img.Width;
                            model.Image.Height = img.Height;
                        }
                    }
                    if(db.CarBrands.Find(model.Image.BrandId) == null)
                    {
                        CarBrand cb = new CarBrand();
                        cb.Name = model.Image.BrandId;
                        db.CarBrands.Add(cb);
                        db.SaveChanges();
                    }
                    if (model.Image.ColorId != null && db.Colors.Find(model.Image.ColorId) == null)
                    {
                        Color c = new Color();
                        c.Val = model.Image.ColorId;
                        db.Colors.Add(c);
                        db.SaveChanges();
                    }
                    if (model.Image.YearId != null && db.Years.Find(model.Image.YearId) == null)
                    {
                        Year y = new Year();
                        y.Val = model.Image.YearId;
                        db.Years.Add(y);
                        db.SaveChanges();
                    }
                    if (model.Image.BodyTypeId != null && db.BodyTypes.Find(model.Image.BodyTypeId) == null)
                    {
                        BodyType bt = new BodyType();
                        bt.Val = model.Image.BodyTypeId;
                        db.BodyTypes.Add(bt);
                        db.SaveChanges();
                    }
                    model.Image.ImageExtension = imgExtension;
                    db.Images.Add(model.Image);
                    db.SaveChanges();

                    //Add similar images in Similars table
                    var Images = db.Images.Where(i => i.ID != model.Image.ID);

                    foreach (Image image in Images)
                    {
                        var score = model.Image.SimilarityScore(image);
                        if (score != 0)
                        {
                            db.Similars.Add(new Similar
                            {
                                Image1Id = model.Image.ID,
                                Image2Id = image.ID,
                                Score = score
                            });
                            db.Similars.Add(new Similar
                            {
                                Image1Id = image.ID,
                                Image2Id = model.Image.ID,
                                Score = score
                            });
                        }
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Images/Edit/5
        [Authorize(Roles ="Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ImageInputViewModel model = new ImageInputViewModel();
            model.Image = image;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ImageInputViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = model.Image.ID;
                Image img = db.Images.Single(m => m.ID == id);
                if (model.ImageFile != null)
                {
                    string imgExtension = Path.GetFileName(model.ImageFile.FileName).Split('.')[1];

                    byte[] thePictureAsBytes;
                    string thePictureDataAsString;
                    ImageToBase64(model.ImageFile, out thePictureAsBytes, out thePictureDataAsString);

                    using (Stream memStream = new MemoryStream(thePictureAsBytes))
                    {
                        using (System.Drawing.Image tmp = System.Drawing.Image.FromStream(memStream))
                        {
                            img.Width = tmp.Width;
                            img.Height = tmp.Height;
                        }
                    }

                    img.ImageEncoding = thePictureDataAsString;
                    img.ImageExtension = imgExtension;
                }
                if (db.CarBrands.Find(model.Image.BrandId) == null)
                {
                    CarBrand cb = new CarBrand();
                    cb.Name = model.Image.BrandId;
                    db.CarBrands.Add(cb);
                    db.SaveChanges();
                }
                if (model.Image.ColorId != null && db.Colors.Find(model.Image.ColorId) == null)
                {
                    Color c = new Color();
                    c.Val = model.Image.ColorId;
                    db.Colors.Add(c);
                    db.SaveChanges();
                }
                if (model.Image.YearId != null && db.Years.Find(model.Image.YearId) == null)
                {
                    Year y = new Year();
                    y.Val = model.Image.YearId;
                    db.Years.Add(y);
                    db.SaveChanges();
                }
                if (model.Image.BodyTypeId != null && db.BodyTypes.Find(model.Image.BodyTypeId) == null)
                {
                    BodyType bt = new BodyType();
                    bt.Val = model.Image.BodyTypeId;
                    db.BodyTypes.Add(bt);
                    db.SaveChanges();
                }
                var checkBrand = img.CarBrand;
                var checkBodyType = img.BodyType;
                var checkColor = img.Color;
                var checkYear = img.Year;

                img.BrandId = model.Image.BrandId;
                img.CarName = model.Image.CarName;
                img.BodyTypeId = model.Image.BodyTypeId;
                img.ColorId = model.Image.ColorId;
                img.YearId = model.Image.YearId;
                db.Entry(img).State = EntityState.Modified;
                //TryUpdateModel(img);
                db.SaveChanges();

                //Add similar images in Similars table
                var Images = db.Images.Where(i => i.ID != img.ID).ToList();

                foreach (Image image in Images)
                {
                    var tmp1 = db.Similars.SingleOrDefault(s => s.Image1Id == img.ID && s.Image2Id == image.ID);
                    if (tmp1 != null)
                    {

                        var tmp2 = db.Similars.Single(s => s.Image1Id == image.ID && s.Image2Id == img.ID);
                        var newScore = tmp1.Image1.SimilarityScore(tmp1.Image2);

                        if (newScore == 0)
                        {
                            db.Similars.Remove(tmp1);
                            db.Similars.Remove(tmp2);
                        }
                        else
                        {
                            tmp1.Score = newScore;
                            tmp2.Score = newScore;
                            db.Entry(tmp1).State = EntityState.Modified;
                            db.Entry(tmp2).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        var score = img.SimilarityScore(image);
                        if (score != 0)
                        {
                            db.Similars.Add(new Similar
                            {
                                Image1Id = img.ID,
                                Image2Id = image.ID,
                                Score = score
                            });
                            db.Similars.Add(new Similar
                            {
                                Image1Id = image.ID,
                                Image2Id = img.ID,
                                Score = score
                            });
                            db.SaveChanges();
                        }
                    }
                }
                //db.SaveChanges();

                //in case the picture was the last of that kind of category
                if (checkBrand != null && checkBrand.Images.Count == 0)
                {
                    db.CarBrands.Remove(checkBrand);
                    db.SaveChanges();
                }
                if (checkBodyType != null && checkBodyType.Images.Count == 0)
                {
                    db.BodyTypes.Remove(checkBodyType);
                    db.SaveChanges();
                }
                if (checkColor != null && checkColor.Images.Count == 0)
                {
                    db.Colors.Remove(checkColor);
                    db.SaveChanges();
                }
                if (checkYear != null && checkYear.Images.Count == 0)
                {
                    db.Years.Remove(checkYear);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Images/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Image image = db.Images.Find(id);

            var checkBrand = image.CarBrand;
            var checkBodyType = image.BodyType;
            var checkColor = image.Color;
            var checkYear = image.Year;

            var actions = db.UserActions.Where(ua => ua.Image.ID == id);
            db.UserActions.RemoveRange(actions);

            var similarImages = db.Similars
                .Where(i => i.Image1Id == id || i.Image2Id == id);
            db.Similars.RemoveRange(similarImages);

            db.Images.Remove(image);
            db.SaveChanges();

            if (checkBrand != null && checkBrand.Images.Count == 0)
            {
                db.CarBrands.Remove(checkBrand);
                db.SaveChanges();
            }
            if (checkBodyType != null && checkBodyType.Images.Count == 0)
            {
                db.BodyTypes.Remove(checkBodyType);
                db.SaveChanges();
            }
            if (checkColor != null && checkColor.Images.Count == 0)
            {
                db.Colors.Remove(checkColor);
                db.SaveChanges();
            }
            if (checkYear != null && checkYear.Images.Count == 0)
            {
                db.Years.Remove(checkYear);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private bool DatesAreOnTheSameDay(DateTime date1, DateTime date2)
        {
            return date1.Day == date2.Day && date1.Month == date2.Month && date1.Year == date2.Year;
        }

        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1.AddDays(-1)));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2.AddDays(-1)));
            return d1 == d2;
        }

        private bool DatesAreInTheSameMonth(DateTime date1, DateTime date2)
        {
            return date1.Month == date2.Month && date1.Year == date2.Year;
        }

        private bool CheckDate(string TimeFrame, DateTime dateTimeOfAction, DateTime currentDateTime)
        {
            if(String.IsNullOrEmpty(TimeFrame) || TimeFrame == "day")
            {
                return DatesAreOnTheSameDay(dateTimeOfAction, currentDateTime);
            }
            else if (TimeFrame == "week")
            {
                return DatesAreInTheSameWeek(dateTimeOfAction, currentDateTime);
            }
            else if (TimeFrame == "month")
            {
                return DatesAreInTheSameMonth(dateTimeOfAction, currentDateTime);
            }
            return true;
        }

        private static readonly SelectList TrendingSelectList =
            new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "day", Value = "day" },
                new SelectListItem { Text = "week", Value = "week" },
                new SelectListItem { Text = "month", Value = "month" },
                new SelectListItem { Text = "ever", Value = "ever" }
            }, "Text", "Value");

        [Authorize]
        public ActionResult Trending(string TimeFrame, int? page)
        {
            DateTime time = DateTime.Now;

            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentTimeFrame = TimeFrame;
            ViewBag.selectList = TrendingSelectList;

            var temp = db.UserActions
                .AsEnumerable() //LINQ to Entities does not recognize the method CheckDate
                .Where(ua => ua.Action == ActionType.Like
                             && CheckDate(TimeFrame, ua.Time, time))
                .ToList() //Open DataReader error
                .GroupBy(ua => ua.Image.ID)
                .Select(group => new TrendingImageViewModel
                {
                    Image = group.Select(g => g.Image).First(),
                    NumberOfLikes = group.Count()
                })
                .OrderByDescending(i => i.NumberOfLikes);

            ViewBag.UserId = CurrentUser().ID;

            var listPaged = temp.ToPagedList(page ?? 1, 3); //change to 5 items per load
            ViewBag.PageCount = listPaged.PageCount;

            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_TrendingImages", listPaged);
            }
            return View(listPaged);
        }

        private double aggregateUserActions(IEnumerable<UserAction> actions)
        {
            int countViews = actions.Where(a => a.Action == ActionType.View).Count();
            double aggregateViews = ((countViews < 10 ? countViews : 10) / 10.0) * 25;
            int aggregateOtherActions = actions.Where(a => a.Action != ActionType.View).Count() * 25;
            return aggregateViews + aggregateOtherActions;
        }

        [Authorize]
        public ActionResult Recommended(int? page)
        {
            // return a 404 if user browses to before the first page
            if (page.HasValue && page < 1)
            {
                return HttpNotFound();
            }

            MyUser myUser = CurrentUser();
            var Images = myUser.Actions
                .GroupBy(ua => ua.Image.ID)
                .Select(group => new
                {
                    Image = group.Select(g => g.Image).First(),
                    Rating = aggregateUserActions(group),
                })
                .Select(a => new
                {
                    a.Image,
                    a.Rating,
                    SimilarImages = a.Image.SimilarImages
                        .Where(i => aggregateUserActions(i.Image2.TakenActions.Where(ta => ta.MyUser.ID == myUser.ID)) < 25) //arbitrary number < 10 views
                })
                .SelectMany(a => a.SimilarImages.Select(si => new
                {
                    si.Image2,
                    Score = a.Rating * 0.6 + si.Score * 0.4, //arbitrary weights
                }))
                .OrderByDescending(a => a.Score)
                .DistinctBy(a => a.Image2.ID)
                .Select(a => a.Image2)
                .ToList();

            var listPaged = Images.ToPagedList(page ?? 1, 4); //change to 5 items per load
            ViewBag.PageCount = listPaged.PageCount;

            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_TRImages", listPaged);
            }
            return View(listPaged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
