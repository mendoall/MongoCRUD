using Microsoft.AspNetCore.Mvc;
using MongoBackEnd.Models;
using MongoDB.Bson;
using System.Diagnostics;

namespace MongoBackEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ObtenerDatos();

            return View();
        }

        private void ObtenerDatos()
        {
            DataBaseHelper.Database db = new DataBaseHelper.Database();

            ViewBag.UserList = db.getUsers();
        }

        public IActionResult Save(User user)
        {
            DataBaseHelper.Database db = new DataBaseHelper.Database();
            if (string.IsNullOrEmpty(user.idStr))
            {
                user.dateIn = DateTime.Now;
                db.insertUser(user);
            }
            else
            {
                db.updateUser(user);
            }
            ObtenerDatos();

            return View("Index");
        }

        public IActionResult AddUser()
        {
            return View(new User());

        }

        public IActionResult Delete(string id)
        {
            DataBaseHelper.Database db = new DataBaseHelper.Database();
            db.deleteUser(id);
            ObtenerDatos();

            return View("Index");
        }

        public IActionResult LoadEdit(string id)
        {
            DataBaseHelper.Database db = new DataBaseHelper.Database();

            var user = db.findUser(id);

            return View("AddUser",user);
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
    }
}