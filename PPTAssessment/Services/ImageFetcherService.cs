using Serilog;
public class ImageFetcherService : IImageFetcherService
{
    private readonly IImageRepository _imageRepository;
    public ImageFetcherService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }
    public string FetchImage(string userIdentifier)
    {
        try
        {
            var avatarUrl = GetImageUrl(userIdentifier);

            return avatarUrl;
        }
        catch (Exception ex)
        {
            Log.Error($"Error generating avatar: {ex.Message}");

        }
        return null!;
    }

    public string GetImageUrl(string userIdentifier)
    {
        var diceBearBaseUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed=";

        //Check if it contains a vowel
        char? vowelFound = userIdentifier.FirstOrDefault(c => "aeiou".Contains(char.ToLower(c)));

        //checks if the user identifier contains a vowel is to be checked first
        if (vowelFound != '\0')
            return $"{diceBearBaseUrl}{vowelFound}&size=150";

        //the non-alphanumeric condition 
        else if (userIdentifier.Any(c => !char.IsLetterOrDigit(c)))
            return $"{diceBearBaseUrl}{new Random().Next(1, 6)}&size=150";

        //checks if the last character is within the range of 6-9
        else if ("6789".Contains(userIdentifier[^1]))
            return Utils.MakeRequestAsync($"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{userIdentifier[^1] - '0'}").Result;

        //checks if the last character is within the range of 1-5
        else if ("12345".Contains(userIdentifier[^1]))
            //convert into an int from raw value
            return _imageRepository.GetImageById(userIdentifier[^1] - '0');

        else
            return $"{diceBearBaseUrl}default&size=150";



    }

}