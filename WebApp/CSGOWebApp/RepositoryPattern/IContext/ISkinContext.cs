using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public interface ISkinContext
    {
        bool InsertSkins(List<Skin> skins, User user, bool invType);

        Inventory GetAllFromInv(User user, bool invType);

        bool TransferSkins(User user, List<Skin> skins, bool targetInv);
    }
}
