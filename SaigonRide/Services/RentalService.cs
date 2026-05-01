using SaigonRide.Data;

namespace SaigonRide.Services
{
    public class RentalService
    {
        private readonly ApplicationDbContext _context;

        // Constructor tự động nhận Database từ hệ thống (Dependency Injection)
        public RentalService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hàm tính toán giá tiền dự kiến
        public decimal CalculateEstimatedFare(int vehicleId, int returnStationId, int durationMinutes)
        {
            // 1. Lấy thông tin xe và trạm trả từ DB
            var vehicle = _context.Vehicles.Find(vehicleId);
            var returnStation = _context.Stations.Find(returnStationId);

            if (vehicle == null || returnStation == null) return 0;

            // 2. Tính giá gốc (Ví dụ: 15 phút * 500 VNĐ = 7500 VNĐ)
            decimal baseFare = durationMinutes * vehicle.PricePerMinute;

            // 3. Kiểm tra Tỷ lệ lấp đầy của trạm đích (Fleet Rebalancing Logic)
            double occupancyRate = (double)returnStation.CurrentInventory / returnStation.MaxCapacity;

            // 4. Áp dụng quy tắc kinh doanh: Dưới 20% thì giảm 15%
            if (occupancyRate < 0.20)
            {
                decimal discount = baseFare * 0.15m;
                return baseFare - discount;
            }

            return baseFare;
        }
    }
}