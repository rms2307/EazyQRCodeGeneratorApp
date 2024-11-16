using QRCoder;
using System.Drawing;

namespace EazyQRCodeGeneratorApp.Pages
{
    public partial class Home
    {
        private string SourceUrl { get; set; } = string.Empty;
        private string? QrCodeBase64 { get; set; }
        private int Size { get; set; } = 15;

        private readonly string ImageType = "data:image/png;base64, ";

        private void GenerateQrCode()
        {
            using QRCodeGenerator qrGenerator = new();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(SourceUrl, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(Size, Color.Black, Color.White);

            QrCodeBase64 = $"{ImageType}{Convert.ToBase64String(qrCodeImage)}";
        }
    }
}
