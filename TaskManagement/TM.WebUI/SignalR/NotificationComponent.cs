using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TM.Domain.DomainObject;

namespace TM.WebUI.SignalR
{
    public class NotificationComponent
    {
        // Register notification (will add sql dependency)
        string memberId = null;
        public void RegisterNotification(DateTime currentTime, string memberId)
        {
            if (string.IsNullOrEmpty(memberId)) return;
            this.memberId = memberId;
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT [Name] FROM [dbo].[MemberTasks] WHERE  MemberId =@MemberId and [CreatedDate] > @CreatedDate";
           
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@MemberId", memberId);
                cmd.Parameters.AddWithValue("@CreatedDate", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                    }
            }
        }

        private void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                // from here we will send notification message to clients
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                // re-register notification
                //string memberId = HttpContext.Current.Session["userName"]?.ToString();
                RegisterNotification(DateTime.Now, memberId);

            }
        }

    }
}