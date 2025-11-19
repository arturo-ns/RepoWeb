using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using prueba.Resources;
using prueba.bounded_context.Domain.Services;
using prueba.bounded_context.Interfaces.REST.Resources;
using prueba.bounded_context.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace prueba.bounded_context.Interfaces.REST;

/// <summary>
///     Controller for Bills endpoint
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Bills")]
public class BillsController(
    IBillCommandService commandService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    /// <summary>
    ///     Creates a new bill
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a bill")]
    [SwaggerResponse(201, "Created", typeof(BillResource))]
    [SwaggerResponse(400, "Bad Request")]
    [SwaggerResponse(409, "Conflict")]
    public async Task<ActionResult> CreateBill([FromBody] CreateBillResource resource)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var command = CreateBillCommandFromResourceAssembler.ToCommandFromResource(resource);

        try
        {
            var result = await commandService.Handle(command);
            if (result is null)
                return Conflict(localizer["BillDuplicated"].Value);

            var resource = BillResourceFromEntityAssembler.ToResourceFromEntity(result);
            return Created($"/api/v1/bills/{result.BillNumber}", resource);
        }
        catch (Exception ex) when (ex.Message.Contains("already exists"))
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex) when (ex.Message.Contains("must be greater than zero") || 
                                   ex.Message.Contains("cannot be less") ||
                                   ex.Message.Contains("must be one of"))
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}
