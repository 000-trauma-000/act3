using ConstellationRestoreApi.Controllers;
using ConstellationRestoreApi.Dto;
using ConstellationRestoreApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace ConstellationRestoreApi.Tests;

public class ConstellationRestoreTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ConstellationRestoreTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Restore_ReturnsMissingSymbols()
    {
        var request = new ConstellationRestoreRequest { Reference = "DRAGON", Damaged = "GOD" };

        var response = await _client.PostAsJsonAsync("/api/constellations/restore", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ConstellationRestoreResponse>();

        body.ShouldNotBeNull();
        body.MissingSymbols.ShouldBe("ANR");
        body.IsComplete.ShouldBeFalse();
    }

    [Fact]
    public async Task Restore_WhenComplete_ReturnsEmptyAndTrue()
    {
        var request = new ConstellationRestoreRequest { Reference = "VEGA", Damaged = "GAVE" };

        var response = await _client.PostAsJsonAsync("/api/constellations/restore", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ConstellationRestoreResponse>();

        body.ShouldNotBeNull();
        body.MissingSymbols.ShouldBe(string.Empty);
        body.IsComplete.ShouldBeTrue();
    }

    [Fact]
    public async Task Restore_Partial_ReturnsSortedMissing()
    {
        var request = new ConstellationRestoreRequest { Reference = "LYRA", Damaged = "LRA" };

        var response = await _client.PostAsJsonAsync("/api/constellations/restore", request);

        var body = await response.Content.ReadFromJsonAsync<ConstellationRestoreResponse>();

        body.ShouldNotBeNull();
        body.MissingSymbols.ShouldBe("Y");
        body.IsComplete.ShouldBeFalse();
    }

    [Fact]
    public async Task Restore_EmptyDamaged_ReturnsAllReferenceSorted()
    {
        var request = new ConstellationRestoreRequest { Reference = "CYGNUS", Damaged = string.Empty };

        var response = await _client.PostAsJsonAsync("/api/constellations/restore", request);

        var body = await response.Content.ReadFromJsonAsync<ConstellationRestoreResponse>();

        body.ShouldNotBeNull();
        body.MissingSymbols.ShouldBe("CGNSUY");
        body.IsComplete.ShouldBeFalse();
    }

    [Fact]
    public async Task Restore_MissingReference_ReturnsBadRequest()
    {
        var request = new ConstellationRestoreRequest { Reference = null, Damaged = "GOD" };

        var response = await _client.PostAsJsonAsync("/api/constellations/restore", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Restore_DamagedContainsUnknownSymbol_ReturnsBadRequest()
    {
        var request = new ConstellationRestoreRequest { Reference = "DRAGON", Damaged = "GODX" };

        var response = await _client.PostAsJsonAsync("/api/constellations/restore", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Restore_ReferenceHasDuplicates_ReturnsBadRequest()
    {
        var request = new ConstellationRestoreRequest { Reference = "AAR", Damaged = "A" };

        var response = await _client.PostAsJsonAsync("/api/constellations/restore", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public void Service_ReturnsMissingForDragonGod()
    {
        var service = new ConstellationRestoreService();

        var response = service.Restore(new ConstellationRestoreRequest { Reference = "DRAGON", Damaged = "GOD" });

        response.MissingSymbols.ShouldBe("ANR");
        response.IsComplete.ShouldBeFalse();
    }

    [Fact]
    public void Service_ReturnsCompleteWhenAllPresent()
    {
        var service = new ConstellationRestoreService();

        var response = service.Restore(new ConstellationRestoreRequest { Reference = "VEGA", Damaged = "GAVE" });

        response.MissingSymbols.ShouldBe(string.Empty);
        response.IsComplete.ShouldBeTrue();
    }
}
public static class Should
{
    public static void ShouldBe<T>(this T actual, T expected)
    {
        if (!Equals(actual, expected))
        {
            throw new Exception($"Expected {expected}, got {actual}.");
        }
    }

    public static void ShouldBeTrue(this bool value)
    {
        if (!value)
        {
            throw new Exception($"Expected true, got false.");
        }
    }

    public static void ShouldBeFalse(this bool value)
    {
        if (value)
        {
            throw new Exception($"Expected false, got true.");
        }
    }

    public static void ShouldNotBeNull<T>(this T value) where T : class
    {
        if (value is null)
        {
            throw new Exception("Expected not null.");
        }
    }
}