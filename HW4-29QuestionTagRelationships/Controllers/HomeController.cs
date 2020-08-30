using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HW4_29QuestionTagRelationships.Models;
using Microsoft.Extensions.Configuration;
using HW4_29QuestionTagRelationships.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HW4_29QuestionTagRelationships.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new QTRepository(_connectionString);
            return View(repo.GetQuestions());
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string email, string password)
        {
            var repo = new QTRepository(_connectionString);
            repo.AddUser(email, password);
            var claims = new List<Claim>               //when sign up, this will also login the user
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/");
        }

        public IActionResult EmailAvailable(string email)     //this is used with ajax when user is signing up
        {
            var repo = new QTRepository(_connectionString);
            var vm = new EmailAvailableViewModel();
            vm.IsAvailable = repo.IsEmailAvailable(email);
            return Json(vm);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new QTRepository(_connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                return Redirect("/home/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/login");
        }

        [Authorize]
        public IActionResult NewQuestion()
        {
            return View();
        }

        [Authorize] [HttpPost]
        public IActionResult NewQuestion(Question question, string tags)
        {
            List<string> tagList = new List<string>();
            tagList = tags.Split(' ').ToList();
            var repo = new QTRepository(_connectionString);
            question.DatePosted = DateTime.Now;
            question.UserId = repo.GetUserByEmail(User.Identity.Name).Id;
            repo.AddQuestion(question, tagList);
            return Redirect("/");
        }

        public IActionResult ViewQuestion(int questionId)
        {
            if (questionId <= 0)
            {
                return RedirectToAction("Index");
            }

            var repo = new QTRepository(_connectionString);
            Question question = repo.GetQuestionById(questionId);

            if (question == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new ViewQuestionViewModel { Question = question, IsAuthenticated=User.Identity.IsAuthenticated};

            if (vm.IsAuthenticated == true)
            {
                vm.User = repo.GetUserByEmail(User.Identity.Name);
            }

            return View(vm);
        }
        [Authorize] [HttpPost]
        public IActionResult PostAnswer(Answer answer)
        {
            var repo = new QTRepository(_connectionString);
            repo.AddAnswer(answer);
            return Json('0');
        }
        public IActionResult GetAnswersForQuestion(int questionId)
        {
            var repo = new QTRepository(_connectionString);
            return Json(repo.GetAnswersForQuestion(questionId));
        }
        [Authorize] [HttpPost]
        public IActionResult LikeQuestion(LikesQuestions likesQuestions)
        {
            var repo = new QTRepository(_connectionString);
            repo.LikeQuestion(likesQuestions);
            return Json('0');
        }
        public IActionResult GetUpdatedLikes(int questionId)
        {
            var repo = new QTRepository(_connectionString);
            return Json(repo.GetCurrentLikes(questionId));
        }

    }
}
