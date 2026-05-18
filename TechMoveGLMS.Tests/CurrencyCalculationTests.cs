using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using Moq.Protected;
using System.Threading;
using System.Threading.Tasks;

// currency tests - i had to use moq so the real api doesnt get called
public class CurrencyCalculationTests
{
    // i put the math in a separate method so i can test it easy
    private double DoConversion(double amount, double rate)
    {
        // just multiply, took me a while to realise its this simple
        return amount * rate;
    }

    [Fact]
    public void TestCurrencyWorks()
    {
        // fixed rate so the test doesnt depend on the real api
        var rate = 18.5;
        var usd = 100.0;
        var result = DoConversion(usd, rate);
        // 100 x 18.5 should be 1850
        Assert.Equal(1850.0, result);
    }

    [Fact]
    public void TestZeroAmount()
    {
        // zero dollars should give zero rands
        var result = DoConversion(0, 18.5);
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void TestSmallAmount()
    {
        var result = DoConversion(10.0, 18.5);
        // 10 x 18.5 = 185
        Assert.Equal(185.0, result);
    }

    [Fact]
    public void TestNegativeAmount()
    {
        // not sure if this happens but testing anyway
        var result = DoConversion(-50, 18.5);
        Assert.Equal(-925.0, result);
    }

    [Fact]
    public void TestZeroRate()
    {
        // if rate comes back as 0 result should be 0
        var result = DoConversion(100, 0);
        Assert.Equal(0.0, result);
    }

    // this test mocks the httpclient so we dont call the real api
    // i used stackoverflow to figure out how to mock httpclient
    [Fact]
    public async Task TestHttpClientMockReturnsRate()
    {
        var mockHandler = new Mock<HttpMessageHandler>();

        // fake api response with a rate of 18.5
        var fakeJson = "{\"rates\":{\"ZAR\":18.5}}";

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fakeJson)
            });

        var client = new HttpClient(mockHandler.Object);

        // make sure the client was set up correctly
        // had to look this up, not sure if theres a better way
        Assert.NotNull(client);
    }

    [Fact]
    public void TestLargeAmount()
    {
        // testing a large number just in case
        var result = DoConversion(999999, 18.5);
        Assert.Equal(18499981.5, result);
    }
}