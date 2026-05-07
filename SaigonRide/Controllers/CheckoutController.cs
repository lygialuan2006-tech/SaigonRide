using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaigonRide.Data;
using SaigonRide.Models.entities;
using SaigonRide.Services;
using System;
using System.Linq;

namespace SaigonRide.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RentalService _rentalService;

        public CheckoutController(ApplicationDbContext context, RentalService rentalService)
        {
            _context = context;
            _rentalService = rentalService;
        }

        public IActionResult Rent(int vehicleId)
        {
            var vehicle = _context.Vehicles.Include(v => v.Station).FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null) return NotFound();

            // Đẩy dữ liệu Trạm xuống cho Mini-map (Dùng Json để Javascript đọc được dễ dàng)
            var stationsData = _context.Stations.Select(s => new {
                id = s.Id,
                name = s.LocationName,
                lat = s.Latitude,
                lng = s.Longitude,
                inventory = s.CurrentInventory,
                capacity = s.MaxCapacity
            }).ToList();

            ViewBag.StationsJson = System.Text.Json.JsonSerializer.Serialize(stationsData);

            return View(vehicle);
        }

        [HttpPost]
        public IActionResult Receipt(int vehicleId, int returnStationId, int duration)
        {
            decimal finalPrice = _rentalService.CalculateEstimatedFare(vehicleId, returnStationId, duration);

            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            var station = _context.Stations.FirstOrDefault(s => s.Id == returnStationId);

            ViewBag.VehicleId = vehicleId;
            ViewBag.ReturnStationId = returnStationId;
            ViewBag.VehicleName = vehicle?.Category;
            ViewBag.StationName = station?.LocationName;
            ViewBag.Duration = duration;
            ViewBag.BasePrice = duration * (vehicle?.PricePerMinute ?? 0);
            ViewBag.FinalPrice = finalPrice;
            ViewBag.IsDiscounted = finalPrice < ViewBag.BasePrice;

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmPayment(int vehicleId, int returnStationId, int duration, decimal finalPrice)
        {
            var vehicle = _context.Vehicles.Find(vehicleId);
            var oldStation = _context.Stations.Find(vehicle.StationId);
            var newStation = _context.Stations.Find(returnStationId);

            if (vehicle == null || oldStation == null || newStation == null) return NotFound();

            var transaction = new RentalTransaction
            {
                CustomerName = User.Identity.Name ?? "Khách vãng lai", // LƯU TÊN Ở ĐÂY
                VehicleId = vehicleId,
                ReturnStationId = returnStationId,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMinutes(duration),
                TotalDue = finalPrice,
                IsDiscountApplied = finalPrice < (vehicle.PricePerMinute * duration)
            };
            _context.RentalTransactions.Add(transaction);

            oldStation.CurrentInventory--;
            newStation.CurrentInventory++;
            vehicle.StationId = returnStationId;

            _context.SaveChanges();

            return View("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}