using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Moneyfaucet
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                    FIELDS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        private int id;
        private string name;
        private int defaultCooldown;    //Cooldown if win
        private DateTime lastCooldown;  //Last cooldown from user
        private int cip;                //Increment of cooldown on loss
        private decimal minWin;         //Minimum money won
        private decimal maxWin;         //Maximum money won
        private int winPercentage;
        private int minSkinAmount;
        private int maxSkinAmount;
        private decimal price;



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                  PROPERTIES                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int DefaultCooldown
        {
            get { return defaultCooldown; }
            set { defaultCooldown = value; }
        }
        public DateTime LastCooldown
        {
            get { return lastCooldown; }
            set { lastCooldown = value; }
        }
        public int CIP
        {
            get { return cip; }
            set { cip = value; }
        }
        public decimal MinWin
        {
            get { return minWin; }
            set { minWin = value; }
        }
        public decimal MaxWin
        {
            get { return maxWin; }
            set { maxWin = value; }
        }
        public int WinPercentage
        {
            get { return winPercentage; }
            set { winPercentage = value; }
        }
        public int MinSkinAmount
        {
            get { return minSkinAmount; }
            set { minSkinAmount = value; }
        }
        public int MaxSkinAmount
        {
            get { return maxSkinAmount; }
            set { maxSkinAmount = value; }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                 CONSTRUCTORS                                                                 //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public Moneyfaucet(int ID, string Name, int DefaultCooldown, DateTime LastCooldown, int CIP, decimal MinWin, decimal MaxWin, int WinPercentage, int MinSkinAmount, int MaxSkinAmount, decimal Price)
        {
            id = ID;
            name = Name;
            defaultCooldown = DefaultCooldown;
            lastCooldown = LastCooldown;
            cip = CIP;
            minWin = MinWin;
            maxWin = MaxWin;
            winPercentage = WinPercentage;
            minSkinAmount = MinSkinAmount;
            maxSkinAmount = MaxSkinAmount;
            price = Price;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                   METHODS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public List<Skin> UseFaucet()
        {
            //OOK ZORGEN DAT ALLES METEEN IN DB KOMT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (lastCooldown < DateTime.Now)
            {
                if (CheckIfWin())
                {
                    return RewardUser();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private List<Skin> RewardUser()
        {
            Random rnd = new Random();

            //amount of skins won
            int SkinAmount = rnd.Next(maxSkinAmount - minSkinAmount) + minSkinAmount;

            //Total value of all the skins won together
            int winDifference = Convert.ToInt32((MaxWin * 100) - (MinWin * 100));
            decimal TotalPrice = rnd.Next(winDifference) + (MinWin * 100);
            TotalPrice = TotalPrice / 100;

            //Percentage of full value for each skin
            List<decimal> percentage = new List<decimal>();
            int RandomSum = 0;
            for (int i = SkinAmount; i > 0; i--)
            {
                int rndresult = rnd.Next(10000000);
                RandomSum = RandomSum + rndresult;
                percentage.Add(rndresult);
            }


            List<Skin> skins = new List<Skin>();
            while (SkinAmount > 0)
            {
                SkinAmount--;

                decimal price = Math.Round(Map(RandomSum, TotalPrice, percentage[SkinAmount]), 2);
                int rarityid;
                string name;

                //Set rarity and name based on the price
                {
                    if (price * 10 <= 3)
                    {
                        rarityid = 1;
                        name = "Common";
                    }
                    else if (price <= 1)
                    {
                        rarityid = 2;
                        name = "Uncommon";
                    }
                    else if (price <= 3)
                    {
                        rarityid = 3;
                        name = "Rare";
                    }
                    else if (price <= 10)
                    {
                        rarityid = 4;
                        name = "Mythical";
                    }
                    else if (price <= 30)
                    {
                        rarityid = 5;
                        name = "Legendary";
                    }
                    else if (price <= 60)
                    {
                        rarityid = 6;
                        name = "Ancient";
                    }
                    else if (price <= 500)
                    {
                        rarityid = 7;
                        name = "Exceedingly Rare";
                    }
                    else
                    {
                        rarityid = 8;
                        name = "Immortal";
                    }
                }
                //

                //Create float based on chance of each wear
                int tempFloat = rnd.Next(440000000, 1000000000);
                {
                    int wearPercentage = rnd.Next(100);
                    if (wearPercentage < 10)
                    {
                        tempFloat = rnd.Next(70000000);
                    }
                    else if (wearPercentage < 25)
                    {
                        tempFloat = rnd.Next(70000000, 150000000);
                    }
                    else if (wearPercentage < 60)
                    {
                        tempFloat = rnd.Next(150000000, 370000000);
                    }
                    else if (wearPercentage < 70)
                    {
                        tempFloat = rnd.Next(370000000, 440000000);
                    }
                }
                //

                skins.Add(new Skin(0, "Token", name, "Token Collection", 1, tempFloat, price, DateTime.Now, new Rarity(rarityid)));
            }

            return skins;
        }

        private bool CheckIfWin()
        {
            try
            {
                Random rnd = new Random();
                decimal winint = rnd.Next(100000000);
                winint = winint / 1000000;
                if (winint <= WinPercentage)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public decimal Map(decimal randomSum, decimal totalPrice, decimal percentage)
        {
            return percentage * (totalPrice / randomSum);
        }
    }
}
