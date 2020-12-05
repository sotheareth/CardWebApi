using Microsoft.EntityFrameworkCore;

namespace CardWebApi.Models
{
    public class PaymentDetailContext : DbContext
    {
        public PaymentDetailContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<PaymentDetail> PaymentDetail { get; set; }

    }
}
