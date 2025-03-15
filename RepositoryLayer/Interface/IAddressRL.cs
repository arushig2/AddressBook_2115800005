using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IAddressRL
    {
        public List<AddressEntryEntity> GetAllAddress();
        public AddressEntryEntity GetAddressByID(int id);
        public AddContactModel AddContact(AddContactModel newContact);
        public UpdateContactModel UpdateContact(int id, UpdateContactModel updateContact);
        public bool DeleteContact(int id);

    }
}
