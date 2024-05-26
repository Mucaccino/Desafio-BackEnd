using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Motto.Utils;
using Motto.Repositories.Interfaces;
using Motto.Services.Interfaces;

namespace Motto.Controllers
{
    /// <summary>
    /// Represents a controller for handling license image operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseImageController : ControllerBase
    {
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly ILicenseImageService _licenseImageService;
        private readonly ILogger<LicenseImageController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseImageController"/> class.
        /// </summary>
        /// <param name="deliveryDriverRepository">The repository for accessing delivery driver data.</param>
        /// <param name="licenseImageService">The service for handling license image operations.</param>
        /// <param name="logger">The logger.</param>
        public LicenseImageController(IDeliveryDriverRepository deliveryDriverRepository, ILicenseImageService licenseImageService, ILogger<LicenseImageController> logger)
        {
            _deliveryDriverRepository = deliveryDriverRepository;
            _licenseImageService = licenseImageService;
            _logger = logger;
        }

        /// <summary>
        /// Uploads an image for a delivery driver.
        /// </summary>
        /// <param name="id">The ID of the delivery driver.</param>
        /// <param name="image">The image file to upload.</param>
        /// <returns>A task that represents the asynchronous operation. Returns an IActionResult indicating the success or failure of the operation.</returns>
        /// <remarks>
        /// The image file must be in PNG or BMP format.
        /// The delivery driver must exist in the repository.
        /// If the image file is not provided, a BadRequest result is returned.
        /// If the image file is not in the correct format, a BadRequest result is returned.
        /// If the delivery driver is not found, a NotFound result is returned.
        /// If there is an error during the upload or update process, a StatusCode result with a 500 status code and an error message is returned.
        /// </remarks>
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

            var driver = await _deliveryDriverRepository.GetById(id);
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
                await _deliveryDriverRepository.Update(driver);

                return Ok("Imagem enviada e atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao fazer o upload da imagem: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the image of a delivery driver by their ID.
        /// </summary>
        /// <param name="id">The ID of the delivery driver.</param>
        /// <returns>
        /// An asynchronous task that returns an IActionResult. 
        /// If the delivery driver is found, returns the image file as a FileResult. 
        /// If the delivery driver is not found, returns a NotFoundResult. 
        /// If there is an error retrieving the image, returns a StatusCodeResult with a 500 status code and an error message.
        /// </returns>
        [HttpGet("{id}/image")]
        [Authorize(Roles = "Admin, DeliveryDriver")]
        public async Task<IActionResult> GetImage(int id)
        {
            var deliveryDriver = await _deliveryDriverRepository.GetById(id);
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
                _logger.LogError(ex, "Ocorreu um erro ao recuperar a imagem.");
                return StatusCode(500, "Ocorreu um erro ao recuperar a imagem.");
            }
        }
    }
}
