﻿using Propeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Interfaces
{
    public interface ICustomerStatusRepository
    {
        Task<IEnumerable<CustomerStatus>> RetrieveStatusesAsync();

        Task<bool> ValidateStatusExists(int statusId);
    }
}
