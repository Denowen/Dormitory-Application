namespace DormitoryApplication.Models
{
    public class Profile
    {
        public IEnumerable<User> usr { get; set; }
        public IEnumerable<Request> req { get; set; }
    }
}
