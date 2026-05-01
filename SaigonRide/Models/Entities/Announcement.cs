using System;

namespace SaigonRide.Models.entities
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // Loại thông báo: "Promo" (Khuyến mãi), "Warning" (Cảnh báo), "Info" (Tin tức)
        public string Type { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}