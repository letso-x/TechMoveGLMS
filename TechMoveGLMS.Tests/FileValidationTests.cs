using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.IO;

// file validation tests - making sure only pdfs get through
public class FileValidationTests
{
    private bool IsValidPdf(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return false;
        var ext = Path.GetExtension(fileName);
        return ext.ToLower() == ".pdf";
    }

    // this mocks a fake uploaded file, i found this on stackoverflow
    private Mock<IFormFile> CreateMockFile(string fileName)
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns(fileName);
        return mockFile;
    }

    [Fact]
    public void CheckPdfIsAllowed()
    {
        // normal pdf should pass
        var mockFile = CreateMockFile("contract.pdf");
        var result = IsValidPdf(mockFile.Object.FileName);
        Assert.True(result);
    }

    [Fact]
    public void TestBadFileGetsRejected()
    {
        // exe files should be blocked
        var mockFile = CreateMockFile("virus.exe");
        var result = IsValidPdf(mockFile.Object.FileName);
        Assert.False(result);
    }

    [Fact]
    public void TestDocxGetsRejected()
    {
        var mockFile = CreateMockFile("contract.docx");
        var result = IsValidPdf(mockFile.Object.FileName);
        Assert.False(result);
    }

    [Fact]
    public void TestNullFileGetsRejected()
    {
        // make sure null doesnt crash everything
        var result = IsValidPdf(null);
        Assert.False(result);
    }

    [Fact]
    public void TestEmptyStringGetsRejected()
    {
        // empty string should also fail
        var result = IsValidPdf("");
        Assert.False(result);
    }

    [Fact]
    public void TestJpgGetsRejected()
    {
        var mockFile = CreateMockFile("photo.jpg");
        var result = IsValidPdf(mockFile.Object.FileName);
        Assert.False(result);
    }

    [Fact]
    public void TestUppercasePdfWorks()
    {
        // not sure if uppercase extension works, testing just in case
        var mockFile = CreateMockFile("CONTRACT.PDF");
        var result = IsValidPdf(mockFile.Object.FileName);
        Assert.True(result);
    }

    [Fact]
    public void TestZipGetsRejected()
    {
        var mockFile = CreateMockFile("files.zip");
        var result = IsValidPdf(mockFile.Object.FileName);
        Assert.False(result);
    }
}