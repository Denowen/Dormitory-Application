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
                Response.Cookies.Append("email", usr.Email, cookie);
                Response.Cookies.Append("fullname", name, cookie);
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

            string query = "INSERT INTO Dormitory_App.[dbo].[User](Name, Lname, Email, SchoolId, Password, RoleId) VALUES (@Name, @Lname, @Email, @SchoolId, @Password, 1)";

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
            return RedirectToAction("Giris", "Home");
            }

        }

        
        public ActionResult Dorm_Apply2()
        {
            string conString = "Data Source=DESKTOP-N9HBLJE; database=Dormitory_App;Integrated Security=True";

            SqlConnection con = new SqlConnection(conString);
            
            string sql = "SELECT * FROM Dormitory_App.[dbo].[Dorms]";

            SqlCommand cmd = new SqlCommand(sql, con);

            con.Open();

            List<AllDorms> DormsModel = new List<AllDorms>();

            SqlDataReader reader = cmd.ExecuteReader();


            while(reader.Read())
            {
                string DormNo = reader["DormNo"].ToString();
                int RemainingCapacity = (int)reader["RemainingCapacity"];
                int Capacity = (int)reader["Capacity"];
                int DormTypeId = (int)reader["DormTypeId"];
                int Id = (int)reader["Id"];

                var alldorm = new AllDorms();

                alldorm.DormNo = DormNo;
                alldorm.RemainingCapacity = RemainingCapacity;
                alldorm.Capacity = Capacity;
                alldorm.DormTypeId = DormTypeId;
                alldorm.Id = Id;

                DormsModel.Add(alldorm);

            }
            return View("Dorm_Apply", DormsModel);
            con.Close();
            reader.Close();

        }


        [Route("Home/ChooseDorm/{id?}")]
        public IActionResult ChooseDorm(int? id)
        {
            Console.WriteLine("dormno:", id);
            return View("Index");
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
            Dorm_Apply2();
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