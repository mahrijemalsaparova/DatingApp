using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext //EntityFramwork'ün Dbcontext clası.
    {   
        /*hangi veritabanına bağlanıcak
        DbContextOptions generic sınıflı bir configirasyondur. Buraya DataContext'mizi options olarak geçiyoruz.
        options'u da base' e gönderiyoruz.
        base DbContext'tır.
        options: hangi veritabanına gidilecek?
        */
        public DataContext(DbContextOptions<DataContext> options) : base(options){} //ctor contranctor oluşturma
           
           
           
        public DbSet<Value> Values { get; set; } //DbSet Property --->Modeldeki Value ile Vertabanındaki 
                                                  //Values tablosu ile ilişkilendir demek.
         public DbSet<User> Users { get; set; }

        }
}
