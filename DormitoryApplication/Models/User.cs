namespace DormitoryApplication.Models
{
    public class User
    {
        public string? Name { get; set; }  
        public string? Lname { get; set; }
        public string? Email { get; set; }
        public string? SchoolId { get; set; }
        public string? Password { get; set; }
        public string? Password2 { get; set; }
        public string? DormTypeId { get; set; }
        public string? DormId { get; set; }
        public string? DormName { get; set; }
        public string? DormTypeName { get; set; }
        public int? RecoveryCode { get; set; }

    }
}
