﻿using AutoMapper;
using Propeller.Entities;
using Propeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Mappers
{
    public class NoteProfile: Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteDto>();
        }
    }
}
