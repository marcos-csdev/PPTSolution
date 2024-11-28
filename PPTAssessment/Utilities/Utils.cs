public static class Utils
{
    public async static Task<string> MakeRequestAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("invalid request URL");

        var client = new HttpClient();
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode || response.Content == null)
            throw new Exception("Failed to acquire response from the server");

        var content = await response.Content.ReadFromJsonAsync<Image>();

        if (content == null || string.IsNullOrWhiteSpace(content.Url))
            throw new Exception("Failed to acquire response from the server");

        return content.Url;
    }
}