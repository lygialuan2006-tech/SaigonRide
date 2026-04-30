using System.ComponentModel.DataAnnotations.Schema;

namespace SaigonRide.Models.entities
{
    public class Vehicle
    {
        public int Id { get; set; } // Khóa chính
        public string Category { get; set; } // "Standard Bike" hoặc "E-Scooter"
        [Column(TypeName = "decimal(18, 2)")] // Thêm nhãn này
        public decimal PricePerMinute { get; set; }

        // Khóa ngoại trỏ về bảng Station
        public int StationId { get; set; }
        public Station Station { get; set; }
    }
}