using DormitoryApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace DormitoryApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
         public string conString = "Data Source=LAPTOP-N7FBE5OG;Initial Catalog=Dormitory_App;Integrated Security=True";
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
                string email = readerr["Email"].ToString();
                Console.WriteLine(email);
                Console.WriteLine("Name: " + name);
                con.Close();
                
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("name", name, cookie);
                Response.Cookies.Append("roleId", roleId.ToString(), cookie);
                Response.Cookies.Append("schoolId", schoolId, cookie);
                Response.Cookies.Append("email", email, cookie);
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
            SqlConnection con2 = new SqlConnection(conString);
            
                
            string sql = "SELECT * FROM Dormitory_App.[dbo].[Dorms] dm inner join Dormitory_App.[dbo].[DormType] dt on dm.DormTypeId = dt.Id";
            string sql2 = "SELECT * FROM Dormitory_App.[dbo].[Applications] WHERE dormId = @dormId";
            

            SqlCommand cmd = new SqlCommand(sql, con);
            
            
            con.Open();
            
            
            List<AllDorms> DormsModel = new List<AllDorms>();

            SqlDataReader reader = cmd.ExecuteReader();
            

            while (reader.Read())
            {

                string DormNo = reader["DormNo"].ToString();
                string DormTypeName = reader["Name"].ToString();
                string DormTypeName2 = reader["Description"].ToString();
                int RemainingCapacity = (int)reader["RemainingCapacity"];
                int Capacity = (int)reader["Capacity"];
                int DormTypeId = (int)reader["DormTypeId"];
                int Id = (int)reader["Id"];

                

                var alldorm = new AllDorms();

                alldorm.DormNo = DormNo;
                alldorm.RemainingCapacity = RemainingCapacity;
                alldorm.Capacity = Capacity;
                alldorm.DormTypeId = DormTypeId;
                alldorm.DormTypeName = DormTypeName + " " + DormTypeName2;
                alldorm.Id = Id;
                    

                DormsModel.Add(alldorm);
            }
            con.Close();

            for (int i = 0; i < DormsModel.Count; i++)
            {
                SqlCommand cmd2 = new SqlCommand(sql2, con2);
                cmd2.Parameters.AddWithValue("@dormId", DormsModel[i].Id);
                con2.Open();
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    string RoomMate = reader2["roomMate"].ToString();
                    DormsModel[i].RoomMate = RoomMate;

                }
            con2.Close();
            reader2.Close();
            }

            
            return View("Dorm_Apply", DormsModel);
            
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

            if(selectedDorm.Id > 0)
            {
                string query = "UPDATE Dormitory_App.[dbo].[Dorms] SET DormNo='" + selectedDorm.DormNo + "' ,Capacity='" + selectedDorm.Capacity + "' ,RemainingCapacity='" + selectedDorm.RemainingCapacity + "' ,DormTypeId='" + selectedDorm.DormTypeId + "' WHERE Id='" + selectedDorm.Id + "'";

                SqlCommand cmd2 = new SqlCommand(query, con);
                con.Open();
                cmd2.ExecuteNonQuery();

                con.Close();
            }
            else
            {
                string query = "INSERT INTO Dormitory_App.[dbo].[Dorms](DormTypeId, Capacity, RemainingCapacity,DormNo) VALUES (@DormTypeId, @Capacity, @Capacity, @DormNo)";


                using (SqlCommand cmd2 = new SqlCommand(query, con))
                {
                    cmd2.Parameters.AddWithValue("@DormNo", selectedDorm.DormNo);
                    cmd2.Parameters.AddWithValue("@DormTypeId", selectedDorm.DormTypeId);
                    cmd2.Parameters.AddWithValue("@Capacity", selectedDorm.Capacity);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                }
            }

            

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
            

            if (dormtype.Id > 0)
            {
                string query = "UPDATE Dormitory_App.[dbo].[DormType] SET Name='" + dormtype.Name + "' ,Description='" + dormtype.Description + "' ,Price='" + dormtype.Price + "' ,Gender='" + dormtype.Gender + "' WHERE Id='" + dormtype.Id + "'";

                SqlCommand cmd2 = new SqlCommand(query, con);
                con.Open();
                cmd2.ExecuteNonQuery();

                con.Close();
            }
            else { 

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

        public bool checkApplication()
        {
            bool result = false;
            string UserSchoolId = Request.Cookies["schoolId"];
            SqlConnection con3 = new SqlConnection(conString);

            string sql3 = "SELECT * FROM Dormitory_App.[dbo].[Applications] WHERE schoolId='" + UserSchoolId + "'";
            SqlCommand cmd3 = new SqlCommand(sql3, con3);
            con3.Open();
            SqlDataReader reader3 = cmd3.ExecuteReader();

            if (reader3.Read())
            {
                result = true;
            }
            return result;
        }

        public static void MailSender(string body, string sendEmail, string subject)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("yurtlar.destek@gmail.com");
                mail.To.Add(sendEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("yurtlar.destek@gmail.com", "yurt1234");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
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
            SqlConnection con2 = new SqlConnection(conString);
            SqlConnection con3 = new SqlConnection(conString);

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
            string user_name = Request.Cookies["name"];

            string query2 = "UPDATE Dormitory_App.[dbo].[User] SET DormId='" + Id + "', DormTypeId='" + DormTypeId + "' WHERE SchoolId='" + UserSchoolId + "'";

            SqlCommand cmd2 = new SqlCommand(query2, con);

            cmd2.ExecuteNonQuery();

            string query3 = "UPDATE Dormitory_App.[dbo].[Dorms] SET RemainingCapacity='" + Remaining + "' WHERE Id='" + Id + "'";

            SqlCommand cmd3 = new SqlCommand(query3, con);

            cmd3.ExecuteNonQuery();

            con.Close();

            string roomMate = user_name;
            string temp = "";
            string query4 = "SELECT * FROM Dormitory_App.[dbo].[Applications] WHERE dormId= '" + Id + "'";
            SqlCommand cmd4 = new SqlCommand(query4, con2);

            con2.Open();
            SqlDataReader reader2 = cmd4.ExecuteReader();

            
            while (reader2.Read())
            {
                
                if(reader2["roomMate"].ToString() != "")
                {
                    roomMate = roomMate + "    " +  reader2["roomMate"];
                }
                
               
            }
            reader2.Close();
            string query6 = "UPDATE Dormitory_App.[dbo].[Applications] SET roomMate='" + roomMate + "' WHERE dormId='" + Id + "'";
            SqlCommand cmd6 = new SqlCommand(query6, con2);
            cmd6.ExecuteNonQuery();

            con2.Close();
            

            


            string query5 = "INSERT INTO Dormitory_App.[dbo].[Applications](schoolId, dormId, dormTypeId, roomMate) VALUES (@schoolId, @dormId, @dormTypeId, @roomMate)";
            using (SqlCommand cmd5 = new SqlCommand(query5, con3))
            {
                cmd5.Parameters.AddWithValue("@schoolId", UserSchoolId);
                cmd5.Parameters.AddWithValue("@dormId",Id);
                cmd5.Parameters.AddWithValue("@dormTypeId",DormTypeId);
                cmd5.Parameters.AddWithValue("@roomMate", roomMate);
                con3.Open();
                cmd5.ExecuteNonQuery();

            }
            string em = Request.Cookies["email"];
            string nm = Request.Cookies["name"];
            string bod = "<h3>Yurt Başvurunuz Başarılı</h3><p>Sevgili " + nm + ", </p><p>Yurt başvurunuz başarılı bir şekilde alınmıştır, teşekkür ederiz.</p>";
            MailSender(bod, em, "Yurt Başvurusu");

            Dorm_Type();
            return RedirectToAction("Odeme", "Home");

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

        [HttpPost]
        public ActionResult Dorm_pay()
        {
            string schoolid = Request.Cookies["schoolId"];

            SqlConnection con = new SqlConnection(conString);

            con.Open();

            string query2 = "UPDATE Dormitory_App.[dbo].[User] SET isPaid=1 WHERE SchoolId='" + schoolid + "'";

            SqlCommand cmd2 = new SqlCommand(query2, con);

            cmd2.ExecuteNonQuery();

            con.Close();



            return RedirectToAction("Profile", "Home");
        }

        public IActionResult Odeme()
        {
            return View();
        }
        public IActionResult Success()
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
            bool res = checkApplication();
            if (res)
            {
                return View("Success", "Home");
            }
            else
            {
                Dorm_Apply2();
            }
            
            return View();
        }

        public ActionResult Profile_detail()
        {
            SqlConnection con = new SqlConnection(conString);

            con.Open();

            string UserSchoolId = Request.Cookies["schoolId"];

            string sql = "SELECT u.Name as Name, u.Lname, u.Email, u.SchoolId, d.DormNo, f.Name as Name2, f.Description FROM Dormitory_App.[dbo].[User] u JOIN Dormitory_App.[dbo].[Dorms] d ON u.DormId=d.Id JOIN Dormitory_App.[dbo].[DormType] f ON u.DormTypeId=f.Id WHERE u.SchoolId='" + UserSchoolId + "'";

            SqlCommand cmd = new SqlCommand(sql, con);

            List<User> UserList = new List<User>();

            SqlDataReader reader = cmd.ExecuteReader();

            int dormid3 = 0;

            while (reader.Read())
            {

                var usr = new User();

                string name = reader["Name"].ToString();
                string lname = reader["Lname"].ToString();
                string email = reader["Email"].ToString();
                string schoolid = reader["SchoolId"].ToString();

                if (reader["DormNo"] == DBNull.Value)
                {
                    string dormid = " ";
                    usr.DormName = dormid;
                }
                else
                {
                    string dormid = reader["DormNo"].ToString();
                    usr.DormName = dormid;
                }

                if (reader["Name2"] == DBNull.Value)
                {
                    string dormtype = " ";
                    usr.DormTypeName = dormtype;
                }
                else
                {
                    string dormtype = reader["Name2"].ToString();
                    usr.DormTypeName = dormtype;
                }

                if (reader["Description"] == DBNull.Value)
                {
                    string desc = string.Empty;
                    usr.DormTypeName += desc;
                }
                else
                {
                    string desc = reader["Description"].ToString();
                    usr.DormTypeName += desc;
                }



                usr.Name = name;
                usr.Email = email;
                usr.Lname = lname;
                usr.SchoolId = schoolid;


                UserList.Add(usr);

            }
            SqlConnection con2 = new SqlConnection(conString);

            con2.Open();
            string sql2 = "SELECT s.Id, s.isDone, v.Name FROM Dormitory_App.[dbo].[Requests] s JOIN Dormitory_App.[dbo].[RequestsType] v ON s.RequestTypeId = v.Id WHERE s.UserSchoolId ='" + UserSchoolId + "'";

            SqlCommand cmd2 = new SqlCommand(sql2, con2);

            List<Request> UserList2 = new List<Request>();

            SqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                string name = (string)reader2["Name"];
                int idd = (int)reader2["Id"];
                int isDone = (int)reader2["isDone"];

                var req = new Request();

                req.RequestName = name;
                req.Id = idd;
                req.isDone = isDone;


                UserList2.Add(req);

            }

            Profile pf = new Profile();
            pf.usr = UserList;
            pf.req = UserList2;
            return View("Profile", pf);
            con.Close();
            con2.Close();
            reader.Close();
            reader2.Close();

        }

        [HttpPost]
        public ActionResult Profile_edit(User usr)
        {

            string schoolid = Request.Cookies["schoolId"];

            SqlConnection con = new SqlConnection(conString);

            con.Open();

            if(usr.Password != null && usr.Password2 != null)
            {
                if ((usr.Password == usr.Password2) && (usr.Password != null))
                {
                    string query2 = "UPDATE Dormitory_App.[dbo].[User] SET Password='" + usr.Password + "' WHERE SchoolId='" + schoolid + "'";
                    SqlCommand cmd2 = new SqlCommand(query2, con);

                    cmd2.ExecuteNonQuery();

                    con.Close();
                }
            }

            return RedirectToAction("Profile", "Home");

        }





            

            

        public IActionResult Profile()
        {
            Profile_detail();
            return View();
        }

        public IActionResult Talepler()
        {
            Talep_Load();
            return View();
        }

        [Route("Home/usrDetay/{id:int}")]
        public ActionResult usrDetay(int id)
        {
            SqlConnection con = new SqlConnection(conString);

            string sql = "SELECT rqs.Id, rqs.Description, rqs.isDone, rt.Name, rqs.DormNo, rqs.DormType, rqs.UserSchoolId, rqs.Response FROM Dormitory_App.[dbo].[Requests] rqs inner join Dormitory_App.[dbo].[RequestsType] rt on rqs.RequestTypeId = rt.Id WHERE rqs.Id='" + id + "'";

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
                string response = reader["Response"].ToString();


                var req = new Request();

                req.Id = Id;
                req.Response = response;
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

            return View("userTalepDetay", reqtype);
        }

        public IActionResult userTalepDetay()
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