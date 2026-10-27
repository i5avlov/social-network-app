using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Services.Contracts;
using SocialNetworkApp.Data.Models;
using SocialNetworkApp.Data.Models.Enums;
using SocialNetworkApp.ViewModels.Invitations;
using System.Security.Claims;

namespace SocialNetworkApp.Controllers
{
    [Authorize] 
    public class InvitationsController : Controller
    {
        private readonly IInvitationsService invitationsService; 
        private readonly IFriendshipsService friendshipsService; 

        public InvitationsController(
            IInvitationsService invitationsService, 
            IFriendshipsService friendshipsService)
        {
            this.invitationsService = invitationsService; 
            this.friendshipsService = friendshipsService; 
        } 

        [HttpGet]
        public async Task<IActionResult> Create(string receiverUserId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); 
            // No logged in user 
            if (userId == null)
            {
                return Unauthorized();
            }

            Invitation? existingInvitation = await this.invitationsService
                .GetBySenderIdReceiverId(userId, receiverUserId);

            // Invitation exists and it is accepted,
            // normal user has no way to send current invitation 
            if (existingInvitation != null 
                && existingInvitation.Status == InvitationStatus.Accepted)
            {
                return View("Error"); 
            }

            // Invitation between logged in user and receiver user 
            // that is waiting or rejected but not by the logged in user 
            if (existingInvitation != null 
                && this.invitationsService.IsRejectedByUserIdAsync(existingInvitation, userId) == false) 
            {
                string message = string.Empty;

                // Waiting for action 
                if (existingInvitation.Status == InvitationStatus.Waiting)
                {
                    // Logged in user has sent an invitation to receiver user 
                    if (this.invitationsService.IsSentByUserIdAsync(existingInvitation, userId)) 
                    {
                        message = "Your invitation is waiting for the user to take action. Please, wait.";
                    }
                    // Logged in user has received an invitation from receiver user
                    else if (this.invitationsService.IsSentByUserIdAsync(existingInvitation, receiverUserId))
                    {
                        message = "The user already has sent you an invitation. Please, take action.";
                    }
                }
                // Invitation by logged in user has been rejected by receiver user 
                else if (this.invitationsService.IsRejectedByUserIdAsync(existingInvitation, receiverUserId))
                {
                    message = "Your invitation is rejected by the user. Sorry.";
                }

                ViewData["InvitationNotSentMessage"] = message;
                ViewData["ReceiverUserId"] = receiverUserId;
                return View("SendingStopped");
                
            }

            InvitationReceiverViewModel receiverUser; 
            try
            {
                // Invitation receiver user details 
                receiverUser = await this.invitationsService
                    .GetReceiverUserAsync(receiverUserId); 
            }
            catch (ArgumentException ex)
            { 
                // No receiver user
                return BadRequest(ex.Message); 
            }

            CreateInvitationFormModel model = new CreateInvitationFormModel()
            {
                SenderUserId = userId, 
                ReceiverUser = receiverUser 
            }; 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInvitationFormModel model) 
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // No logged in user 
            if (userId == null)
            {
                return Unauthorized(); 
            }
            // Sending user id not matching logged in user id 
            if (userId != model.SenderUserId)
            {
                return BadRequest();
            }
            // Model errors
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            try
            { 
                // Invitation sending success 
                await this.invitationsService.CreateAsync(userId, model.ReceiverUser.Id, model.Text); 
                TempData["InvitationSentMessage"] = "You have successfully sent an invitation";
            }
            catch
            { 
                // Invitation sending error 
                TempData["InvitationSentMessage"] = "An error occurred during sending your invitation";
            }

            return RedirectToAction("Profile", "Users", new { userId = model.ReceiverUser.Id }); 
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int invitationId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // No logged in user 
            if (userId == null)
            {
                return Unauthorized(); 
            } 

            try
            {
                // Logged user accepts received invitation 
                bool acceptSuccessful = await this.invitationsService.AcceptAsync(invitationId); 

                // Get invitation details to access sender user id
                string senderUserId = this.invitationsService.GetDetailsById(invitationId).Result.SenderUserId; 
                // Create friendship 
                bool createSuccessful = await this.friendshipsService.AddAsync(senderUserId, userId); 

                return RedirectToAction("Profile", "Users", new { userId = userId }); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int invitationId)
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // No logged in user 
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                bool rejectSuccessful = await this.invitationsService.AcceptAsync(invitationId);
                return RedirectToAction("Profile", "Users", new { userId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
