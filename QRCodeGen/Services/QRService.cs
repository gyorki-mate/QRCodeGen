using QRCoder;

namespace QRCodeGen.Services
{
    public class QrService
    {
        public byte[] GenerateQrCode(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException("No data provided.");

            // Generate the QR code
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);  // Return the image as byte[]
        }
    }
}