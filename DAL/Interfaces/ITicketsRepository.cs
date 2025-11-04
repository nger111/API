using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public partial interface IticketsRepository
    {
        bool Create(Tickets model);
        Tickets GetDatabyID(string id);
        List<Tickets> GetDataAll();
        bool Update(Tickets model);
        bool Delete(int ticketId); 
    }
}
