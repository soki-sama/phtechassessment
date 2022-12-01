using AutoMapper;
using Propeller.Entities;
using Propeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Mappers
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, PropellerUser>();
        }
    }
}
