using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Rarity
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                    FIELDS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        private int id;
        private string name;
        private int colorR;
        private int colorB;
        private int colorG;
        private int higherRarityID;



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
        public int ColorR
        {
            get { return colorR; }
            set { colorR = value; }
        }
        public int ColorG
        {
            get { return colorG; }
            set { colorG = value; }
        }
        public int ColorB
        {
            get { return colorB; }
            set { colorB = value; }
        }
        public int HigherRarityID
        {
            get { return higherRarityID; }
            set { higherRarityID = value; }
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                 CONSTRUCTORS                                                                 //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public Rarity(int ID, string RarityName, int ColorR, int ColorG, int ColorB, int HigherRarityID)
        {
            id = ID;
            name = RarityName;
            colorR = ColorR;
            colorG = ColorG;
            colorB = ColorB;
            higherRarityID = HigherRarityID;
        }

        public Rarity(int ID, string RarityName, int ColorR, int ColorG, int ColorB)
        {
            id = ID;
            name = RarityName;
            colorR = ColorR;
            colorG = ColorG;
            colorB = ColorB;
        }

        public Rarity(int ID)
        {
            id = ID;
        }
    }
}
