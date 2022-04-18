using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RentalService.Chat
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            string userName = Context.User.Identity.Name;
            string you = "You";
            await Clients.Others.SendAsync("Receive", message, userName);
            await Clients.Caller.SendAsync("Receive", message, you);
        }
        [Authorize(Roles = "admin")]
        public async Task Notify(string message)
        {
            await Clients.All.SendAsync("ReceiveNotify", message);
        }
    }
}
