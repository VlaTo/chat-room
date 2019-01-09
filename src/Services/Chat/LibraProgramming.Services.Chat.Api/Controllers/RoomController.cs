using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using LibraProgramming.ChatRoom.Services.Chat.Api.Models;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Domain;
using Orleans;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public sealed class RoomController : Controller
    {
        private readonly IClusterClient client;
        private readonly IMapper mapper;

        public RoomController(IClusterClient client, IMapper mapper)
        {
            this.client = client;
            this.mapper = mapper;
        }

        /// <summary>
        /// POST /api/room/
        /// </summary>
        /// <param name="request"></param>
        /// <returns>
        /// <see cref="RoomOperationResult" /> object with created room information.
        /// </returns>
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] RoomCreateRequest request)
        {
            HttpContext.Request.Body.Seek(0L, SeekOrigin.Begin);

            var temp = new StreamReader(HttpContext.Request.Body);

            var temp1 = temp.ReadToEnd();

            try
            {
                var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);
                var id = await resolver.RegisterRoomAsync(request.Name);
                var room = client.GetGrain<IChatRoom>(id);

                await room.SetDescriptionAsync(request.Description);

                return Json(mapper.Map<RoomOperationResult>(new RoomCreateResponse
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description
                }));
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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        [Consumes("application/json")]
        public async Task<IActionResult> Edit(long id, [FromBody] RoomEditRequest request)
        {
            try
            {
                if (null == request || false == ModelState.IsValid)
                {
                    return BadRequest("Invalid State");
                }

                var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);
                var rooms = await resolver.GetRoomsAsync();

                if (false == rooms.ContainsKey(id))
                {
                    throw new InvalidOperationException();
                }

                var room = client.GetGrain<IChatRoom>(id);

                await Task.WhenAll(
                    resolver.RenameRoomAsync(id, request.Name),
                    room.SetDescriptionAsync(request.Description)
                );

                return Json(mapper.Map<RoomOperationResult>(new RoomEditResponse
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description
                }));
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
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);

                if (false == await resolver.RemoveRoomAsync(id))
                {
                    throw new InvalidOperationException();
                }

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
