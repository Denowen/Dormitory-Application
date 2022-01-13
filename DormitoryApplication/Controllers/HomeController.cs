using DormitoryApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DormitoryApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
         public string conString = "Data Source=LAPTOP-N7FBE5OG;Initial Catalog=Dormitory_App;Integrated Security=True";

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

            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Dormitory_App.[dbo].[User] where Email='" + usr.Email + "' and Password='" + usr.Password + "'";
            SqlDataReader readerr = cmd.ExecuteReader();
            if (readerr.Read())
            {
                string name = readerr["Name"].ToString();
                int roleId = (int)readerr["RoleId"];
                string schoolId = readerr["SchoolId"].ToString();
                Console.WriteLine("Name: " + name);
                con.Close();
                CookieOptions cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("name", name, cookie);
                Response.Cookies.Append("schoolId", schoolId, cookie);
                readerr.Close();

                if (roleId == 1)

                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Admin_yurt_secenekler", "Home");
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

            SqlConnection con = new SqlConnection(conString);
            
            string sql = "SELECT * FROM Dormitory_App.[dbo].[Dorms] dm inner join Dormitory_App.[dbo].[DormType] dt on dm.DormTypeId = dt.Id";

            SqlCommand cmd = new SqlCommand(sql, con);

            con.Open();

            List<AllDorms> DormsModel = new List<AllDorms>();

            SqlDataReader reader = cmd.ExecuteReader();


            while(reader.Read())
            {
                string DormNo = reader["DormNo"].ToString();
                string DormTypeName = reader["Name"].ToString();
                int RemainingCapacity = (int)reader["RemainingCapacity"];
                int Capacity = (int)reader["Capacity"];
                int DormTypeId = (int)reader["DormTypeId"];
                int Id = (int)reader["Id"];

                var alldorm = new AllDorms();

                alldorm.DormNo = DormNo;
                alldorm.RemainingCapacity = RemainingCapacity;
                alldorm.Capacity = Capacity;
                alldorm.DormTypeId = DormTypeId;
                alldorm.DormTypeName = DormTypeName;
                alldorm.Id = Id;

                DormsModel.Add(alldorm);

            }
            return View("Dorm_Apply", DormsModel);
            con.Close();
            reader.Close();

        }
        public ActionResult Dorm_Type()
        {

            SqlConnection con = new SqlConnection(conString);

            string sql = "SELECT * FROM Dormitory_App.[dbo].[DormType]";

            SqlCommand cmd = new SqlCommand(sql, con);

            con.Open();

            List<DormType> dormtype = new List<DormType>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string Name = reader["Name"].ToString();
                int Id = (int)reader["Id"];

                var dt = new DormType();

                dt.Name = Name;
                dt.Id = Id;

                dormtype.Add(dt);

            }
            return View("Admin_yurt_secenekler", dormtype);
            con.Close();
            reader.Close();


    }
        public ActionResult Request_Type()
        {

            SqlConnection con = new SqlConnection(conString);

            string sql = "SELECT * FROM Dormitory_App.[dbo].[Requests] rqs inner join Dormitory_App.[dbo].[RequestsType] rt on rqs.RequestTypeId = rt.Id";

            SqlCommand cmd = new SqlCommand(sql, con);

            con.Open();

            List<Request> reqtype = new List<Request>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int Id = (int)reader["Id"];
                string Description = reader["Description"].ToString();
                bool isDone = (bool)reader["isDone"];
                int RequestTypeId = (int)reader["RequestTypeId"];
                string UserSchoolId = reader["UserSchoolId"].ToString();
                string RequestName = reader["Name"].ToString();

                var req = new Request();

                req.Id = Id;
                req.Description = Description;
                req.isDone = isDone;
                req.RequestTypeId = RequestTypeId;
                req.UserSchoolId = UserSchoolId;
                req.RequestName = RequestName;

                reqtype.Add(req);

            }
            return View("Admin_talepler", reqtype);
            con.Close();
            reader.Close();


        }

        [HttpPost]
        public ActionResult Add_Room(SelectedDorm selectedDorm)
        {

            SqlConnection con = new SqlConnection(conString);

            string query = "INSERT INTO Dormitory_App.[dbo].[Dorms](DormTypeId, Capacity, RemainingCapacity, Price,DormNo) VALUES (@DormTypeId, @Capacity, @Capacity, @Price, @DormNo)";

            //string query = "select DormNo from Dormitory_App.[dbo].[Dorms] where DormNo= '" + selectedDorm.DormNo + "'";

            
            //SqlCommand cmd = new SqlCommand(query, con);
            
            //SqlDataReader readerr = cmd.ExecuteReader();
            //if (readerr.Read())
            //{
            //    string dormno = readerr["DormNo"].ToString();
            //    if(!dormno.Equals(selectedDorm.DormNo.ToString()))
            //    {
            //        Console.WriteLine("hilalll");
                    using (SqlCommand cmd2 = new SqlCommand(query, con))
                    {
                        cmd2.Parameters.AddWithValue("@DormNo", selectedDorm.DormNo);
                        cmd2.Parameters.AddWithValue("@DormTypeId", selectedDorm.DormTypeId);
                        cmd2.Parameters.AddWithValue("@Capacity", selectedDorm.Capacity);
                        cmd2.Parameters.AddWithValue("@Price", selectedDorm.Price);
                        con.Open();
                        int result = cmd2.ExecuteNonQuery();


                    }

            //    }
            //}

            return RedirectToAction("Admin_oda", "Home");
            

        }

        [HttpPost]
        public ActionResult Add_Dorm(SelectedDorm selectedDorm)
        {

            SqlConnection con = new SqlConnection(conString);

            string query = "INSERT INTO Dormitory_App.[dbo].[DormType](Name) VALUES (@DormTypeName)";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@DormTypeName", selectedDorm.DormTypeName);
                con.Open();
                int result = cmd.ExecuteNonQuery();


            }

            return RedirectToAction("Admin_yurt_secenekler", "Home");
        }

        public ActionResult Talep_Load()
        {

            SqlConnection con = new SqlConnection(conString);
            SqlConnection con2 = new SqlConnection(conString);
            SqlConnection con3 = new SqlConnection(conString);

            string sql = "SELECT * FROM Dormitory_App.[dbo].[RequestsType]";
            string sql2 = "SELECT * FROM Dormitory_App.[dbo].[Dorms]";
            string sql3 = "SELECT * FROM Dormitory_App.[dbo].[DormType]";

            SqlCommand cmd = new SqlCommand(sql, con);
            SqlCommand cmd2 = new SqlCommand(sql2, con2);
            SqlCommand cmd3 = new SqlCommand(sql3, con3);

            con.Open();
            con2.Open();
            con3.Open();


            List<RequestType> reqtype = new List<RequestType>();
            List<AllDorms> alldorms = new List<AllDorms>();
            List<DormType> dormtype = new List<DormType>();

            SqlDataReader reader = cmd.ExecuteReader();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            SqlDataReader reader3 = cmd3.ExecuteReader();

            while (reader.Read())
            {
                int Id = (int)reader["Id"];
                string Name = reader["Name"].ToString();
            

                var req = new RequestType();

                req.Id = Id;
                req.Name = Name;

                reqtype.Add(req);

            }
            while (reader2.Read())
            {
                int Id = (int)reader2["Id"];
                string DormNo = reader2["DormNo"].ToString();

                var req = new AllDorms();

                req.Id = Id;
                req.DormNo = DormNo;

                alldorms.Add(req);

            }
            while (reader3.Read())
            {
                int Id = (int)reader3["Id"];
                string Name = reader3["Name"].ToString();

                var req = new DormType();

                req.Id = Id;
                req.Name = Name;

                dormtype.Add(req);

            }
            ViewModel vw = new ViewModel();
            vw.dorms = alldorms;
            vw.dt = dormtype;
            vw.reqs = reqtype;
            return View("Talepler", vw);
            con.Close();
            con2.Close();
            con3.Close();
            reader.Close();
            reader2.Close();
            reader3.Close();

        }

        [HttpPost]
        public ActionResult Talep_sent(Talep talep)
        {

            SqlConnection con = new SqlConnection(conString);

            
            string query = "INSERT INTO Dormitory_App.[dbo].[Requests](Description, isDone, RequestTypeId, UserSchoolId) VALUES (@description, 0, @reqType, @SchoolId)";

            string UserSchoolId = Request.Cookies["schoolId"];

            Console.WriteLine("userschoolId", UserSchoolId);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@description", talep.description);
                    cmd.Parameters.AddWithValue("@reqType", talep.reqType);
                    cmd.Parameters.AddWithValue("@SchoolId", UserSchoolId);
                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Talepler", "Home");
            

        }

        public void DelRoom(int id)
        {

            SqlConnection con = new SqlConnection(conString);

            string query = "DELETE FROM Dormitory_App.[dbo].[Dorms] WHERE Id='" + id + "'";
            Console.WriteLine(id);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


        }

        [Route("Home/DeleteRoom/{id:int}")]
        public IActionResult DeleteRoom(int id)
        {

            DelRoom(id);
            Dorm_Apply2();
            return View("Admin_oda");
        }

        [Route("Home/ChooseDorm/{id:int}")]
        public IActionResult ChooseDorm(int id)
        {
            Console.WriteLine(id);
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
            Dorm_Type();
            return View();
        }
        public IActionResult Admin_talepler()
        {
            Request_Type();
            return View();
        }
        public IActionResult Admin_talep_detay()
        {
            return View();
        }
        public IActionResult Admin_oda()
        {
            Dorm_Apply2();
            return View();
        }
        public IActionResult Dorm_apply()
        {
            Dorm_Apply2();
            return View();
        }
        public IActionResult Talepler()
        {
            Talep_Load();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}