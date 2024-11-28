public class ImageRepository : IImageRepository
{
    private readonly ImageDbContext _dbContext;
    public ImageRepository(ImageDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public string GetImageById(int id)
    {
        if (id < 1)
            throw new ArgumentException("Id is invalid");
        var dbImage = _dbContext?.Images?.FirstOrDefault(x => x.Id == id);

        if (dbImage == null)
            throw new ArgumentException("Image not found in the DB");

        return dbImage.Url;

    }
}

