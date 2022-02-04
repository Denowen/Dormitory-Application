namespace DormitoryApplication.Models
{
    public class ViewModel
    {
        public IEnumerable<AllDorms> dorms { get; set; }
        public IEnumerable<RequestType> reqs { get; set; }
        public IEnumerable<DormType> dt { get; set; }
    }
}
