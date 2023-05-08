using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBusinessManagementService.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhotoPath { get; set; } = string.Empty;

        public Worker ToWorker(int? businessId)
        {
            return new Worker()
            {
                UserId = this.Id,
                User = this,
                BusinessId = businessId,
                Description = String.Empty,
                Rating = 0,
                ImagesPath = string.Empty
            };
        }

    }
}
