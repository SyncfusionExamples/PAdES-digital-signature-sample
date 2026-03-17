
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;


//Load a PDF document.
using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument(Path.GetFullPath(@"Data/pdf-succinctly.pdf")))
{
    //Creates a certificate instance from PFX file with private key.
    FileStream certificateStream = new FileStream(Path.GetFullPath(@"Data/PDF.pfx"), FileMode.Open, FileAccess.Read);
    PdfCertificate pdfCert = new PdfCertificate(certificateStream, "syncfusion");

    //Creates a digital signature.
    PdfSignature signature = new PdfSignature(loadedDocument, loadedDocument.Pages[0], pdfCert, "Signature");

    //Sets signature settings to customize cryptographic standard specified.
    PdfSignatureSettings settings = signature.Settings;
    settings.CryptographicStandard = CryptographicStandard.CADES;
    signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;
    
    //Adds time stamp by using the server URI and credentials.
    signature.TimeStampServer = new TimeStampServer(new Uri("http://time.certum.pl/"));

    // Save the PDF document
    loadedDocument.Save(Path.GetFullPath(@"Output/Output.pdf"));

    //Close the document
    loadedDocument.Close();
}

