using QRCoder;
using System.Drawing;

namespace EazyQRCodeGeneratorApp.Pages
{
    public partial class Home
    {
        private string SourceUrl { get; set; } = string.Empty;
        private byte[]? QrCodeByte { get; set; }
        private string? QrCodeBase64 { get; set; }

        private readonly string ImageType = "data:image/png;base64,";

        private void GenerateQrCode()
        {
            using QRCodeGenerator qrGenerator = new();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(SourceUrl, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new(qrCodeData);
            QrCodeByte = qrCode.GetGraphic(25, Color.Black, Color.White);

            QrCodeBase64 = $"{ImageType}{Convert.ToBase64String(QrCodeByte)}";
        }
    }
}
