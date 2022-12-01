using AutoMapper;
using Propeller.Entities;
using Propeller.Models;
using Propeller.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Mappers
{
    public class ContactProfile: Profile
    {
        public ContactProfile()
        {
            CreateMap<UpdateContactRequest, Contact>();
            CreateMap<Contact, ContactDto>();
        }
    }
}
