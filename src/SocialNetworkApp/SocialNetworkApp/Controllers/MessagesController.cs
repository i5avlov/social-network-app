using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.ViewModels.Messages;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    
    public class MessagesController : Controller
    {
        private readonly IMessagesService messagesService; 
        private readonly IUsersService usersService; 

        public MessagesController(
            IMessagesService messagesService, 
            IUsersService usersService) 
        {
            this.messagesService = messagesService; 
            this.usersService = usersService; 
        } 

        [HttpGet]
        public async Task<IActionResult> Chat(string otherUserId) 
        {
            string? loggedUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // No logged in user 
            if (loggedUserId == null)
            {
                return Unauthorized();
            }

            try
            {
                MessageSenderViewModel loggedUser = await this.usersService.GetMessageSenderUserById(loggedUserId);
                MessageSenderViewModel otherUser = await this.usersService.GetMessageSenderUserById(otherUserId); 
                IEnumerable<MessageListEntryViewModel> messagesExchanged 
                    = await this.messagesService.GetAllBetweenUsersAsync(loggedUserId, otherUserId);

                ChatViewModel model = new ChatViewModel()
                {
                    Messages = messagesExchanged,
                    LoggedUser = loggedUser,
                    OtherUser = otherUser 
                };
                return View(model); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Chat(ChatViewModel model) 
        {
            string? loggedUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // No logged in user or request message sender id and logged in user id mismatch 
            if (loggedUserId == null 
                || loggedUserId != model.LoggedUser.Id) 
            {
                return Unauthorized(); 
            } 

            bool otherUserExists = await this.usersService.ExistsById(model.OtherUser.Id);
            // No user with request message receiver id 
            if (otherUserExists == false)
            {
                return BadRequest(); 
            }

            if (ModelState.IsValid == false)
            {
                return View(model); 
            } 

            await this.messagesService.AddAsync(loggedUserId, model.OtherUser.Id, model.MessageText ?? ""); 
            return RedirectToAction(nameof(Chat), new { otherUserId = model.OtherUser.Id }); 
        }
    }
}
