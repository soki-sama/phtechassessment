using AutoMapper;
using Propeller.Entities;
using Propeller.Models.Requests;
using Propeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Propeller.Shared;

namespace Propeller.Mappers
{
    public class CustomerStatusProfile: Profile
    {
        public CustomerStatusProfile()
        {
            CreateMap<CustomerStatus, CustomerStatusDto>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s.ID.Obfuscate()));
        }

    }
}
