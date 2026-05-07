using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SaigonRide.Data;
using System.Linq;

namespace SaigonRide.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userName = User.Identity.Name;

            // Lấy chuyến đi gần nhất của user này
            var recentRide = _context.RentalTransactions
                .Include(t => t.Vehicle)
                .Include(t => t.ReturnStation)
                .Where(t => t.CustomerName == userName)
                .OrderByDescending(t => t.StartTime)
                .FirstOrDefault();

            // Đếm tổng số chuyến đi để làm "Điểm thưởng" ảo
            var totalRides = _context.RentalTransactions.Count(t => t.CustomerName == userName);

            ViewBag.TotalRides = totalRides;

            return View(recentRide);
        }
    }
}