using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Skill_Exchange.Application.DTOs
{
    public class PaginationDto
    {
        public bool ApplyPagination { get; set; } = false;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 0;
    }
}