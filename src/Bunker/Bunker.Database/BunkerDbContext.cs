using Bunker.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bunker.Database
{
    public class BunkerDbContext : DbContext
    {
        public DbSet<Challange>       Challanges       { get; set; }
        public DbSet<ChallangeTeam>   ChallangeTeams   { get; set; }
        public DbSet<Company>         Companies        { get; set; }
        public DbSet<CompanyJoinInfo> CompanyJoinInfos { get; set; }
        public DbSet<CompanyPlayer>   CompanyPlayers   { get; set; }
        public DbSet<Player>          Players          { get; set; }
        public DbSet<PlayerRole>      PlayerRoles      { get; set; }
        public DbSet<Role>            Roles            { get; set; }
        public DbSet<PlayerTeam>      PlayerTeams      { get; set; }
        public DbSet<Task>            Tasks            { get; set; }
        public DbSet<Team>            Teams            { get; set; }
        public DbSet<TeamJoinInfo>    TeamJoinInfos    { get; set; }
        public DbSet<PlayerTask>      PlayerTasks      { get; set; }

        public BunkerDbContext(DbContextOptions<BunkerDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.ConfigureWarnings(x => x.Throw(RelationalEventId.QueryClientEvaluationWarning));
#endif
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChallangeTeam>()
                        .HasKey(x => new {x.ChallangeId, x.TeamId});

            modelBuilder.Entity<CompanyPlayer>()
                        .HasKey(x => new {x.CompanyId, x.PlayerId});

            modelBuilder.Entity<PlayerTeam>()
                        .HasKey(x => new {x.PlayerId, x.TeamId});

            modelBuilder.Entity<PlayerTask>()
                        .HasKey(x => new {x.PlayerId, x.TaskId});

            modelBuilder.Entity<PlayerRole>()
                        .HasKey(x => new {x.PlayerId, x.RoleId});

            modelBuilder.Entity<Player>()
                        .HasIndex(x => x.Email)
                        .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}