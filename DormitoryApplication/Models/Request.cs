namespace DormitoryApplication.Models
{
    public class Request
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool isDone { get; set; }

        public int RequestTypeId { get; set; }

        public string UserSchoolId { get; set; }

        public string RequestName { get; set; }

        public string DormNo { get; set; }

        public string DormTypeName { get; set; }

    }
}
