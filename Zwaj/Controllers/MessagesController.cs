using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zwaj.BL.DTOs;
using Zwaj.BL.Helper;
using Zwaj.BL.Interfaces;
using Zwaj.DAL.Entity;

namespace Zwaj.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IZwajRep rep;
        private readonly IMapper mapper;

        public MessagesController(IZwajRep rep,IMapper mapper)
        {
            this.rep = rep;
            this.mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(string userId,int id)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var messageFromRepo = await rep.GetMessage(id);
            if (messageFromRepo == null)
                return NotFound();
            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageForUser(string userId,[FromQuery]MessageParams messageParams)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            messageParams.UserId = userId;

            var messagesFromRepo = await rep.GetMessagesForUser(messageParams);
            var messages = mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesFromRepo);
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);
            return Ok(messages);
        }




        [HttpPost]
        public async Task<IActionResult> CreateMessage(string userId, MessageForCreationDTO message)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            message.SenderId = userId;
            var recipient = await rep.GetUser(message.RecipientId);
            if (recipient == null)
                return BadRequest(new {message = "لم يتم الوصول للمرسل اليه"});
            var newMessage = mapper.Map<Message>(message);
            rep.Add(newMessage);
            return Ok(message);
        }

        [HttpGet("chat/{recipientId}")]
        public async Task<IActionResult> GetConversation(string userId, string recipientId)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            var messagesFromRepo = await rep.GetConversation(userId, recipientId);
            var messgaes = mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesFromRepo);
            return Ok(messgaes);
        }
    }
}