using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Interfaces;

namespace BLL
{
    public partial class TicketsBusiness : ITicketBusiness
    {
        private IticketsRepository _res;
        public TicketsBusiness(IticketsRepository TicketsRes)
        {
            _res = TicketsRes;
        }
        public bool Create(Tickets model)
        {
            return _res.Create(model);
        }
        public Tickets GetDatabyID(string id)
        {
            return _res.GetDatabyID(id);
        }
        public List<Tickets> GetDataAll()
        {
            return _res.GetDataAll();
        }
        public bool Update(Tickets model) // NEW
        {
            return _res.Update(model);
        }
        public bool Delete(int ticketId) // NEW
        {
            return _res.Delete(ticketId);
        }
    }

}
