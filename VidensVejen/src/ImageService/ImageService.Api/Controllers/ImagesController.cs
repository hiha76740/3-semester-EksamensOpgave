using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ImageService.FacadeLib.Commands.DTOs;
using ImageService.FacadeLib.Commands.Interfaces;
using ImageService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Responses.ImageService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ImageService.Api.Controllers;

[Route("/v1/images")]
[ApiController]
public class ImagesController(
    IUploadImageHandler upload,
    IDeleteImageHandler delete,
    IImageQueries queries,
    IConfiguration appsettings
    ) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [EndpointSummary("Get url for image")]
    [EndpointDescription("Gets the url for the requested image")]
    [ProducesResponseType<ImageUrlResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns the url for the requested image")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the url for requested image")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Image was not found")]
    public async Task<ActionResult<ImageUrlResponse>> GetImageUrl(
        [Required][Description("The id of the image you want to get url for")] Guid id)
    {
        try
        {
            var dto = await queries.GetImageUrlComponentsById(id);

            if (dto == null)
                return NotFound($"Image with id {id} was not found");

            var baseUrl = appsettings["ImageBaseUrl"];

            var url = baseUrl == null ? $"/images/themes/{dto.StorageKey}{dto.Extension}" : $"{baseUrl}/images/themes/{dto.StorageKey}{dto.Extension}";

            var response = new ImageUrlResponse(url);

            return Ok(response);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{fileName}")]
    [EndpointSummary("Delete a image")]
    [EndpointDescription("Deletes the requested image")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Deletion of requested image completed successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to delete the requested image")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Image was not found")]
    public async Task<ActionResult> DeleteImage(
        [Required][Description("The file name you want to get url for")] string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName) == true)
                return BadRequest("File name must have a value");

            await delete.HandleAsync(fileName);

            return NoContent();

        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [RequestSizeLimit(6 * 1024 * 1024)]
    [EndpointSummary("Upload and save a image")]
    [EndpointDescription("Uploads and saves a single image")]
    [ProducesResponseType<ImageIdResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns the id of the image")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to upload and save the image")]
    public async Task<ActionResult<ImageIdResponse>> UploadImage(
        [Required][FromForm][Description("The image you want to upload and save")] IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        try
        {
            const long MaxFileSize = 5 * 1024 * 1024;

            if (imageFile.Length == 0)
                return BadRequest("Please choose a image file");

            if (imageFile.Length > MaxFileSize)
                return BadRequest($"Size limit of image is 5 MB.");

            var extension = Path.GetExtension(imageFile.FileName)
                .ToLowerInvariant();

            using var stream = imageFile.OpenReadStream();

            var uploadImageCommand = new UploadImageCommand(stream, imageFile.ContentType, extension);

            var id = await upload.HandleAsync(uploadImageCommand, cancellationToken);

            var response = new ImageIdResponse(id);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



}
