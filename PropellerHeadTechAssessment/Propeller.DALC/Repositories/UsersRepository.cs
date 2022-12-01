﻿using Microsoft.EntityFrameworkCore;
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
    public class UsersRepository: IUsersRepository
    {
        private CustomerDbContext _customerDbContext;

        public UsersRepository(CustomerDbContext customerDbContext)
        {
            _customerDbContext = customerDbContext ?? throw new ArgumentNullException(nameof(customerDbContext));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<User?> ValidateUser(string userName, string password)
        {
            return _customerDbContext.Users
                        .Where(x=> x.UserName.Equals(userName) && x.Password.Equals(password))
                        .FirstOrDefaultAsync();
        }

    }
}