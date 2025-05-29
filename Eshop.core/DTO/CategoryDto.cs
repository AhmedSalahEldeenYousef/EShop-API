using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Core.DTO
{
    public record CategoryDto(string Name, string Description);
    public record UpdateCategoryDto(int id, string Name, string Description);
   
}
