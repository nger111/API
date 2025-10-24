using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public partial interface ITicketBusiness
    {
        bool Create(Tickets model);
        Tickets GetDatabyID(string id);
        List<Tickets> GetDataAll();
        bool Update(Tickets model);
        bool Delete(int ticketId); // NEW
    }
}
