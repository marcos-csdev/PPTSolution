using Microsoft.EntityFrameworkCore;

public class ImageDbContext(DbContextOptions<ImageDbContext> options) : DbContext(options)
{
    public DbSet<Image>? Images { get; set; }
}