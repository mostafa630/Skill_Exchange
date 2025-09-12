using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Validator
{
    public interface IValidator<T>
    {
        Task<bool> IsValidAsync(T entity, CancellationToken cancellationToken);
    }

}
