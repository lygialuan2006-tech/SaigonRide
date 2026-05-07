using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaigonRide.Data;
using SaigonRide.Models.entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaigonRide.Controllers
{
    [Authorize] // Chỉ giữ cái này để yêu cầu đăng nhập chung
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Khách hàng gửi báo cáo
        [HttpPost]
        public async Task<IActionResult> CreateReport(int rentalTransactionId, string title, string description)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            int userId = int.Parse(userIdString);
            var transaction = await _context.RentalTransactions.FindAsync(rentalTransactionId);

            if (transaction == null) return NotFound("Rental transaction not found");

            var report = new Report
            {
                UserId = userId,
                RentalTransactionId = rentalTransactionId,
                Title = title,
                Description = description,
                Status = ReportStatus.Open,
                CreatedAt = DateTime.Now
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã gửi báo cáo thành công!";
            return RedirectToAction("Index", "Home");
        }

        // Khách hàng xem báo cáo của mình
        public async Task<IActionResult> MyReports()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            int userId = int.Parse(userIdString);

            var reports = await _context.Reports
                .Include(r => r.RentalTransaction)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(reports);
        }

        // Admin xem tất cả báo cáo (ĐÃ XÓA SẠCH [Authorize(Roles="Admin")])
        public async Task<IActionResult> AdminReports(string searchString)
        {
            if (User.FindFirst("UserType")?.Value != "Admin")
                return RedirectToAction("Index", "Home");

            var reports = _context.Reports
                .Include(r => r.User)
                .Include(r => r.RentalTransaction)
                .AsQueryable();
             
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                reports = reports.Where(r =>
                    r.Title.ToLower().Contains(searchString) ||
                    r.User.FullName.ToLower().Contains(searchString) ||
                    r.Id.ToString().Contains(searchString));
            }

            ViewBag.CurrentSearch = searchString;
            return View(await reports.OrderByDescending(r => r.CreatedAt).ToListAsync());
        }

    }
}