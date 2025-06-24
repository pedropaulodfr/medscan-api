using authentication_jwt.DTO;
using Microsoft.EntityFrameworkCore;

namespace authentication_jwt.Models
{
    public partial class AppDbContext
    {
        public virtual DbSet<UsuarioNotificacao> UsuariosNotificacao { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioNotificacao>().HasNoKey();
        }
    }
}