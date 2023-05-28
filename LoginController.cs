using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Employee_Timesheet.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace Employee_Timesheet.Controllers
{
    public class LoginController : Controller
    {
        private readonly CompanyTimesheetContext _context;
        private AuthenticationProperties properties;

        public LoginController(CompanyTimesheetContext context)
        {
            _context = context;
        }

      
      
        public IActionResult EmployeeLogin()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index","Home");
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> EmployeeLogin(string username, string password)
        {
            if (IsValidEmployee(username, password))
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, username),
                };
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
 

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),properties);

                // Successful login, redirect to the home page or any other authenticated area
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ModelState.AddModelError("", "Invalid username or password");
                //ViewBag.ErrorMessage = "Invalid username or password";
                ViewData["ValidateMessage"] = "Invalid username or password";
                return View();
            }
        }


        //private bool IsValidEmployee(string username, string password)
        //{
        // var employee = _context.Employees
        //    .FirstOrDefault(e => e.EmployeeId == username && e.Password == password);

        //    return employee != null;
        //}
        private bool IsValidEmployee(string username, string password)
        {
            if (username.StartsWith("EMP"))
            {
                var employee = _context.Employees
                    .FirstOrDefault(e => e.EmployeeId == username && e.Password == password);

                return employee != null;
            }
            else if (username.StartsWith("MGR"))
            {
                var manager = _context.Managers
                    .FirstOrDefault(m => m.ManagerId == username && m.Password == password);

                return manager != null;
            }
            else
            {
                return false;
            }
        }


    }
}
