using CastilloLawnCare.Models;
using Microsoft.AspNetCore.Mvc;
using CastilloLawnCare.Models.DataModels;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using CastilloLawnCare.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CastilloLawnCare.Controllers
{
    //*************IMPORTANT*********************
    //TODO - REMEMBER!!!! : GET FUNCTIONALITY OF THINGS WORKING AND THEN SET WHAT CAN AND CANT BE SEEN IF A USER ISNT REGISTERED!

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Data.CastilloLawnCareDBData _castilloLawnCareDBData;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _castilloLawnCareDBData = new Data.CastilloLawnCareDBData();
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<vmCustomerReview> vmCustomerReviews = new List<vmCustomerReview>();
            var users = _userManager.Users.ToList();
            var dmCustomerReviewList = _castilloLawnCareDBData.GetReviews();
            if (dmCustomerReviewList.Count != 0)
            {
                foreach (var review in dmCustomerReviewList)
                {
                    vmCustomerReview vmCustomerReview = ParseDMReviewToVMReview(review);
                    vmCustomerReviews.Add(vmCustomerReview);
                }
                vmCustomerReviews = (from x in vmCustomerReviews
                                     join y in users on x.Id equals y.Id into gj
                                     from j in gj.DefaultIfEmpty()
                                     where x.Id == j.Id
                                     select new vmCustomerReview()
                                     {
                                         ReviewID = x.ReviewID,
                                         Description = x.Description,
                                         ReviewDate = x.ReviewDate,
                                         Rating = x.Rating,
                                         FirstName = j.FirstName,
                                         LastName = j.LastName
                                     }).ToList();
            }

            return View(vmCustomerReviews);
        }



        [HttpPost]
        public IActionResult PostReview(int? rating, string review)
        {
            var currentUserID = (HttpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (IsPresent(rating.ToString()) && IsPresent(review))
            {
                var dmCustomerReview = new dmCustomerReview()
                {
                    Description = review,
                    UserID = currentUserID,
                    ReviewDate = DateTime.Now,
                    Rating = (int)rating
                };

                _castilloLawnCareDBData.AddReview(dmCustomerReview);
            }

            return Redirect("Index");
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Appointment()
        {
            return View();
        }

        public JsonResult LoadAppointments()
        {
            List<vmAppointment> vmAppointmentList = new List<vmAppointment>();
            var currentUserID = (HttpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var dmAppointmentList = _castilloLawnCareDBData.GetAppointmentsByUserID(currentUserID);
            var newDmAppointment = dmAppointmentList.Select(x => new dmAppointment() { AppointmentTypeID = x.AppointmentTypeID, ServiceID = x.ServiceID} ).FirstOrDefault();
            var AppointmentType = _castilloLawnCareDBData.GetAppointmentTypeByID(newDmAppointment.AppointmentTypeID);
            var serviceType = _castilloLawnCareDBData.GetServiceTypeByID(newDmAppointment.ServiceID);

            foreach(var item in dmAppointmentList)
            {
                var vmAppointment = ParseDMAppointToVMAppointWithoutServiceAndApptType(item);
                vmAppointment.AppointmentType = AppointmentType;
                vmAppointment.ServiceType = serviceType;
                vmAppointmentList.Add(vmAppointment);
            }
            //vmAppointment vmAppointment = new vmAppointment()
            //{
            //    AppointmentDate = DateTime.Now.AddHours(6),
            //    AppointmentTypeID = 1,
            //};
            var obj = Newtonsoft.Json.JsonConvert.SerializeObject(vmAppointmentList);
            return Json(obj);
        }

        public IActionResult Services()
        {
            return View();
        }


        public async Task<IActionResult> Apply()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var dmServiceList = _castilloLawnCareDBData.GetServices();
            
            vmAppointment appointment = null;
            if (currentUser != null)
            {
                try
                {
                    appointment = new vmAppointment()
                    {
                        Id = currentUser.Id,
                        FirstName = currentUser.FirstName,
                        LastName = currentUser.LastName,
                        Address = currentUser.Address,
                        City = currentUser.City,
                        State = currentUser.State,
                        Email = currentUser.Email,
                        PhoneNumber = currentUser.PhoneNumber,
                    };
                    foreach (var d in dmServiceList)
                    {
                        appointment.ServiceList.Add(new SelectListItem() { Text = d.ServiceType, Value = d.ServiceID.ToString() });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Success = false;
                    ViewBag.Error = "Error has occured: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Success = false;
                ViewBag.Error = "Must be logged in or registered to view";
            }
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(vmAppointment vmAppointment)
        {
            var existingUser = await _userManager.GetUserAsync(User);
            if (existingUser != null)
            {
                var updatedUser = ParseToNewUser(vmAppointment);
                if (CheckIfUpdating(ref existingUser, updatedUser))
                {
                    var result = await _userManager.UpdateAsync(existingUser);
                    if (!result.Succeeded)
                    {
                        ViewBag.Success = result.Succeeded;
                        ViewBag.ErrorList = result.Errors.Select(x => x.Description).ToList();
                        return View(vmAppointment);
                    }
                }
                dmAppointment dmAppointment = ParseVMAppointToDMAppoint(vmAppointment);
                if (dmAppointment != null)
                {
                    _castilloLawnCareDBData.AddAppointment(dmAppointment);
                    ViewBag.Success = true;
                }
                else { ViewBag.Success = false; ViewBag.Error = "Error with saving your Appointment. Try Again later."; }
            }
            return View(vmAppointment);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Parse
        private vmCustomerReview ParseDMReviewToVMReview(dmCustomerReview review)
        {
            try
            {
                vmCustomerReview vmCustomerReview = new vmCustomerReview()
                {
                    ReviewID = review.ReviewID,
                    Description = review.Description,
                    Id = review.UserID,
                    ReviewDate = review.ReviewDate,
                    Rating = review.Rating,
                };
                return vmCustomerReview;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private User ParseToNewUser(vmAppointment vmAppointment)
        {
            User updatedUser = new User
            {
                FirstName = vmAppointment.FirstName,
                LastName = vmAppointment.LastName,
                Address = vmAppointment.Address,
                City = vmAppointment.City,
                State = vmAppointment.State,
                Email = vmAppointment.Email,
                PhoneNumber = vmAppointment.PhoneNumber
            };
            return updatedUser;
        }

        private dmAppointment ParseVMAppointToDMAppoint(vmAppointment vmAppointment)
        {
            try
            {
                dmAppointment dmAppointment = new dmAppointment()
                {
                    AppointmentDate = vmAppointment.AppointmentDate,
                    AppointmentTypeID = vmAppointment.AppointmentTypeID,
                    ServiceID = vmAppointment.ServiceID,
                    UserID = vmAppointment.Id
                };
                return dmAppointment;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private vmAppointment ParseDMAppointToVMAppointWithoutServiceAndApptType(dmAppointment dmAppointment)
        {
            try
            {
                vmAppointment vmAppointment = new vmAppointment()
                {
                    AppointmentID = dmAppointment.AppointmentID,
                    AppointmentDate = dmAppointment.AppointmentDate,
                    AppointmentTypeID = dmAppointment.AppointmentTypeID,
                    ServiceID = dmAppointment.ServiceID,
                    Id = dmAppointment.UserID
                };
                return vmAppointment;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Validation

        private bool CheckIfUpdating(ref User existingUser, User updatedUser)
        {
            int count = 0;
            if (existingUser.FirstName != updatedUser.FirstName)
            {
                existingUser.FirstName = updatedUser.FirstName;
                count++;
            }
            if (existingUser.LastName != updatedUser.LastName)
            {
                existingUser.LastName = updatedUser.LastName;
                count++;
            }
            if (existingUser.Address != updatedUser.Address)
            {
                existingUser.Address = updatedUser.Address;
                count++;
            }
            if (existingUser.City != updatedUser.City)
            {
                existingUser.City = updatedUser.City;
                count++;
            }
            if (existingUser.State != updatedUser.State)
            {
                existingUser.State = updatedUser.State;
                count++;
            }
            if (existingUser.Email != updatedUser.Email)
            {
                existingUser.Email = updatedUser.Email;
                existingUser.UserName = updatedUser.Email;
                count++;
            }
            if (existingUser.PhoneNumber != updatedUser.PhoneNumber)
            {
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                count++;
            }
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        private bool IsPresent(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            else
                return true;
        }

        #endregion

    }
}