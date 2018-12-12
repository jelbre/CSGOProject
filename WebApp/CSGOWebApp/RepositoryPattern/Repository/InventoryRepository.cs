using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class InventoryRepository
    {
        IInventoryContext context;
        public InventoryRepository(IInventoryContext Context)
        {
            context = Context;
        }
    }
}
