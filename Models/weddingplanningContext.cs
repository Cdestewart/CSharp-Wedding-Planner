using Microsoft.EntityFrameworkCore;
 
namespace WeddingPlanning.Models
{
    public class weddingplanningContext : DbContext
    {
        
        public weddingplanningContext(DbContextOptions<weddingplanningContext> options) : base(options) { }
            public DbSet<User> users {get;set;}
            
            public DbSet<Wedding> wedding {get;set;}

             public DbSet<Attendee> attendees { get; set; }
        
    }

}
