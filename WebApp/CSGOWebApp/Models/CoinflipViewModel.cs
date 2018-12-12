using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CoinflipViewModel
    {
        //List of coinflips
        private List<Coinflip> coinflips;


        //Used for selecting skins when creating coinflip
        private int index;
        private Inventory inventory;
        private List<List<Skin>> orderedSkins;
        private List<int> selectedSkins;
        private List<Skin> finalSkinList;
        private string sortOption;
        private string rarityFilter;
        private string textFilter;
        private int chunkSize;

        private string errorMessage;




        public List<Coinflip> Coinflips
        {
            get { return coinflips; }
            set { coinflips = value; }
        }


        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public Inventory Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }
        public List<List<Skin>> OrderedSkins
        {
            get { return orderedSkins; }
            set { orderedSkins = value; }
        }
        public List<int> SelectedSkins
        {
            get { return selectedSkins; }
            set { selectedSkins = value; }
        }
        public List<Skin> FinalSkinList
        {
            get { return finalSkinList; }
            set { finalSkinList = value; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }




        public CoinflipViewModel()
        {
            coinflips = new List<Coinflip>();

            inventory = new Inventory();
            orderedSkins = new List<List<Skin>>();
            selectedSkins = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                selectedSkins.Add(-1);
            }
            finalSkinList = new List<Skin>();
        }

        public CoinflipViewModel(List<Coinflip> CoinFlips)
        {
            coinflips = CoinFlips;

            inventory = new Inventory();
            orderedSkins = new List<List<Skin>>();
            selectedSkins = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                selectedSkins.Add(-1);
            }
            finalSkinList = new List<Skin>();
        }




        public List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public decimal GetFinalSkinListPrice()
        {
            decimal price = 0;
            foreach(Skin skin in finalSkinList)
            {
                price = price + skin.Price;
            }
            return price;
        }
    }
}
