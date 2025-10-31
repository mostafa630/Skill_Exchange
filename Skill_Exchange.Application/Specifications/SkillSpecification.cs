using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Specifications
{
    public class SkillSpecification : BaseSpecification<Skill>
    {
        private SkillSpecification()
        {

        }
        public static SkillSpecification Build(PaginationDto pagination)
        {
            var skillSpec = new SkillSpecification();
            skillSpec = applyPagination(skillSpec, pagination);
            return skillSpec;
        }
        // uncmment that and pass a SkillFilterDto to the Constructor and see how we apply it in the UserSpecification
        // private static SkillSpecification filter_process(UserSpecification userSpec, SkillFilterDTO filter)
        // {

        // }

        // uncmment that and pass a SkillIncludeDto to the Constructor and see how we apply it in the UserSpecification
        // private static UserSpecification include_process(UserSpecification userSpec, UserIncludesDTO includes)
        // {
        // }


        private static SkillSpecification applyPagination(SkillSpecification skillSpec, PaginationDto pagination)
        {
            if (pagination.ApplyPagination)
            {
                skillSpec.ApplyPaging(pagination.Skip, pagination.Take);
                return skillSpec;
            }
            return skillSpec;
        }

    }
}