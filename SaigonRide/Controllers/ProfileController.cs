using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SaigonRide.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        // Màn hình Settings chính
        public IActionResult Index()
        {
            // Trong thực tế, cậu sẽ query Database để lấy User info hiện tại
            // Ở đây tớ truyền tạm ViewBag để hiển thị giao diện
            ViewBag.CurrentName = User.Identity.Name;
            ViewBag.CurrentEmail = "user@saigonride.vn"; // Giả lập
            ViewBag.AvatarPath = "/images/avatars/default.png"; // Giả lập

            return View();
        }

        // Màn hình Hỗ trợ (Support) đã làm ở bước trước
        public IActionResult Support()
        {
            return View();
        }

        // Xử lý Upload Avatar
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            if (avatarFile != null && avatarFile.Length > 0)
            {
                // Đảm bảo thư mục tồn tại
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên file ngẫu nhiên để không bị trùng
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + avatarFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(fileStream);
                }

                // TODO: Lưu đường dẫn "/images/avatars/uniqueFileName" vào cột AvatarUrl của bảng User trong Database.
                TempData["SuccessMessage"] = "Cập nhật ảnh đại diện thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}