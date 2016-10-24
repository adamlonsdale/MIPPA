using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mippa.Models
{
    public class MippaContext : DbContext
    {
        public MippaContext(DbContextOptions<MippaContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //modelBuilder.Entity<Post>()
            //    .HasOne(p => p.Blog)
            //    .WithMany(b => b.Posts)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TeamRoster>()
                .HasOne(t => t.Team)
                .WithMany();

            builder.Entity<TeamRoster>()
                .HasOne(t => t.Team)
                .WithMany(t => t.Players)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TeamMatch>()
                .HasOne(s => s.HomeTeam)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TeamMatch>()
                .HasOne(s => s.AwayTeam)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlayerMatch>()
                .HasOne(m => m.HomePlayer)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlayerMatch>()
                .HasOne(m => m.AwayPlayer)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlayerMatch>()
                .HasOne(m => m.HomePlayerScore)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PlayerMatch>()
                .HasOne(m => m.AwayPlayerScore)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TeamResult>()
                .HasOne(m => m.Team)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamRoster> TeamRosters { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Scorecard> Scorecards { get; set; }
        public DbSet<PlayerMatch> PlayerMatches { get; set; }
        public DbSet<PlayerScore> PlayerScores { get; set; }
        public DbSet<PlayerResult> PlayerResults { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<TeamMatch> TeamMatches { get; set; }
        public DbSet<TeamResult> TeamResults { get; set; }
        public DbSet<ResetRequest> ResetRequests { get; set; }
    }
}
