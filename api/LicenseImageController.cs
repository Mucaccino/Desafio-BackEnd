using Microsoft.AspNetCore.Mvc;
using Motto.Models;
using Motto.Utils;
using Motto.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Motto.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseImageController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMinioService _minioService;

        public LicenseImageController(ApplicationDbContext dbContext, IMinioService minioService)
        {
            _dbContext = dbContext;
            _minioService = minioService;
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

            var driver = await _dbContext.DeliveryDrivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound("Entregador não encontrado.");
            }

            try
            {
                // Salvar a imagem no MinIO
                var fileName = $"DeliveryDriverImage_{driver.Id}{System.IO.Path.GetExtension(image.FileName)}";
                var imagePath = await _minioService.UploadImageAsync(image, fileName);

                // Atualizar o campo DriverLicenseImage do DeliveryDriver
                driver.DriverLicenseImage = imagePath;
                _dbContext.Update(driver);
                await _dbContext.SaveChangesAsync();

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
            // Verificar se o DeliveryDriver com o ID especificado existe
            var deliveryDriver = await _dbContext.DeliveryDrivers.FindAsync(id);
            if (deliveryDriver == null)
            {
                return NotFound();
            }

            try
            {
                // Recuperar a imagem do MinIO
                var imageStream = await _minioService.GetImageAsync(deliveryDriver.DriverLicenseImage);
                // Retornar a imagem como resposta
                return File(imageStream.ToArray(), "image/png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao recuperar a imagem.");
            }
        }
    }
}