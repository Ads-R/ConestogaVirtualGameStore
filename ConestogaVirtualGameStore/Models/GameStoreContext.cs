using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConestogaVirtualGameStore.Models
{
    public class GameStoreContext : IdentityDbContext<ApplicationUser>
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options): base(options)
        {
        }
    }
}
