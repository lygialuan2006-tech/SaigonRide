namespace SaigonRide.Models.entities
{
    public class BaseUser
    {
        public int Id { get; set; } // Khóa chính
        public string FullName { get; set; }
        public string UserType { get; set; } // "Local" hoặc "Tourist"
    }
}