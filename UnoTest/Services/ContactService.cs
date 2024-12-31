using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace UnoTest.Services;

public class ContactService
{
    private static readonly HttpClient _httpClient;
#if __ANDROID__
    private readonly string _url = "https://10.0.2.2:5901/";
#else
    private readonly string _url = "https://localhost:5901/";
#endif

    static ContactService()
    {
#if __WASM__
        var innerHandler = new Uno.UI.Wasm.WasmHttpHandler();
#else
#if DEBUG
        var innerHandler = GetInsecureHandler();
#else
        var innerHandler = new HttpClientHandler();
#endif   
#endif
        _httpClient = new HttpClient(innerHandler);
    }
    static HttpClientHandler GetInsecureHandler()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            if (cert is not null && cert.Issuer.Equals("CN=localhost"))
                return true;
            return errors == System.Net.Security.SslPolicyErrors.None;
        };
        return handler;
    }

    public async Task<Contact> GetContactAsync(int contactId)
    {
        var url = $"{_url}contacts/{contactId}";
        return await ExecuteGetAsync<Contact>(url);
    }

    public async Task<List<Contact>> GetContactsAsync()
    {
        var url = $"{_url}contacts";
        return await ExecuteGetAsync<List<Contact>>(url);
    }

    public async Task<List<Contact>> GetContactsAsync(string firstName, string lastName)
    {
        var url = $"{_url}contacts/search?firstname={firstName}&lastname={lastName}";
        return await ExecuteGetAsync<List<Contact>>(url);
    }

    public async Task<Contact?> SaveContactAsync(Contact contact)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var url = $"{_url}contacts/";
        var jsonRequest = JsonSerializer.Serialize(contact);
        var jsonContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, jsonContent);

        if (response.StatusCode != HttpStatusCode.Created)
            throw new HttpRequestException(
                $"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        if (content is null)
        {
            throw new ApplicationException("Failed to save the contact");
        }
        return JsonSerializer.Deserialize<Contact>(content, options);
    }

    public async Task<bool> DeleteContactAsync(Contact contact)
    {
        return await DeleteContactAsync(contact.ContactId);
    }

    public async Task<bool> DeleteContactAsync(int contactId)
    {
        var url = $"{_url}contacts/{contactId}";
        var response = await _httpClient.DeleteAsync(url);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<Phone> GetContactPhoneAsync(int contactId, int phoneId)
    {
        var url = $"{_url}contacts/{contactId}/phones/{phoneId}";
        return await ExecuteGetAsync<Phone>(url);
    }

    public async Task<List<Phone>> GetContactPhonesAsync(int contactId)
    {
        var url = $"{_url}contacts/{contactId}/phones";
        return await ExecuteGetAsync<List<Phone>>(url);
    }

    public async Task<Address> GetContactAddressAsync(int contactId, int addressId)
    {
        var url = $"{_url}contacts/{contactId}/addresses/{addressId}";
        return await ExecuteGetAsync<Address>(url);
    }

    public async Task<List<Address>> GetContactAddressesAsync(int contactId)
    {
        var url = $"{_url}contacts/{contactId}/addresses";
        return await ExecuteGetAsync<List<Address>>(url);
    }

    private async Task<T> ExecuteGetAsync<T>(string url)
    {
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await _httpClient.GetAsync(url);
        if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpRequestException(
                $"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

        // Parse the Results
        var content = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var results = JsonSerializer.Deserialize<T>(content, options);

        if (results is null)
        {
            throw new HttpRequestException("Item was not found");
        }
        return results;
    }
}