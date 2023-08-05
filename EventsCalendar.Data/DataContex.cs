using EventsCalendar.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventsCalendar.Data;

public class DataContext : DbContext
{
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Category> Categories { get; set; } = null!;
	public DbSet<Note> Notes { get; set; } = null!;

	public DataContext(bool cleanDb = false) :base()
	{
		if (cleanDb)
			Database.EnsureDeleted();
		Database.EnsureCreated();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
	{
		var builder = new ConfigurationBuilder();
		
		builder.SetBasePath(Directory.GetCurrentDirectory());
		builder.AddJsonFile("appsettings.json");
		var config = builder.Build();

		// получение ст роки подключения
		string connectionString = config.GetConnectionString("DefaultConnection");

		optBuilder.UseSqlite(connectionString);
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Note>()
			.HasMany(n=>n.Categories)
			.WithMany()
			.UsingEntity(j => j.ToTable("NotesCategories"));
	}
}