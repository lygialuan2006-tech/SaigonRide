using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SaigonRide.Data;
using System.Linq;

namespace SaigonRide.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Tìm tất cả các hóa đơn khớp với tên người đang đăng nhập
            var userName = User.Identity.Name;
            var userHistory = _context.RentalTransactions
                .Include(t => t.Vehicle)
                .Include(t => t.ReturnStation)
                .Where(t => t.CustomerName == userName)
                .OrderByDescending(t => t.StartTime) // Xếp chuyến mới nhất lên đầu
                .ToList();

            return View(userHistory);
        }
    }
}