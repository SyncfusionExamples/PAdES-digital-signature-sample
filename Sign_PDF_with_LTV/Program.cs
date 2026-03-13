using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;


namespace Sign_PDF_with_LTV
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load existing PDF document.  
            using (PdfDocument document = new PdfDocument())
            {
                //Adds a new page.
                PdfPageBase page = document.Pages.Add();
                //Create graphics for the page. 
                PdfGraphics graphics = page.Graphics;
                //Creates a certificate instance from PFX file with private key.
                FileStream certificateStream = new FileStream(Path.GetFullPath(@"Data/PDF.pfx"), FileMode.Open, FileAccess.Read);
                PdfCertificate pdfCert = new PdfCertificate(certificateStream, "DigitalPass123");
                //Creates a digital signature.
                PdfSignature signature = new PdfSignature(document, page, pdfCert, "Signature");
                //Adding the digital signature standard and hashing algorithm.
                signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
                signature.Settings.DigestAlgorithm = DigestAlgorithm.SHA256;
                //Add timestamp server link to the signature.
                signature.TimeStampServer = new TimeStampServer(new Uri("http://timestamp.digicert.com/"));
                //Create a new Memory Stream
                MemoryStream memoryStream = new MemoryStream();
                //Save the PDF document to memory.
                document.Save(memoryStream);
                //Close the original document.
                document.Close(true);
                //Reset stream position before re-loading.
                memoryStream.Position = 0;
                //Load the signed document to update LTV information.
                PdfLoadedDocument signedDocument = new PdfLoadedDocument(memoryStream);
                //Get the signed signature field (ensure it exists and is a signature field).
                if (signedDocument.Form != null && signedDocument.Form.Fields.Count > 0 && signedDocument.Form.Fields[0] is PdfLoadedSignatureField signatureField)
                {
                    //Update LTV information.
                    signatureField.Signature.EnableLtv = true;
                }
                //Save the PDF document to a file stream.
                using (FileStream output = File.Create("SignPDFWithLTV.pdf"))
                {
                    signedDocument.Save(output);
                }
                //Close the signed document.
                signedDocument.Close(true);
            }
        }
    }
}
