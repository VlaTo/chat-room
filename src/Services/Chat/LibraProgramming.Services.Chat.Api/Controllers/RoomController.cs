using AutoMapper;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries;
using LibraProgramming.ChatRoom.Services.Chat.Api.Models;
using LibraProgramming.Services.Chat.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public sealed class RoomController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public RoomController(
            IMediator mediator,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// GET /api/room/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(RoomOperationResult), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var room = await mediator.Send(new GetRoomQuery {Id = id});
                return Ok(mapper.Map<RoomOperationResult>(room));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// PUT /api/room/room-id/
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long id, [FromBody] RoomDetailsModel model)
        {
            try
            {
                if (null == model || false == ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }

                await mediator.Send(new EditRoomCommand(id, model.Name, model.Description));

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:long}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await mediator.Send(new DeleteRoomCommand(id));
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
