namespace DormitoryApplication.Models
{
    public class SelectedDorm
    {
        public int Capacity { get; set; }

        public int Id { get; set; }
        public int RemainingCapacity { get; set; }
        public string? DormNo { get; set; }

        public int DormTypeId { get; set; }

        public string? DormTypeName { get; set; }

        public int Price { get; set; }
    }
}
