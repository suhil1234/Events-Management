using Event_managment.Models;
using System.Collections.Generic;
using System.Linq;

namespace Event_managment.BL
{
    public interface IAccessListService
    {
        List<TbAccessList> GetAll();
        TbAccessList GetByID(int id);
        bool Save(TbAccessList ipAddress);
        bool IsEventUnique(string ipAddress, int currentId);
        bool IsBlocked(string ipAddress);
        bool Delete(int id);
    }

    public class ClsAccessList : IAccessListService
    {
        private readonly EventManagement2Context _context;

        public ClsAccessList(EventManagement2Context context)
        {
            _context = context;
        }

        public List<TbAccessList> GetAll()
        {
            return _context.TbAccessLists.Where(l => !l.IsDeleted).ToList();
        }

        public TbAccessList GetByID(int id)
        {
            return _context.TbAccessLists.FirstOrDefault(a => a.Id == id && !a.IsDeleted);
        }

        public bool Save(TbAccessList ipAddress)
        {
            if (ipAddress.Id == 0)
            {
                _context.TbAccessLists.Add(ipAddress);
            }
            else
            {
                var existingEntry = _context.TbAccessLists.Find(ipAddress.Id);
                existingEntry.IpAddress = ipAddress.IpAddress;
                existingEntry.Reason = ipAddress.Reason;
                existingEntry.Type = ipAddress.Type;
            }
            _context.SaveChanges();
            return true;
        }

        public bool IsEventUnique(string ipAddress, int currentId)
        {
            return _context.TbAccessLists.Any(e => e.IpAddress == ipAddress && e.Id != currentId);
        }

        public bool IsBlocked(string ipAddress)
        {
            return _context.TbAccessLists.Any(a => a.IpAddress == ipAddress && a.Type == "Deny");
        }

        public bool Delete(int id)
        {
            var accessList = GetByID(id);
            accessList.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }
    }
}