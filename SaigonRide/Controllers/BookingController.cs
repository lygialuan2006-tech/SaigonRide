using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SaigonRide.Data;
using System.Linq;

namespace SaigonRide.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Màn hình chọn Trạm/Bản đồ bắt đầu đặt xe
        public IActionResult Index()
        {
            var stations = _context.Stations
                                   .Include(s => s.Vehicles)
                                   .ToList();
            return View(stations);
        }
    }
}