using Microsoft.AspNetCore.Mvc;
using QRCodeGen.Models;
using QRCodeGen.Services;

namespace QRCodeGen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrController(QrService qrService) : ControllerBase
    {
        // qr comes from the name -controller
        // POST api/qr/generate-qrcode
        [HttpPost("generate-qrcode")]
        public IActionResult GenerateQrCode([FromBody] QRCode request)
        {
            if (request == null || string.IsNullOrEmpty(request.Data))
            {
                return BadRequest("No data provided.");
            }

            try
            {
                // Call the service to generate the QR code
                var qrCodeImage = qrService.GenerateQrCode(request.Data);

                // Return the QR code image as a PNG file
                return File(qrCodeImage, "image/png");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return error if data is invalid
            }
        }
    }
}