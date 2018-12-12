using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Inventory
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                    FIELDS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        private int id;
        private bool type;
        private List<Skin> skins;



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                  PROPERTIES                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public bool Type
        {
            get { return type; }
            set { type = value; }
        }
        public List<Skin> Skins
        {
            get { return skins; }
            set { skins = value; }
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                 CONSTRUCTORS                                                                 //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public Inventory (int ID, bool Type, List<Skin> Skins)
        {
            id = ID;

            //Later wss via repo en DB met gebruikersID
            type = Type;
            skins = Skins;
        }

        public Inventory()
        {
            skins = new List<Skin>();
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                   METHODS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public List<Skin> GetSkins(string textfilter, Filters filter, SortOptions sortOption)
        {
            List<Skin> tempskins = FilterText(textfilter, skins);
            tempskins = Filter(filter, tempskins);
            tempskins = Sort(sortOption, tempskins);
            return tempskins;
        }

        public List<Skin> FilterText(string textfilter, List<Skin> skins)
        {
            List<Skin> tempskins;

            //Filter ipv overschrijven
            tempskins = skins;

            return tempskins;
        }

        public List<Skin> Filter(Filters filter, List<Skin> skins)
        {
            List<Skin> tempskins;

            //Filter ipv overschrijven
            tempskins = skins;

            return tempskins;
        }

        public List<Skin> Sort(SortOptions sortOption, List<Skin> skins)
        {
            List<Skin> tempskins;

            //Sorteer ipv overschrijven
            tempskins = skins;

            return tempskins;
        }
    }
}
