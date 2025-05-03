using Microsoft.EntityFrameworkCore;
using XelerationTask.Core.Models;

namespace XelerationTask.Infastructure.Persistence
{
    public class FileSystemDbContext : DbContext
    {

        public FileSystemDbContext(DbContextOptions<FileSystemDbContext> options)
            : base(options)
        {

        }

        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<ProjectFolder> ProjectFolders { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProjectFile>()
                        .HasKey(p => p.Id);

            modelBuilder.Entity<ProjectFolder>()
                        .HasKey(p => p.Id);

            modelBuilder.Entity<User>()
                        .HasKey(p => p.Id);


            modelBuilder.Entity<ProjectFile>()
                .HasOne(p => p.ParentFolder)  
                .WithMany(p => p.Files)  
                .HasForeignKey(p => p.ParentFolderId)  
                .IsRequired();

            modelBuilder.Entity<ProjectFolder>()
                .HasOne(p => p.ParentFolder)
                .WithMany(p => p.SubFolders)
                .HasForeignKey(p => p.ParentFolderId);

            modelBuilder.Entity<User>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<ProjectFolder>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<ProjectFile>().HasQueryFilter(e => !e.IsDeleted);


        }
    }
}