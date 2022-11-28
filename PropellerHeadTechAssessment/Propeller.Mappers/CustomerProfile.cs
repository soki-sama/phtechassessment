using AutoMapper;
using Propeller.Entities;
using Propeller.Models;
using Propeller.Models.Requests;

namespace Propeller.Mappers
{
    public class CustomerProfile: Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerRequest, CustomerDto>();
            CreateMap<CreateCustomerRequest, Customer>();
        }
    }
}