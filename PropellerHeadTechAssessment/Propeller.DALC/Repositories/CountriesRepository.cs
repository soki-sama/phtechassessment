using Microsoft.EntityFrameworkCore;
using Propeller.DALC.Interfaces;
using Propeller.DALC.Sqlite;
using Propeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.DALC.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private PropellerDbContext _propellerDbContext;

        public CountriesRepository(PropellerDbContext propellerDbContext)
        {
            _propellerDbContext = propellerDbContext;
        }

        public async Task<IEnumerable<Country>> RetrieveCountries()
        {
            return await _propellerDbContext.Countries.ToListAsync();
        }

    }
}
