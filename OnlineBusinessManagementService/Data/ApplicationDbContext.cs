using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OnlineBusinessManagementService.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<FavoriteBusiness> FavoriteBusinesses { get; set; }
        public DbSet<FavoriteService> FavoriteServices { get; set; }

        public DbSet<CategoryOfService> Categories { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessReview> BusinessReviews { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Blacklist> Blacklist { get; set; }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerReview> WorkerReviews { get; set; }
        public DbSet<WorkerServices> WorkerServices { get; set; }
        public DbSet<Schedule> Schedules { get; set; }


        public DbSet<Service> Services { get; set; }
        public DbSet<AdvertisementServices> AdvertisementServices { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Record> Records { get; set; }
        public DbSet<RecordServices> RecordServices { get; set; }
        public DbSet<TimeForSchedule> TimeForSchedule { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Business>().HasIndex(e => e.UserId).IsUnique();

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole()
                    {
                        Id = "71882a8e-3d94-4dd9-97c4-bac815be9fc2",
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole()
                    {
                        Id = "9d3bf9af-7384-4d35-b1f6-ab0552b327c0",
                        Name = "Manager",
                        NormalizedName = "MANAGER"
                    },
                    new IdentityRole()
                    {
                        Id = "e8a23752-e32d-446e-a3b5-7f4b49004dee",
                        Name = "Worker",
                        NormalizedName = "WORKER"
                    },
                    new IdentityRole()
                    {
                        Id = "73843752-7f4b-446e-4dee-e32d4900a3b5",
                        Name = "User",
                        NormalizedName = "USER"
                    }
                );
            builder.Entity<CategoryOfService>().HasData(
                    new CategoryOfService()
                    {
                        Id = 1,
                        Name = "Hairdressing Services",
                        ImagePath = ""
                    },
                    new CategoryOfService()
                    {
                        Id = 2,
                        Name = "Eyelashes",
                        ImagePath = ""
                    },
                    new CategoryOfService()
                    {
                        Id = 3,
                        Name = "Fitness",
                        ImagePath = ""
                    },
                    new CategoryOfService()
                    {
                        Id = 4,
                        Name = "Barbershop",
                        ImagePath = ""
                    },
                    new CategoryOfService()
                    {
                        Id = 5,
                        Name = "Brows",
                        ImagePath = ""
                    },
                    new CategoryOfService()
                    {
                        Id = 6,
                        Name = "Visage",
                        ImagePath = ""
                    },
                    new CategoryOfService()
                    {
                        Id = 7,
                        Name = "Cosmetology, care",
                        ImagePath = ""
                    }
               );
            builder.Entity<TimeForSchedule>().HasData(
                    new TimeForSchedule()
                    {
                        Id = 1,
                        TimeInt = 0,
                        TimeString = "00:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 2,
                        TimeInt = 1,
                        TimeString = "01:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 3,
                        TimeInt = 2,
                        TimeString = "02:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 4,
                        TimeInt = 3,
                        TimeString = "03:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 5,
                        TimeInt = 4,
                        TimeString = "04:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 6,
                        TimeInt = 5,
                        TimeString = "05:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 7,
                        TimeInt = 6,
                        TimeString = "06:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 8,
                        TimeInt = 7,
                        TimeString = "07:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 9,
                        TimeInt = 8,
                        TimeString = "08:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 10,
                        TimeInt = 9,
                        TimeString = "09:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 11,
                        TimeInt = 10,
                        TimeString = "10:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 12,
                        TimeInt = 11,
                        TimeString = "11:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 13,
                        TimeInt = 12,
                        TimeString = "12:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 14,
                        TimeInt = 13,
                        TimeString = "13:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 15,
                        TimeInt = 14,
                        TimeString = "14:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 16,
                        TimeInt = 15,
                        TimeString = "15:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 17,
                        TimeInt = 16,
                        TimeString = "16:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 18,
                        TimeInt = 17,
                        TimeString = "17:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 19,
                        TimeInt = 18,
                        TimeString = "18:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 20,
                        TimeInt = 19,
                        TimeString = "19:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 21,
                        TimeInt = 20,
                        TimeString = "20:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 22,
                        TimeInt = 21,
                        TimeString = "21:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 23,
                        TimeInt = 22,
                        TimeString = "22:00"
                    },
                    new TimeForSchedule()
                    {
                        Id = 24,
                        TimeInt = 23,
                        TimeString = "23:00"
                    }
               );
        }
    }
}
