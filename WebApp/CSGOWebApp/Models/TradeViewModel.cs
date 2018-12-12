using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TradeViewModel
    {
        private int index;
        private Inventory inventory;
        private List<List<Skin>> orderedSkins;
        private List<int> selectedSkins;
        private List<Skin> finalSkinList;
        private string sortOption;
        private string rarityFilter;
        private string textFilter;
        private int chunkSize;

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
        public string SortOption
        {
            get { return sortOption; }
            set { sortOption = value; }
        }
        public string RarityFilter
        {
            get { return rarityFilter; }
            set { rarityFilter = value; }
        }
        public string TextFilter
        {
            get { return textFilter; }
            set { textFilter = value; }
        }
        public int ChunkSize
        {
            get { return chunkSize; }
            set { chunkSize = value; }
        }



        public TradeViewModel()
        {
            inventory = new Inventory();
            orderedSkins = new List<List<Skin>>();
            selectedSkins = new List<int>();
            for (int i = 0; i < 24; i++)
            {
                selectedSkins.Add(-1);
            }
            finalSkinList = new List<Skin>();
        }

        public TradeViewModel(int chunkSize)
        {
            inventory = new Inventory();
            orderedSkins = new List<List<Skin>>();
            selectedSkins = new List<int>();
            for (int i = 0; i < chunkSize; i++)
            {
                selectedSkins.Add(-1);
            }
            finalSkinList = new List<Skin>();
            this.chunkSize = chunkSize;
        }



        public void SortInventory()
        {
            try
            {
                switch (sortOption)
                {
                    case "Acquired":
                        inventory.Skins.Sort((x, y) => DateTime.Compare(y.DateAdded, x.DateAdded));
                        break;

                    case "Quality":
                        inventory.Skins.Sort((x, y) => y.Rarity.ID.CompareTo(x.Rarity.ID));
                        break;

                    case "Alphabetical":
                        inventory.Skins = inventory.Skins.OrderBy(x => x.Weapon).ThenBy(x => x.Name).ToList();
                        break;

                    case "Collection":
                        inventory.Skins = inventory.Skins.OrderBy(x => x.Collection).ThenByDescending(x => x.Rarity.ID).ToList();
                        break;

                    case "PriceHL":
                        inventory.Skins.Sort((x, y) => decimal.Compare(y.Price, x.Price));
                        break;

                    case "PriceLH":
                        inventory.Skins.Sort((x, y) => decimal.Compare(x.Price, y.Price));
                        break;
                }
            }
            catch
            { }
            FilterInventory();
        }

        private void FilterInventory()
        {
            try
            {
                List<Skin> rarityFilteredSkins = new List<Skin>();

                //Filter rarity
                if (rarityFilter != "Show all items" && rarityFilter != null)
                {
                    foreach (Skin skin in inventory.Skins)
                    {
                        if (skin.Rarity.Name == rarityFilter)
                        {
                            rarityFilteredSkins.Add(skin);
                        }
                    }
                }
                else
                {
                    rarityFilteredSkins = inventory.Skins;
                }

                List<Skin> fullFilteredSkins = new List<Skin>();

                //Filter string
                if (textFilter != null)
                {
                    foreach (Skin skin in rarityFilteredSkins)
                    {
                        string searchString = skin.Weapon + " | " + skin.Name;
                        if (searchString.IndexOf(textFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            fullFilteredSkins.Add(skin);
                        }
                    }
                }
                else
                {
                    fullFilteredSkins = rarityFilteredSkins;
                }
                OrderedSkins = ChunkBy(fullFilteredSkins, chunkSize);
            }
            catch
            { }
        }




        private List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
