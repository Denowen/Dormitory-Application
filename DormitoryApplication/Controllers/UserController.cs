using DormitoryApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryApplication.Controllers
{
    public class UserController
    {
        UserContext userContext = new UserContext();

       public void AddModel(User _user)
        {
            userContext.User.Add(_user);
            userContext.SaveChanges();
        }

        public List<User> GetUser()
        {
            List<User> users = new List<User>();
            users = userContext.User.OrderBy(n => n.SchoolId).ToList();
            return users;
        }

        



        
    }
}
