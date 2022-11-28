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

            CreateMap<CreateCustomerRequest, Customer>()
                .ForMember(d => d.CustomerStatusID, o => o.MapFrom(s => s.Status));

            CreateMap<UpdateCustomerRequest, Customer>()
                .ForMember(d => d.CustomerStatusID, o => o.MapFrom(s => s.Status));
        }
    }
}