using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Entities;
using Motto.Services;
using Motto.Repositories;
using Motto.Utils;
using System.IO;
using System.Threading.Tasks;

namespace Motto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseImageController : ControllerBase
    {
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly ILicenseImageService _licenseImageService;

        public LicenseImageController(IDeliveryDriverRepository deliveryDriverRepository, ILicenseImageService licenseImageService)
        {
            _deliveryDriverRepository = deliveryDriverRepository;
            _licenseImageService = licenseImageService;
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Roles = "DeliveryDriver")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("A imagem não foi enviada.");
            }

            // Verificar o tipo MIME do arquivo
            var mimeType = ImageUtils.GetMimeType(image.FileName);
            if (mimeType != "image/png" && mimeType != "image/bmp")
            {
                return BadRequest("O formato do arquivo deve ser PNG ou BMP.");
            }

            var driver = await _deliveryDriverRepository.GetByIdAsync(id);
            if (driver == null)
            {
                return NotFound("Entregador não encontrado.");
            }

            try
            {
                // Salvar a imagem no MinIO
                var fileName = $"DeliveryDriverImage_{driver.Id}{Path.GetExtension(image.FileName)}";
                var imagePath = await _licenseImageService.UploadImageAsync(image, fileName);

                // Atualizar o campo DriverLicenseImage do DeliveryDriver
                driver.DriverLicenseImage = imagePath;
                await _deliveryDriverRepository.UpdateAsync(driver);

                return Ok("Imagem enviada e atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao fazer o upload da imagem: {ex.Message}");
            }
        }

        [HttpGet("{id}/image")]
        [Authorize(Roles = "Admin, DeliveryDriver")]
        public async Task<IActionResult> GetImage(int id)
        {
            var deliveryDriver = await _deliveryDriverRepository.GetByIdAsync(id);
            if (deliveryDriver == null)
            {
                return NotFound();
            }

            try
            {
                var imageStream = await _licenseImageService.GetImageAsync(deliveryDriver.DriverLicenseImage);
                return File(imageStream.ToArray(), "image/png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar a imagem.");
            }
        }
    }
}
