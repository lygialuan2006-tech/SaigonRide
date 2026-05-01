using System.Collections.Generic;

namespace SaigonRide.Models.entities
{
    public class Station
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentInventory { get; set; }

        // 2 Biến mới để vẽ lên bản đồ
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}