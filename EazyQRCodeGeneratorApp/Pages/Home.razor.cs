using EazyQRCodeGeneratorApp.Enums;
using QRCoder;
using System.Drawing;
using static QRCoder.PayloadGenerator;

namespace EazyQRCodeGeneratorApp.Pages
{
    public partial class Home
    {
        private OptionType SelectedOption { get; set; } = OptionType.URL;

        private string SourceUrl { get; set; } = string.Empty;
        public string? WiFiName { get; set; }
        public string? WiFiPassword { get; set; }
        public string? PixKey { get; set; }
        public string? PixBeneficiary { get; set; }
        public string? PixCity { get; set; }


        private byte[]? QrCodeByte { get; set; }
        private string? QrCodeBase64 { get; set; }

        private bool DisableButtonGenerate => EnableButton();

        private string GeneratePayloadQrCodeFromUrl()
        {
            if (string.IsNullOrWhiteSpace(SourceUrl))
                return string.Empty;

            return SourceUrl;
        }
        private string GeneratePayloadQrCodePix()
        {
            if (string.IsNullOrWhiteSpace(PixKey)
                || string.IsNullOrWhiteSpace(PixBeneficiary)
                || string.IsNullOrWhiteSpace(PixCity))
                return string.Empty;

            return $"00020126360014BR.GOV.BCB.PIX0114{PixKey}5204000053039865802BR5910{PixBeneficiary.PadRight(10)[..10]}6009{PixCity.PadRight(9)[..9]}"; ;
        }

        private string GeneratePayloadQrCodeToWiFi()
        {
            if (string.IsNullOrWhiteSpace(WiFiName) || string.IsNullOrWhiteSpace(WiFiPassword))
                return string.Empty;

            WiFi wifiPayload = new(ssid: WiFiName, password: WiFiPassword, WiFi.Authentication.WPA);

            return wifiPayload.ToString();
        }

        private void GenerateQrCode()
        {
            string payload = string.Empty;
            switch (SelectedOption)
            {
                case OptionType.URL:
                    payload = GeneratePayloadQrCodeFromUrl();
                    break;
                case OptionType.PIX:
                    payload = GeneratePayloadQrCodePix();
                    break;
                case OptionType.WiFi:
                    payload = GeneratePayloadQrCodeToWiFi();
                    break;
                default:
                    break;
            }

            using QRCodeGenerator qrGenerator = new();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new(qrCodeData);
            QrCodeByte = qrCode.GetGraphic(25, Color.Black, Color.White);

            QrCodeBase64 = $"data:image/png;base64,{Convert.ToBase64String(QrCodeByte)}";
        }

        private void OnOptionSelected(OptionType type)
        {
            SelectedOption = type;

            SourceUrl = string.Empty;
            QrCodeBase64 = null;
            QrCodeByte = null;
            WiFiName = string.Empty;
            WiFiPassword = string.Empty;
        }

        private bool EnableButton()
        {
            return string.IsNullOrWhiteSpace(SourceUrl) &&
                (string.IsNullOrWhiteSpace(WiFiName) || string.IsNullOrWhiteSpace(WiFiPassword)) &&
                (string.IsNullOrWhiteSpace(PixKey) || string.IsNullOrWhiteSpace(PixBeneficiary) || string.IsNullOrWhiteSpace(PixCity));
        }
    }
}
