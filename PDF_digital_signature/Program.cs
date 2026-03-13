
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Graphics;


namespace PDF_digital_signature
{
    class Program
    {
        static void Main(string[] args)
        {
            //Creates a new PDF document.
            using (PdfDocument document = new PdfDocument())
            {
                //Adds a new page.
                PdfPageBase page = document.Pages.Add();

                //Create graphics for the page. 
                PdfGraphics graphics = page.Graphics;

                //Creates a certificate instance from PFX file with private key.
                FileStream certificateStream = new FileStream(Path.GetFullPath(@"Data/PDF.pfx"), FileMode.Open, FileAccess.Read);
                PdfCertificate pdfCert = new PdfCertificate(certificateStream, "syncfusion");

                //Creates a digital signature.
                PdfSignature signature = new PdfSignature(document, page, pdfCert, "Signature");

                //Sets an image for signature field.
                FileStream imageStream = new FileStream(Path.GetFullPath(@"Data/signature.png"), FileMode.Open, FileAccess.Read);

                //Sets an image for signature field.
                PdfBitmap signatureImage = new PdfBitmap(imageStream);

                //Sets signature information.
                signature.Bounds = new RectangleF(new PointF(0, 0), new SizeF(100, 100));
                signature.SignedName = "Syncfusion";
                signature.ContactInfo = "johndoe@owned.us";
                signature.LocationInfo = "Honolulu, Hawaii";
                signature.Reason = "I am author of this document.";

                //Draw the image in signature appearance. 
                signature.Appearance.Normal.Graphics.DrawImage(signatureImage, new RectangleF(0, 0, 100, 100));

                //Save the PDF document
                document.Save(Path.GetFullPath(@"Output/Output.pdf"));

            }

        }
    }
}