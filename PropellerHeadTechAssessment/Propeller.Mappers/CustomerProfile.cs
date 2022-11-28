using AutoMapper;
using Propeller.Entities;
using Propeller.Models;

namespace Propeller.Mappers
{
    public class CustomerProfile: Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}