using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FixFinder.Models
{
    public class MessagesRepository
    {
        public IEnumerable<Mensagem> GetAllMessages()
        {
            List<Mensagem> mensagens;
            using (var context = new DatabaseEntities())
            {
                mensagens = context.Mensagem.ToList();
            }
            return mensagens;
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
                ChatHub.sendMessages();
        }
    }
}