using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public class CoinflipRepository
    {
        ICoinflipContext context;
        public CoinflipRepository(ICoinflipContext Context)
        {
            context = Context;
        }

        public List<Coinflip> GetAll()
        {
            return context.GetAll();
        }

        public Coinflip GetByID(int ID)
        {
            return context.GetByID(ID);
        }

        public int CreateCoinflip(User User, List<Skin> Skins)
        {
            return context.CreateCoinflip(User, Skins);
        }

        public bool PotentialJoin(Coinflip coinflip)
        {
            return context.PotentialJoin(coinflip);
        }

        public bool JoinCoinflip(Coinflip coinflip, User User, List<Skin> Skins)
        {
            return context.JoinCoinflip(coinflip, User, Skins);
        }

        public int RewardWinner(Coinflip coinflip)
        {
            return context.RewardWinner(coinflip);
        }
    }
}
