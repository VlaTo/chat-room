using AutoMapper;
using LibraProgramming.ChatRoom.Domain.Models;
using LibraProgramming.ChatRoom.Domain.Results;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries;
using LibraProgramming.ChatRoom.Services.Chat.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
    //[ApiController]
    public sealed class RoomsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public RoomsController(
            IMediator mediator,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(RoomsResult), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> List()
        {
            try
            {
                var rooms = await mediator.Send(new GetRoomsQuery());

                return Ok(new RoomsResult
                {
                    Rooms = rooms
                        .Select(room => mapper.Map<RoomDetails>(room))
                        .ToArray()
                });
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns><see cref="RoomCreatedResult" /> object with created room information.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(RoomCreatedResult), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateRoomModel model)
        {
            if (false == ModelState.IsValid)
            {
                return BadRequest(model);
            }

            try
            {
                var id = await mediator.Send(new CreateRoomCommand(model.Name, model.Description));
                var room = await mediator.Send(new GetRoomQuery(id));

                return Created(
                    Url.Action("Get", "Room", new {id}),
                    mapper.Map<RoomCreatedResult>(room)
                );
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}