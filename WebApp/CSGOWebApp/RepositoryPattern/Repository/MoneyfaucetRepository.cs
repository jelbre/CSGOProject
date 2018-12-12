using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class MoneyfaucetRepository
    {
        IMoneyfaucetContext context;
        public MoneyfaucetRepository(IMoneyfaucetContext Context)
        {
            context = Context;
        }

        public List<Moneyfaucet> GetAll(User user)
        {
            return context.GetAll(user);
        }
    }
}
