using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class SkinRepository
    {
        ISkinContext context;
        public SkinRepository(ISkinContext Context)
        {
            context = Context;
        }

        public bool InsertSkins(List<Skin> skins, User user, bool invType)
        {
            return context.InsertSkins(skins, user, invType);
        }

        public Inventory GetAllFromInv(User user, bool invType)
        {
            return context.GetAllFromInv(user, invType);
        }

        public bool TransferSkins(User user, List<Skin> skins, bool targetInv)
        {
            return context.TransferSkins(user, skins, targetInv);
        }
    }
}
