using DormitoryApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DormitoryApplication.Controllers
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
            return View();
        }

        [HttpPost]
        public ActionResult Login(User usr)
        {
            
            string conString = "Data Source=DESKTOP-N9HBLJE; database=Dormitory_App;Integrated Security=True";

            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select Name from Dormitory_App.[dbo].[User] where Email='" + usr.Email + "' and Password='" + usr.Password + "'";
            SqlDataReader readerr = cmd.ExecuteReader();
            if (readerr.Read())
            {
                string name = readerr["Name"].ToString();
                Console.WriteLine("Name: " + name);
                con.Close();
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append(name, usr.Email, cookie);
                readerr.Close();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                readerr.Close();
                return RedirectToAction("Giris", "Home");
            }
            
        }

        [HttpPost]
        public ActionResult Register(User usr)
        {

            string conString = "Data Source=DESKTOP-N9HBLJE; database=Dormitory_App;Integrated Security=True";

            SqlConnection con = new SqlConnection(conString);

            string query = "INSERT INTO Dormitory_App.[dbo].[User](Name, Lname, Email, SchoolId, Password) VALUES (@Name, @Lname, @Email, @SchoolId, @Password)";

            if (!usr.Password.Equals(usr.Password2))
            {
                return RedirectToAction("Register", "Home");
            }
            else { 
            
            using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Name", usr.Name);
                            cmd.Parameters.AddWithValue("@Lname", usr.Lname);
                            cmd.Parameters.AddWithValue("@Email", usr.Email);
                            cmd.Parameters.AddWithValue("@SchoolId", usr.SchoolId);
                            cmd.Parameters.AddWithValue("@Password", usr.Password);
                            con.Open();
                            int result = cmd.ExecuteNonQuery();
                        }
            return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult Giris()
        {
            return View();
        }

        public IActionResult Odeme()
        {
            return View();
        }
        public IActionResult Sifreunuttum()
        {
            return View();
        }
        public IActionResult Sifreunuttum2()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Admin_yurt_secenekler()
        {
            return View();
        }
        public IActionResult Admin_talepler()
        {
            return View();
        }
        public IActionResult Admin_talep_detay()
        {
            return View();
        }
        public IActionResult Admin_oda()
        {
            return View();
        }
        public IActionResult Dorm_apply()
        {
            return View();
        }
        public IActionResult Talepler()
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