using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibraProgramming.ChatRoom.Services.Chat.Api.Models;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Domain;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IClusterClient client;
        private readonly IMapper mapper;

        public RoomsController(IClusterClient client, IMapper mapper)
        {
            this.client = client;
            this.mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);
                var rooms = await resolver.GetRoomsAsync();
                var models = new List<RoomOperationResult>();

                foreach (var (id, name) in rooms)
                {
                    var room = client.GetGrain<IChatRoom>(id);
                    var description = await room.GetDescriptionAsync();

                    models.Add(mapper.Map<RoomOperationResult>(new RoomResolveResponse
                    {
                        Id = id,
                        Name = name,
                        Description = description.Description
                    }));
                }

                return Json(new RoomListOperationResult
                {
                    Rooms = models.ToArray()
                });
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}