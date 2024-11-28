using Microsoft.EntityFrameworkCore;

namespace PPT.Tests;

/*
These tests are simulating requests from the index.html page.
In order to run these tests, please have the API running in the background
*/
public class ImageFetcherServiceTests
{
    private HttpClient? _httpClient;
    private string requestUrl = "https://localhost:7181/avatar?userIdentifier=";

    [SetUp]
    public void Setup()
    {
        //Frontend URL
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:5500")
        };
    }
    [TearDown]
    public void TearDown()
    {
        _httpClient?.Dispose();
    }


    [TestCase("12345")]
    [TestCase("fsddsdsd3")]
    public async Task ImageFetcherServiceTest_request_one_to_five_returns_DB_image(string userIdentifier)
    {
        string expectedUrl = "https://api.dicebear.com/9.x/avataaars/svg?seed=";
        if (_httpClient != null)
        {
            var response = await _httpClient.GetAsync($"{requestUrl}{userIdentifier}");

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();

                Assert.That(content.Contains(expectedUrl), Is.True);
            }
            else
            {
                Assert.Fail("Failed to fetch image");
            }

        }
        else
        {
            Assert.Fail("Failed to acquire client");
        }
    }

    [TestCase("6789")]
    [TestCase("sdsddd6")]
    [TestCase("drfg16")]
    public async Task ImageFetcherServiceTest_request_six_to_nine_returns_my_json_server_image(string userIdentifier)
    {
        if (_httpClient != null)
        {
            var response = await _httpClient.GetAsync($"{requestUrl}{userIdentifier}");

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();

                Assert.That(content.Contains($"https://api.dicebear.com/8.x/pixel-art/png?seed={userIdentifier[^1] - '0'}"), Is.True);
            }
            else
            {
                Assert.Fail("Failed to fetch image");
            }

        }
        else
        {
            Assert.Fail("Failed to acquire client");
        }
    }
    [TestCase("a")]
    [TestCase("aaaaaaaaaaa")]
    public async Task ImageFetcherServiceTest_request_vowel_returns_dicebear_image(string userIdentifier)
    {
        if (_httpClient != null)
        {
            var response = await _httpClient.GetAsync($"{requestUrl}{userIdentifier}");

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();

                Assert.That(content.Contains("https://api.dicebear.com/8.x/pixel-art/png?seed="), Is.True);
            }
            else
            {
                Assert.Fail("Failed to fetch image");
            }

        }
        else
        {
            Assert.Fail("Failed to acquire client");
        }
    }
    [TestCase("dsds%fdfd")]
    [TestCase("*")]
    public async Task ImageFetcherServiceTest_request_non_alphanumeric_returns_dicebear_with_random_number_image(string userIdentifier)
    {
        if (_httpClient != null)
        {
            var response = await _httpClient.GetAsync($"{requestUrl}{userIdentifier}");

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();

                Assert.That(content.Contains("https://api.dicebear.com/8.x/pixel-art/png?seed="), Is.True);
            }
            else
            {
                Assert.Fail("Failed to fetch image");
            }

        }
        else
        {
            Assert.Fail("Failed to acquire client");
        }
    }
    [TestCase("dsdsfsfdsdffg")]
    public async Task ImageFetcherServiceTest_request_consonants_returns_dicebear_with_default_image(string userIdentifier)
    {
        if (_httpClient != null)
        {
            var response = await _httpClient.GetAsync($"{requestUrl}{userIdentifier}");

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();

                Assert.That(content.Contains("https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150"), Is.True);
            }
            else
            {
                Assert.Fail("Failed to fetch image");
            }

        }
        else
        {
            Assert.Fail("Failed to acquire client");
        }
    }
}