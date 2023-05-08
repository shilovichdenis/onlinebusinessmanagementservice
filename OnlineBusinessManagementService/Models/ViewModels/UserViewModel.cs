using Microsoft.EntityFrameworkCore;
using OnlineBusinessManagementService.Data;
using System.ComponentModel.DataAnnotations;

namespace OnlineBusinessManagementService.Models.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhotoPath { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<FavoriteBusiness> FavoriteBusinesses { get; set; }
        public List<FavoriteService> FavoriteServices { get; set; }
        public List<RecordViewModel> Records { get; set; }

        public static void UpdateData(UserViewModel model, ref User user)
        {
            user.Name = model.Name;
            user.SurName = model.SurName;
            user.PhotoPath = model.PhotoPath;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.DateOfBirth = model.DateOfBirth;
        }
    }
}
