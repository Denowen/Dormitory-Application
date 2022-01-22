using DormitoryApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DormitoryApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
         public string conString = "Data Source=DESKTOP-N9HBLJE;Initial Catalog=Dormitory_App;Integrated Security=True";
        CookieOptions cookie = new CookieOptions();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult DormType_List_Home()
        {

            SqlConnection con = new SqlConnection(conString);

            string sql = "SELECT * FROM Dormitory_App.[dbo].[DormType]";

            SqlCommand cmd = new SqlCommand(sql, con);

            con.Open();

            List<DormType> DormTypeModel = new List<DormType>();

            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                string DormTypeName = reader["Name"].ToString();
                string DormTypeDesc = reader["Description"].ToString();
                string Gender = reader["Gender"].ToString();
                int Price = (int)reader["Price"];
                int Id = (int)reader["Id"];

                var alldorm = new DormType();

                alldorm.Name = DormTypeName;
                alldorm.Id = Id;
                alldorm.Price = Price;
                alldorm.Description = DormTypeDesc;
                alldorm.Gender = Gender;

                DormTypeModel.Add(alldorm);

            }
            return View("Index", DormTypeModel);
            con.Close();
            reader.Close();

        }

        public IActionResult Index()
        {
            DormType_List_Home();
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
                
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("name", name, cookie);
                Response.Cookies.Append("roleId", roleId.ToString(), cookie);
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
                string Gender = reader["Gender"].ToString();
                string Description = reader["Description"].ToString();
                int Id = (int)reader["Id"];
                int Price = (int)reader["Price"];

                var dt = new DormType();

                dt.Name = Name;
                dt.Gender = Gender;
                dt.Id = Id;
                dt.Description = Description;
                dt.Price = Price;

                dormtype.Add(dt);

            }
            return View("Admin_yurt_secenekler", dormtype);
            con.Close();
            reader.Close();


    }
        public ActionResult Request_Type()
        {

            SqlConnection con = new SqlConnection(conString);

            string sql = "SELECT * FROM Dormitory_App.[dbo].[Requests] rqs inner join Dormitory_App.[dbo].[RequestsType] rt on rqs.RequestTypeId = rt.Id WHERE rqs.isDone<1";

            SqlCommand cmd = new SqlCommand(sql, con);

            con.Open();

            List<Request> reqtype = new List<Request>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int Id = (int)reader["Id"];
                string Description = reader["Description"].ToString();
                int isDone = (int)reader["isDone"];
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

            string query = "INSERT INTO Dormitory_App.[dbo].[Dorms](DormTypeId, Capacity, RemainingCapacity,DormNo) VALUES (@DormTypeId, @Capacity, @Capacity, @DormNo)";

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
                        con.Open();
                        cmd2.ExecuteNonQuery();
                    }

            //    }
            //}

            return RedirectToAction("Admin_oda", "Home");
            

        }

        [Route("Home/Admin_talep_detay2/{id:int}")]
        public ActionResult Admin_talep_detay2(int id)
        {

            SqlConnection con = new SqlConnection(conString);

            string sql = "SELECT rqs.Id, rqs.Description, rqs.isDone, rt.Name, rqs.DormNo, rqs.DormType, rqs.UserSchoolId FROM Dormitory_App.[dbo].[Requests] rqs inner join Dormitory_App.[dbo].[RequestsType] rt on rqs.RequestTypeId = rt.Id WHERE rqs.Id='" + id + "'";

            SqlCommand cmd = new SqlCommand(sql, con);

            con.Open();

            List<Request> reqtype = new List<Request>();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int Id = (int)reader["Id"];
                string Description = reader["Description"].ToString();
                int isDone = (int)reader["isDone"];
                string UserSchoolId = reader["UserSchoolId"].ToString();
                string RequestName = reader["Name"].ToString();
                string DormNo = reader["DormNo"].ToString();
                string DormType = reader["DormType"].ToString();


                var req = new Request();

                req.Id = Id;
                req.Description = Description;
                req.isDone = isDone;
                req.UserSchoolId = UserSchoolId;
                req.RequestName = RequestName;
                req.DormNo = DormNo;
                req.DormTypeName = DormType;

                reqtype.Add(req);

            }
           
            con.Close();
            reader.Close();

            return View("Admin_talep_detay", reqtype);


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

            
            string query = "INSERT INTO Dormitory_App.[dbo].[Requests](Description, isDone, RequestTypeId, UserSchoolId, DormNo, DormType) VALUES (@description, 0, @reqType, @SchoolId, @DormNo, @DormType)";

            string UserSchoolId = Request.Cookies["schoolId"];

            Console.WriteLine("userschoolId", UserSchoolId);
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@description", talep.description);
                    cmd.Parameters.AddWithValue("@reqType", talep.reqType);
                    cmd.Parameters.AddWithValue("@SchoolId", UserSchoolId);
                cmd.Parameters.AddWithValue("@DormNo", talep.dormNo);
                cmd.Parameters.AddWithValue("@DormType", talep.dormType);
                con.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Talepler", "Home");
            

        }

        [Route("Home/DeleteDorm/{id:int}")]
        public ActionResult DeleteDorm(int id)
        {

            SqlConnection con = new SqlConnection(conString);

            string query = "DELETE FROM Dormitory_App.[dbo].[DormType] WHERE Id='" + id + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();



            Dorm_Type();
            return View("Admin_yurt_secenekler");

        }

        [HttpPost]
        public ActionResult Add_DormType(DormType dormtype)
        {

            SqlConnection con = new SqlConnection(conString);

            string query = "INSERT INTO Dormitory_App.[dbo].[DormType](Name, Description, Price, Gender) VALUES (@Name, @Description, @Price, @Gender)";

            using (SqlCommand cmd2 = new SqlCommand(query, con))
            {
                cmd2.Parameters.AddWithValue("@Name", dormtype.Name);
                cmd2.Parameters.AddWithValue("@Gender", dormtype.Gender);
                cmd2.Parameters.AddWithValue("@Description", dormtype.Description);
                cmd2.Parameters.AddWithValue("@Price", dormtype.Price);
                con.Open();
                cmd2.ExecuteNonQuery();

            }

            Dorm_Type();
            return RedirectToAction("Admin_yurt_secenekler", "Home");

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


        [Route("Home/TalepEnd/{id:int}")]
        public IActionResult TalepEnd(int id)
        {

            SqlConnection con = new SqlConnection(conString);

            con.Open();

            string query2 = "UPDATE Dormitory_App.[dbo].[Requests] SET isDone='" + 1 + "' WHERE Id='" + id + "'";

            SqlCommand cmd2 = new SqlCommand(query2, con);

            cmd2.ExecuteNonQuery();

            con.Close();


            Dorm_Type();
            return RedirectToAction("Admin_talepler", "Home");
        }


        [Route("Home/DeleteRoom/{id:int}")]
        public IActionResult DeleteRoom(int id)
        {

            DelRoom(id);
            Dorm_Apply2();
            return View("Admin_oda");
        }

        [Route("Home/ChooseDorm/{id:int}")]
        public ActionResult ChooseDorm(int id)
        {

            SqlConnection con = new SqlConnection(conString);

            string query = "SELECT * FROM Dormitory_App.[dbo].[Dorms] WHERE Id='" + id + "'";

            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            List<AllDorms> dorms = new List<AllDorms>();

            SqlDataReader reader = cmd.ExecuteReader();

            int Id = 0;
            int DormTypeId = 0;
            int Remaining = 0;
            while (reader.Read())
            {
                Id = (int)reader["Id"];
                DormTypeId = (int)reader["DormTypeId"];
                Remaining = (int)reader["RemainingCapacity"];
                Remaining -= 1;

                var dorms2 = new AllDorms();

                dorms2.Id = Id;
                dorms2.DormTypeId = DormTypeId;
                dorms2.RemainingCapacity = Remaining-1;

                dorms.Add(dorms2);
            }
            reader.Close();
            string UserSchoolId = Request.Cookies["schoolId"];

            string query2 = "UPDATE Dormitory_App.[dbo].[User] SET DormId='" + Id + "', DormTypeId='" + DormTypeId + "' WHERE SchoolId='" + UserSchoolId + "'";

            SqlCommand cmd2 = new SqlCommand(query2, con);

            cmd2.ExecuteNonQuery();

            string query3 = "UPDATE Dormitory_App.[dbo].[Dorms] SET RemainingCapacity='" + Remaining + "' WHERE Id='" + Id + "'";

            SqlCommand cmd3 = new SqlCommand(query3, con);

            cmd3.ExecuteNonQuery();
            con.Close();

            Dorm_Type();
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Giris()
        {
            return View();
        }

        public IActionResult Logout()
        {
            Console.WriteLine("Girdi");
            if (Request.Cookies["name"] != null)
            {
                Response.Cookies.Delete("name");
            }
            if (Request.Cookies["schoolId"] != null)
            {
                Response.Cookies.Delete("schoolId");
            }
            return RedirectToAction("Giris", "Home");
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
            if (Request.Cookies["roleId"] == "2" || Request.Cookies["roleId"] == "3")
            {
                Dorm_Type();
                return View();
            }
            else
            {
                return RedirectToAction("Giris", "Home");
            }
            
        }
        public IActionResult Admin_talepler()
        {
            if (Request.Cookies["roleId"] != "1")
            {
                Request_Type();
                return View();
            }
            else
            {
                return RedirectToAction("Giris", "Home");
            }
            
        }
        public IActionResult Admin_talep_detay()
        {
            if (Request.Cookies["roleId"] == "2" || Request.Cookies["roleId"] == "3")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Giris", "Home");
            }
            
        }
        public IActionResult Admin_oda()
        {
            if (Request.Cookies["roleId"] == "2" || Request.Cookies["roleId"] == "3")
            {
                Dorm_Apply2();
                return View();
            }
            else
            {
                return RedirectToAction("Giris", "Home");
            }
            
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