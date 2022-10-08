﻿namespace Logistics.Sdk.Controllers;

[Route("[controller]")]
[ApiController]
public class LoadController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DataResult<LoadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Permissions.Load.View)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetLoadByIdQuery
        {
            Id = id
        });

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(PagedDataResult<LoadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Permissions.Load.View)]
    public async Task<IActionResult> GetList([FromQuery] GetLoadsQuery request)
    {
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Permissions.Load.Create)]
    public async Task<IActionResult> Create([FromBody] CreateLoadCommand request)
    {
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("update/{id}")]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Permissions.Load.Edit)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateLoadCommand request)
    {
        request.Id = id;
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("delete/{id}")]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    [Authorize(Policy = Permissions.Load.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteLoadCommand
        {
            Id = id
        });

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}