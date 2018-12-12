using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Jackpot : GamblingGame
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                    FIELDS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        private decimal minDeposit;
        private decimal maxdeposit;
        private int skinLimit;



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                  PROPERTIES                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public decimal MinDeposit
        {
            get { return minDeposit; }
            set { minDeposit = value; }
        }
        public decimal MaxDeposit
        {
            get { return maxdeposit; }
            set { maxdeposit = value; }
        }
        public int SkinLimit
        {
            get { return skinLimit; }
            set { skinLimit = value; }
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                 CONSTRUCTORS                                                                 //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public Jackpot(int ID, List<Bet> Bets, decimal MinDeposit, decimal MaxDeposit, int SkinLimit) : base(ID, Bets)
        {
            minDeposit = MinDeposit;
            maxdeposit = MaxDeposit;
            skinLimit = SkinLimit;
        }
    }
}
