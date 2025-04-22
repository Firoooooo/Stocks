using Microsoft.EntityFrameworkCore;
using StocksAPI.Models;
using System.Reflection.Emit;

namespace StocksAPI.Data
{
    /// <summary>
    /// database context for the stocks database
    /// </summary>
    public class StocksDBContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserPortfolio> UserPortfolio { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<PortfolioValueHistory> PortfolioValueHistory { get; set; }
        public DbSet<Stock> Stock { get; set; }


        /// <summary>
        /// constructor for the database context
        /// </summary>
        /// <param name="_sTOCKOptions">the passed context class is degenerated and the db context class is passed through</param>
        public StocksDBContext(DbContextOptions<StocksDBContext> _sTOCKOptions) 
            : base(_sTOCKOptions)
        {

        }

        /// <summary>
        /// configures the database context
        /// </summary>
        /// <param name="_mODEL">model builder</param>
        protected override void OnModelCreating(ModelBuilder _mODEL)
        {
            _mODEL.Entity<User>()
                .HasKey(U => U.UserID);

            _mODEL.Entity<Stock>()
                .HasKey(S => S.StockID);

            _mODEL.Entity<PortfolioValueHistory>()
                .HasKey(P => P.HistoryID);

            _mODEL.Entity<Transaction>()
                .HasKey(T => T.TransactionID);

            _mODEL.Entity<UserPortfolio>()
                .HasKey(P => P.PortfolioID);

            _mODEL.Entity<Transaction>()
                .HasOne(T => T.User)
                .WithMany(U => U.Transactions)
                .HasForeignKey(T => T.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            _mODEL.Entity<Transaction>()
                .HasOne(T => T.Stock)
                .WithMany(S => S.Transactions)
                .HasForeignKey(T => T.StockID)
                .OnDelete(DeleteBehavior.Cascade);

            _mODEL.Entity<PortfolioValueHistory>()
                .HasOne(P => P.User)
                .WithMany(U => U.PortfolioValueHistories)
                .HasForeignKey(P => P.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            _mODEL.Entity<UserPortfolio>()
                .HasOne(P => P.User)
                .WithMany(u => u.UserPortfolios)
                .HasForeignKey(P => P.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            _mODEL.Entity<UserPortfolio>()
                .HasOne(P => P.Stock)
                .WithMany(S => S.UserPortfolios)
                .HasForeignKey(P => P.StockID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
