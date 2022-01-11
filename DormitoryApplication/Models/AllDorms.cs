using Microsoft.AspNetCore.Mvc;

namespace DormitoryApplication.Models
{
    public class AllDorms
    {
        public int Capacity { get; set; }
        public int RemainingCapacity { get; set; }
        public string? DormNo { get; set; }

        public int DormTypeId { get; set; }

        public int Id { get; set; }
    }
}
