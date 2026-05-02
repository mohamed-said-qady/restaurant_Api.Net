using Microsoft.Extensions.DependencyInjection; // «·”ÿ— œÂ ÂÊ «··Ì ÂÌÕ· «·√“„…
using restaurant.Data; // ⁄œ· Õ”» «·‹ Namespace ⁄‰œﬂ
using restaurant.Helper;
using restaurant.Model;
using RestaurantSystem.IntegrationTests.Helpers;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
namespace RestaurantSystem.IntegrationTests;

public class RestaurantApiTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    // 1. ·«“„ ‰⁄—› «·„ €Ì— œÂ Â‰« ⁄‘«‰ «·„ÌÀÊœ“ «··Ì  Õ   ‘Ê›Â
    private readonly TestingWebAppFactory<Program> _factory;

    public RestaurantApiTests(TestingWebAppFactory<Program> factory)
    {
        _factory = factory; // 2. ·«“„  Œ“‰ «·‰”Œ… «··Ì Ã«Ì… ·ﬂ Â‰«
        _client = factory.CreateClient();
    }
    #region AuthenticateAsync
    // --- „ÌÀÊœ «·‹ Auth «·„”«⁄œ… ---
    private async Task AuthenticateAsync()
    {
        _client.DefaultRequestHeaders.Authorization = null;

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            Username = "admin@restaurant.com", //  √ﬂœ ≈‰ «·«”„ „ÿ«»ﬁ ··‹ DTO
            Password = "Admin@123"
        });

        // Â‰« Â‰ﬁ—√ «·‹ ServiceResult «··Ì —«Ã⁄
        var result = await response.Content.ReadFromJsonAsync<ServiceResult<string>>();

        if (result == null || !result.IsSuccess)
        {
            throw new Exception($"Login failed! Message: {result?.Message}");
        }

        // «· Êﬂ‰ „ÊÃÊœ ÃÊÂ «·‹ Data
        var token = result.Data;

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
    #endregion

    [Fact]
    public async Task UpdateMenuItem_ExistingId_ReturnsSuccess()
    {
        // 1. Arrange: €Ì—‰« «·‰Ê⁄ Â‰« ·ÌﬂÊ‰ int »œ· Guid
        int itemId;

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var item = new MenuItem { Name = "»—Ã— ﬁœÌ„", Price = 100.0m, Description = "Ê’› ﬁœÌ„" };
            db.MenuItems.Add(item);
            await db.SaveChangesAsync();
            itemId = item.Id; // œ·Êﬁ Ì «·‹ int ÂÌ—ﬂ» ⁄·Ï «·‹ int »œÊ‰ √Œÿ«¡
        }

        // 2. Act
        await AuthenticateAsync();
        var updatedItem = new { Name = "»—Ã— „‘ÊÌ ⁄ «·›Õ„", Price = 140.0 };

        // »‰»⁄  «·‹ itemId «··Ì ÂÊ int œ·Êﬁ Ì
        var response = await _client.PutAsJsonAsync($"/api/MenuItem/{itemId}", updatedItem);

        // 3. Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    //[Fact]
    //public async Task CreateOrder_ReturnsCorrectDataFromDb()
    //{
    //    // 1. Arrange
    //    await AuthenticateAsync();
    //    var orderToCreate = new { CustomerNotes = "»œÊ‰ ‘ÿ…", DeliveryAddress = "«·ﬁ«Â—…" };

    //    // 2. Act
    //    var response = await _client.PostAsJsonAsync("/api/Order", orderToCreate);

    //    // 3. Assert & Read
    //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    //    // Â‰« »ﬁÏ »‰ﬁ—√ "«·ÕﬁÌﬁ…" «··Ì « ”Ã· 
    //    var createdOrder = await response.Content.ReadFromJsonAsync<OrderDto>();

    //    // ‰√ﬂœ ≈‰ «·œ« « «··Ì —Ã⁄  ÂÌ ÂÌ «··Ì »⁄ ‰«Â«
    //    Assert.NotNull(createdOrder);
    //    Assert.NotEqual(0, createdOrder.Id); // ‰√ﬂœ ≈‰ «·‹ ID « ﬂ—Ì  „‘ »‹ 0
    //    Assert.Equal(orderToCreate.CustomerNotes, createdOrder.CustomerNotes);
    //    Assert.Equal("Pending", createdOrder.Status); // ‰√ﬂœ ≈‰ «·”Ì—›Ì” Õÿ  «·Õ«·… «·«› —«÷Ì… ’Õ
    //}
    //1.  ” : „”„ÊÕ »«·œŒÊ·(Authorized)
    [Fact]
    public async Task GetMenu_WithAuth_ReturnsSuccess()
    {
        await AuthenticateAsync();
        //ARRANGE
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var item = new MenuItem { Name = "Pizza Margherita", Price = 150 };
            db.MenuItems.Add(item);
            await db.SaveChangesAsync();
        }

        //ACT
        var response = await _client.GetAsync("/api/MenuItem");
        var result = await response.Content.ReadFromJsonAsync<ServiceResult<IEnumerable<MenuItem>>>();
        //ASSERT

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Contains(result.Data,a => a.Name == "Pizza Margherita");
    }

    // 2.  ” : „„‰Ê⁄ „‰ «·œŒÊ· (Unauthorized)
    [Fact]
    public async Task GetMenu_WithoutAuth_ReturnsUnauthorized()
    {
        _client.DefaultRequestHeaders.Authorization = null; // ‰‘Ì· «· Êﬂ‰
        var response = await _client.GetAsync("/api/MenuItem");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // 3.  ” : ≈÷«›… œ« « ’ÕÌÕ…
    [Fact]
    public async Task AddMenuItem_ValidData_ReturnsCreated()
    {
        await AuthenticateAsync();
        var newItem = new { Name = "Pizza", Price = 200 };
        var response = await _client.PostAsJsonAsync("/api/MenuItem", newItem);

        // ·Ê «·‹ API ⁄‰œﬂ »Ì—Ã⁄ 201 Created √Ê 200 OK
        Assert.True(response.IsSuccessStatusCode);
    }

    // 4.  ” : ≈÷«›… œ« « €·ÿ (”⁄— ”«·» „À·«)
    [Fact]
    public async Task AddMenuItem_InvalidData_ReturnsBadRequest()
    {
        await AuthenticateAsync();
        var badItem = new { Name = "", Price = -50 }; // œ« «  ÷—» «·‹ Validation
        var response = await _client.PostAsJsonAsync("/api/MenuItem", badItem);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

// ﬂ·«” »”Ìÿ ⁄‘«‰ Ìﬁ—√ «· Êﬂ‰ «··Ì —«Ã⁄
public class LoginResult
{
    public string Token { get; set; } = "";
}