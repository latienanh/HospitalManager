using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Users;

namespace HospitalManager.Hub
{

    [HubRoute("/notificationHub")]
    public class NotificationHub(ICurrentUser currentUser) : AbpHub
    {

        public override async Task OnConnectedAsync()
        {
            string groupName = null;
            if (currentUser.Roles != null && currentUser.Roles.Contains("admin"))
            {
                groupName = "admin";
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }
            await Clients.Caller.SendAsync("ReceiveGroupInfo", groupName);
            await base.OnConnectedAsync();
        }

        public async Task SendNotification(string message)
        {
            await Clients.Group("Admin").SendAsync("ReceiveNotification", message);
        }
     
        public override async Task OnDisconnectedAsync(Exception exception)
        {

            if (currentUser.Roles.Contains("Admin"))
            { 
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admin");
            } 
            await base.OnDisconnectedAsync(exception);
        }
    }
}
