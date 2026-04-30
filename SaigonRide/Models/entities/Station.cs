using System.Collections.Generic;

namespace SaigonRide.Models.entities
{
    public class Station
    {
        public int Id { get; set; } // Khóa chính
        public string LocationName { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentInventory { get; set; }

        // Mối quan hệ: 1 Trạm có nhiều Xe
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}