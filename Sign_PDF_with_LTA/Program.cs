
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf;

//Creates a new PDF document.
using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument(Path.GetFullPath(@"Data/pdf-succinctly.pdf")))
{
    //Load digital ID with password.
    FileStream documentStream2 = new FileStream(Path.GetFullPath(@"Data/DigitalSignatureTest.pfx"), FileMode.Open, FileAccess.Read);
    PdfCertificate certificate = new PdfCertificate(documentStream2, "DigitalPass123");
    //Create a signature with loaded digital ID.
    PdfSignature signature = new PdfSignature(loadedDocument, loadedDocument.Pages[0], certificate, "DigitalSignature");
    signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
    signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA256;

    //Adds time stamp by using the server URI and credentials.
    signature.TimeStampServer = new TimeStampServer(new Uri("http://timestamp.digicert.com/"));

    //Create a new Memory Stream
    MemoryStream memoryStream = new MemoryStream();
    //Save the PDF document to memory.
    loadedDocument.Save(memoryStream);

    //Load existing PDF document.
    using (PdfLoadedDocument ltDocument = new PdfLoadedDocument(memoryStream))
    {
        if (ltDocument.Form != null && ltDocument.Form.Fields.Count > 0 && ltDocument.Form.Fields[0] is PdfLoadedSignatureField signatureField)
        {
            //Update LTV information.
            signatureField.Signature.EnableLtv = true;
        }
        //Load the existing PDF page.
        PdfLoadedPage lpage = ltDocument.Pages[0] as PdfLoadedPage;
        //Create PDF signature with empty certificate.
        PdfSignature timeStamp = new PdfSignature(lpage, "timestamp");
        timeStamp.TimeStampServer = new TimeStampServer(new Uri("http://timestamp.digicert.com/"));
        //Save the PDF document.
        ltDocument.Save(Path.GetFullPath(@"Output/Output.pdf"));
    }
}

