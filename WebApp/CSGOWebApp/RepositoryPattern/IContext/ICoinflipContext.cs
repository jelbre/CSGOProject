using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public interface ICoinflipContext
    {
        List<Coinflip> GetAll();

        Coinflip GetByID(int ID);

        int CreateCoinflip(User User, List<Skin> Skins);

        bool PotentialJoin(Coinflip coinflip);

        bool JoinCoinflip(Coinflip coinflip, User User, List<Skin> Skins);

        int RewardWinner(Coinflip coinflip);
    }
}
