using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

//Load existing PDF document.
using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument(Path.GetFullPath(@"Data/pdf-succinctly.pdf")))
{
    //Load digital ID with password.
    FileStream certificateStream = new FileStream(Path.GetFullPath(@"Data/PDF.pfx"), FileMode.Open, FileAccess.Read);
    PdfCertificate pdfCert = new PdfCertificate(certificateStream, "syncfusion");

    //Create a signature with loaded digital ID.
    PdfSignature signature = new PdfSignature(loadedDocument, loadedDocument.Pages[0], pdfCert, "Signature");

    //Change the digital signature standard and hashing algorithm.
    PdfSignatureSettings settings = signature.Settings;
    settings.CryptographicStandard = CryptographicStandard.CADES;
    signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA512;

    //Save the PDF document
    loadedDocument.Save(Path.GetFullPath(@"Output/Output.pdf"));

    //Close the document
    loadedDocument.Close(true);
}