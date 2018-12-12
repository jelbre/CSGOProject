using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Skin
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                    FIELDS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        private int id;
        private string weapon;
        private string name;
        private string collection;
        private int patternID;
        private double skinFloat;
        private decimal price;
        private DateTime dateAdded;
        private Rarity rarity;



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                  PROPERTIES                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Collection
        {
            get { return collection; }
            set { collection = value; }
        }
        public int PatternID
        {
            get { return patternID; }
            set { patternID = value; }
        }
        public double SkinFloat
        {
            get { return skinFloat; }
            set { skinFloat = value; }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        public DateTime DateAdded
        {
            get { return dateAdded; }
            set { dateAdded = value; }
        }
        public Rarity Rarity
        {
            get { return rarity; }
            set { rarity = value; }
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                 CONSTRUCTORS                                                                 //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public Skin(int ID, string Weapon, string Name, string Collection, int PatternID, double SkinFloat, decimal Price, DateTime DateAdded, Rarity Rarity)
        {
            id = ID;
            
            weapon = Weapon;
            name = Name;
            collection = Collection;
            patternID = PatternID;
            skinFloat = SkinFloat / 1000000000;
            price = Price;
            dateAdded = DateAdded;

            rarity = Rarity;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                   METHODS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public string GetWear()
        {
            if (skinFloat < 0.07)
            {
                return "Factory New";
            }
            else if (skinFloat < 0.15)
            {
                return "Minimal Wear";
            }
            else if (skinFloat < 0.37)
            {
                return "Field Tested";
            }
            else if (skinFloat < 0.44)
            {
                return "Well Worn";
            }
            else
            {
                return "Battle Scarred";
            }
        }
    }
}