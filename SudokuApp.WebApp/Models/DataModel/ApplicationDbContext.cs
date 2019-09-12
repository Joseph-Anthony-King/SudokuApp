using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using SudokuApp.Models;

namespace SudokuApp.WebApp.Models.DataModel {

    public class ApplicationDbContext : DbContext {
        
        private IConfiguration configuration;

        public ApplicationDbContext(
                DbContextOptions<ApplicationDbContext> options,
                IConfiguration iConfigService
            ) : base(options) {

            configuration = iConfigService;
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<SudokuCell> SudokuCells { get; set; }
        public DbSet<SudokuMatrix> SudokuMatrices { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<SudokuSolution> SudokuSolutions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseNpgsql(configuration.GetValue<string>("ConnectionStrings:DatabaseConnection"));

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            var intListConverter = new ValueConverter<List<int>, string>(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(val => int.Parse(val))
                    .ToList()
            );

            modelBuilder.ForNpgsqlUseIdentityColumns();

            modelBuilder.Entity<Role>()
                .HasKey(role => role.Id);

            modelBuilder.Entity<Difficulty>()
                .HasKey(difficulty => difficulty.Id);

            modelBuilder.Entity<Difficulty>()
                .HasMany(difficulty => difficulty.Matrices)
                .WithOne(matrix => matrix.Difficulty);

            modelBuilder.Entity<SudokuCell>()
                .HasKey(cell => cell.Id);

            modelBuilder.Entity<SudokuCell>()
                .Ignore(cell => cell.AvailableValues);

            modelBuilder.Entity<SudokuMatrix>()
                .HasKey(matrix => matrix.Id);
                
            modelBuilder.Entity<SudokuMatrix>()
                .HasMany(matrix => matrix.SudokuCells)
                .WithOne(cell => cell.SudokuMatrix);
                
            modelBuilder.Entity<SudokuMatrix>()
                .Ignore(matrix => matrix.Columns)
                .Ignore(matrix => matrix.Regions)
                .Ignore(matrix => matrix.Rows)
                .Ignore(matrix => matrix.FirstColumn)
                .Ignore(matrix => matrix.SecondColumn)
                .Ignore(matrix => matrix.ThirdColumn)
                .Ignore(matrix => matrix.FourthColumn)
                .Ignore(matrix => matrix.FifthColumn)
                .Ignore(matrix => matrix.SixthColumn)
                .Ignore(matrix => matrix.SeventhColumn)
                .Ignore(matrix => matrix.EighthColumn)
                .Ignore(matrix => matrix.NinthColumn)
                .Ignore(matrix => matrix.FirstRegion)
                .Ignore(matrix => matrix.SecondRegion)
                .Ignore(matrix => matrix.ThirdRegion)
                .Ignore(matrix => matrix.FourthRegion)
                .Ignore(matrix => matrix.FifthRegion)
                .Ignore(matrix => matrix.SixthRegion)
                .Ignore(matrix => matrix.SeventhRegion)
                .Ignore(matrix => matrix.EighthRegion)
                .Ignore(matrix => matrix.NinthRegion)
                .Ignore(matrix => matrix.FirstRow)
                .Ignore(matrix => matrix.SecondRow)
                .Ignore(matrix => matrix.ThirdRow)
                .Ignore(matrix => matrix.FourthRow)
                .Ignore(matrix => matrix.FifthRow)
                .Ignore(matrix => matrix.SixthRow)
                .Ignore(matrix => matrix.SeventhRow)
                .Ignore(matrix => matrix.EighthRow)
                .Ignore(matrix => matrix.NinthRow)
                .Ignore(matrix => matrix.FirstColumnValues)
                .Ignore(matrix => matrix.SecondColumnValues)
                .Ignore(matrix => matrix.ThirdColumnValues)
                .Ignore(matrix => matrix.FourthColumnValues)
                .Ignore(matrix => matrix.FifthColumnValues)
                .Ignore(matrix => matrix.SixthColumnValues)
                .Ignore(matrix => matrix.SeventhColumnValues)
                .Ignore(matrix => matrix.EighthColumnValues)
                .Ignore(matrix => matrix.NinthColumnValues)
                .Ignore(matrix => matrix.FirstRegionValues)
                .Ignore(matrix => matrix.SecondRegionValues)
                .Ignore(matrix => matrix.ThirdRegionValues)
                .Ignore(matrix => matrix.FourthRegionValues)
                .Ignore(matrix => matrix.FifthRegionValues)
                .Ignore(matrix => matrix.SixthRegionValues)
                .Ignore(matrix => matrix.SeventhRegionValues)
                .Ignore(matrix => matrix.EighthRegionValues)
                .Ignore(matrix => matrix.NinthRegionValues)
                .Ignore(matrix => matrix.FirstRowValues)
                .Ignore(matrix => matrix.SecondRowValues)
                .Ignore(matrix => matrix.ThirdRowValues)
                .Ignore(matrix => matrix.FourthRowValues)
                .Ignore(matrix => matrix.FifthRowValues)
                .Ignore(matrix => matrix.SixthRowValues)
                .Ignore(matrix => matrix.SeventhRowValues)
                .Ignore(matrix => matrix.EighthRowValues)
                .Ignore(matrix => matrix.NinthRowValues);

            modelBuilder.Entity<Game>()
                .HasKey(game => game.Id);
                
            modelBuilder.Entity<Game>()
                .HasOne(game => game.SudokuMatrix)
                .WithOne(matrix => matrix.Game);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.SudokuSolution)
                .WithOne(s => s.Game)
                .HasForeignKey<SudokuSolution>(s => s.GameId);
            
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);
            
            modelBuilder.Entity<User>()
                .HasMany(user => user.Games)
                .WithOne(game => game.User);

            modelBuilder.Entity<User>()
                .Property(user => user.UserName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(user => user.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(user => user.FirstName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.LastName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId});

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(user => user.Roles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<SudokuSolution>()
                .HasKey(solution => solution.Id);

            modelBuilder.Entity<SudokuSolution>()
                .Property(solution => solution.SolutionList)
                .HasConversion(intListConverter);

            modelBuilder.Entity<SudokuSolution>()
                .Ignore(solution => solution.FirstRow)
                .Ignore(solution => solution.SecondRow)
                .Ignore(solution => solution.ThirdRow)
                .Ignore(solution => solution.FourthRow)
                .Ignore(solution => solution.FifthRow)
                .Ignore(solution => solution.SixthRow)
                .Ignore(solution => solution.SeventhRow)
                .Ignore(solution => solution.EighthRow)
                .Ignore(solution => solution.NinthRow);
        }
    }
}
