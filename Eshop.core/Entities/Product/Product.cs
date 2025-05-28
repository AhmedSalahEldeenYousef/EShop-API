using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Core.Entities.Product
{
    public  class Product : BaseEntity<int>
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal PriceTotal { get; set; }
        public int CategoryId { get; set; }
        //setting relationship

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        public virtual List<Photo> photos { get; set; }
    }
}
