using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract class GamblingGame
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                    FIELDS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        private int id;
        private List<Bet> bets;



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                  PROPERTIES                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public List<Bet> Bets
        {
            get { return bets; }
            set { bets = value; }
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                 CONSTRUCTORS                                                                 //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public GamblingGame()
        {
            bets = new List<Bet>();
        }

        public GamblingGame(int ID, List<Bet> Bets)
        {
            id = ID;
            bets = Bets;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                   METHODS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public decimal getTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (Bet bet in bets)
            {
                foreach (Skin skin in bet.Skins)
                {
                    totalPrice = totalPrice + skin.Price;
                }
            }
            return totalPrice;
        }

        public decimal getBetPrice(int betIndex)
        {
            decimal totalPrice = 0;
            foreach (Skin skin in bets[betIndex].Skins)
            {
                totalPrice = totalPrice + skin.Price;
            }
            return totalPrice;
        }
    }
}
