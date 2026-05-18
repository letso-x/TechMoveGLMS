using Xunit;
using Moq;

// file validation tests - making sure only pdfs get through
public class FileValidationTests
{
    // i pulled this logic out of the controller to test it
    private bool IsValidPdf(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return false;
        var ext = Path.GetExtension(fileName);
        return ext.ToLower() == ".pdf";
    }

    [Fact]
    public void CheckPdfIsAllowed()
    {
        // normal pdf should pass
        var result = IsValidPdf("contract.pdf");
        Assert.True(result);
    }

    [Fact]
    public void TestBadFileGetsRejected()
    {
        // exe files should be blocked
        var result = IsValidPdf("virus.exe");
        Assert.False(result);
    }

    [Fact]
    public void TestDocxGetsRejected()
    {
        var result = IsValidPdf("contract.docx");
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
        var result = IsValidPdf("photo.jpg");
        Assert.False(result);
    }

    [Fact]
    public void TestUppercasePdfWorks()
    {
        // not sure if uppercase extension works, testing just in case
        var result = IsValidPdf("CONTRACT.PDF");
        Assert.True(result);
    }

    [Fact]
    public void TestZipGetsRejected()
    {
        var result = IsValidPdf("files.zip");
        Assert.False(result);
    }
}