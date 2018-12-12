using Models;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSGOClient
{
    public partial class MainForm : Form
    {
        //Fields
        public User user;
        MoneyfaucetRepository RepoMF = new MoneyfaucetRepository(MoneyfaucetFactory.GetContext(ContextType.MSSQL));
        UserRepository RepoUser = new UserRepository(UserFactory.GetContext(ContextType.MSSQL));
        SkinRepository RepoSkin = new SkinRepository(SkinFactory.GetContext(ContextType.MSSQL));
        //1=home, 2=inv, 3=moneyfaucet
        int loadedPage = 1;

        //Constructor
        public MainForm()
        {
            InitializeComponent();
            if (user == null)
            {
                using (Inlogform inlogform = new Inlogform(this))
                {
                    DialogResult result = inlogform.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        user = inlogform.user;
                        lblUsername.Text = user.Username;
                    }
                }
            }
        }
        
        //Methods
        private List<List<T>> ChunkBy<T>(List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }







        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------Inventory page-----------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------

        //FIELDS
        Inventory inventory;
        List<List<Skin>> chunkedSkins;
        int invIndex = 0;
        string invFilter;

        List<Panel> invSlots;



        //METHODS
        private void LoadInventoryPage()
        {
            invSlots = new List<Panel>
            { pnInvSlot1,  pnInvSlot2,  pnInvSlot3,  pnInvSlot4,  pnInvSlot5,  pnInvSlot6,
              pnInvSlot7,  pnInvSlot8,  pnInvSlot9,  pnInvSlot10, pnInvSlot11, pnInvSlot12,
              pnInvSlot13, pnInvSlot14, pnInvSlot15, pnInvSlot16, pnInvSlot17, pnInvSlot18,
              pnInvSlot19, pnInvSlot20, pnInvSlot21, pnInvSlot22, pnInvSlot23, pnInvSlot24,
              pnInvSlot25, pnInvSlot26, pnInvSlot27, pnInvSlot28, pnInvSlot29, pnInvSlot30};

            dudInvSorter.SelectedIndex = 0;
            dudInvFilter.SelectedIndex = 0;
            invIndex = 0;
            invFilter = null;

            inventory = RepoSkin.GetAllFromInv(user, false);
            SortInventory("Acquired");
        }

        private void LoadInventoryItems()
        {
            lblIInvtemAmount.Text = inventory.Skins.Count.ToString() + " Items";
            lblInvPageNumber.Text = "Page " + (invIndex + 1) + "/" + chunkedSkins.Count.ToString();

            if (invIndex > 0) { btnInvPrev.Visible = true; }
            else { btnInvPrev.Visible = false; }

            if (chunkedSkins.Count > invIndex + 1)
            { btnInvNext.Visible = true; }
            else { btnInvNext.Visible = false; }

            for (int i = 0; i < 30; i++)
            {
                invSlots[i].Refresh();
                if (chunkedSkins.Count > invIndex)
                {
                    if (i < chunkedSkins[invIndex].Count)
                    {
                        Graphics g = invSlots[i].CreateGraphics();

                        g.FillRectangle(new SolidBrush(Color.FromArgb(165, 165, 165)), new Rectangle(0, 0, 65, 45));

                        Color c = Color.FromArgb(chunkedSkins[invIndex][i].Rarity.ColorR, chunkedSkins[invIndex][i].Rarity.ColorG, chunkedSkins[invIndex][i].Rarity.ColorB);
                        g.FillRectangle(new SolidBrush(c), new Rectangle(0, 45, 65, 20));

                        g.DrawImage(Image.FromFile(@"C:\Users\jelle\Desktop\S2 herstart\Periode 3\Killerapp\Applicaties\ClientApp\CSGOClient\CSGOClient\Images\" + chunkedSkins[invIndex][i].Weapon + ".png"),
                            new Rectangle(15, 5, 35, 35));

                        g.DrawString(chunkedSkins[invIndex][i].Weapon, new Font("Microsoft Sans Serif", 7), new SolidBrush(Color.White), 1, 44);
                        g.DrawString(chunkedSkins[invIndex][i].Name, new Font("Microsoft Sans Serif", 6), new SolidBrush(Color.FromArgb(200, 200, 200)), 2, 56);
                    }
                }
            }
        }

        private void SortInventory(string sort)
        {
            try
            {
                switch (sort)
                {
                    case "Acquired":
                        inventory.Skins.Sort((x, y) => DateTime.Compare(y.DateAdded, x.DateAdded));
                        FilterInventory(dudInvFilter.SelectedItem.ToString());
                        break;

                    case "Quality":
                        inventory.Skins.Sort((x, y) => y.Rarity.ID.CompareTo(x.Rarity.ID));
                        FilterInventory(dudInvFilter.SelectedItem.ToString());
                        break;

                    case "Alphabetical":
                        inventory.Skins = inventory.Skins.OrderBy(x => x.Weapon).ThenBy(x => x.Name).ToList();
                        FilterInventory(dudInvFilter.SelectedItem.ToString());
                        break;

                    case "Collection":
                        inventory.Skins = inventory.Skins.OrderBy(x => x.Collection).ThenByDescending(x => x.Rarity.ID).ToList();
                        FilterInventory(dudInvFilter.SelectedItem.ToString());
                        break;
                }
            }
            catch
            { }
        }

        private void FilterInventory (string filter)
        {
            try
            {
                List<Skin> rarityFilteredSkins = new List<Skin>();

                //Filter rarity
                if (filter != "Show all items")
                {
                    foreach (Skin skin in inventory.Skins)
                    {
                        if (skin.Rarity.Name == filter)
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
                if (invFilter != null)
                {
                    foreach (Skin skin in rarityFilteredSkins)
                    {
                        string searchString = skin.Weapon + " | " + skin.Name;
                        if (searchString.IndexOf(invFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            fullFilteredSkins.Add(skin);
                        }
                    }
                }
                else
                {
                    fullFilteredSkins = rarityFilteredSkins;
                }

                chunkedSkins = ChunkBy(fullFilteredSkins, 30);

                LoadInventoryItems();
            }
            catch
            { }
        }



        //EVENT HANDLERS
        private void dudInvFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            SortInventory(dudInvSorter.SelectedItem.ToString());
        }

        private void dudIntSorter_SelectedItemChanged(object sender, EventArgs e)
        {
            SortInventory(dudInvSorter.SelectedItem.ToString());
        }

        private void btnInvNext_Click(object sender, EventArgs e)
        {
            invIndex++;
            LoadInventoryItems();
        }

        private void btnInvPrev_Click(object sender, EventArgs e)
        {
            invIndex--;
            LoadInventoryItems();
        }

        private void btnInvSearch_Click(object sender, EventArgs e)
        {
            invFilter = tbInvFilter.Text;
            if (tbInvFilter.Text == "Enter‌ a‌ filter")
            {
                invFilter = null;
            }
            SortInventory(dudInvSorter.SelectedItem.ToString());
        }






        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------Moneyfaucet page------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------

        //FIELDS
        List<List<Moneyfaucet>> faucets;

        List<Panel> MFP;
        List<List<Label>> MFL;
        List<Button> MFB;
        int mfIndex = 0;



        //METHODS
        private void LoadMoneyfaucetPage()
        {
            SetFilterValues();
            MFP = new List<Panel> { pnMoneyfaucet1, pnMoneyfaucet2, pnMoneyfaucet3, pnMoneyfaucet4 };
            MFL = new List<List<Label>> {
                new List<Label> { lblMFName1, lblMFName2, lblMFName3, lblMFName4 },
                new List<Label> { lblMFPrize1, lblMFPrize2, lblMFPrize3, lblMFPrize4 },
                new List<Label> { lblMFAmount1, lblMFAmount2, lblMFAmount3, lblMFAmount4 },
                new List<Label> { lblMFPercentage1, lblMFPercentage2, lblMFPercentage3, lblMFPercentage4 },
                new List<Label> { lblMFCooldown1, lblMFCooldown2, lblMFCooldown3, lblMFCooldown4 },
                new List<Label> { lblMFCIP1, lblMFCIP2, lblMFCIP3, lblMFCIP4 }
            };
            MFB = new List<Button> { btnUseFaucet1, btnUseFaucet2, btnUseFaucet3, btnUseFaucet4 };

            Loadfaucets();
        }

        private void Loadfaucets()
        {
            List<Moneyfaucet> tempFaucets = FilterFaucets(RepoMF.GetAll(user));
            faucets = ChunkBy(tempFaucets, 4);

            lblMoneyfaucetItemAmount.Text = tempFaucets.Count.ToString() + " Faucets";
            lblMoneyfaucetPage.Text = "Page " + (mfIndex + 1) + "/" + faucets.Count.ToString();

            foreach (Panel panel in MFP)
            {
                panel.Visible = false;
            }

            if (faucets.Count > mfIndex + 1)
            { btnMFNext.Visible = true; }
            else { btnMFNext.Visible = false; }

            if (mfIndex > 0)
            { btnMFPrev.Visible = true; }
            else { btnMFPrev.Visible = false; }

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    MFL[0][i].Text = faucets[mfIndex][i].Name;
                    MFL[1][i].Text = faucets[mfIndex][i].MinWin.ToString() + "$ - " + faucets[mfIndex][i].MaxWin.ToString() + "$";
                    MFL[2][i].Text = faucets[mfIndex][i].MinSkinAmount.ToString() + " - " + faucets[mfIndex][i].MaxSkinAmount.ToString();
                    MFL[3][i].Text = faucets[mfIndex][i].WinPercentage.ToString() + "%";

                    TimeSpan cd = new TimeSpan(0, faucets[mfIndex][i].DefaultCooldown, 0);
                    if (cd.Days > 0)
                    { MFL[4][i].Text = cd.Days.ToString() + " day(s)"; }
                    else if (cd.Hours > 0)
                    { MFL[4][i].Text = cd.Hours.ToString() + " hour(s)"; }
                    else if (cd.Minutes > 0)
                    { MFL[4][i].Text = cd.Minutes.ToString() + " Min"; }

                    TimeSpan cip = new TimeSpan(0, faucets[mfIndex][i].DefaultCooldown * faucets[mfIndex][i].CIP, 0);
                    if (cip.Days > 0)
                    { MFL[5][i].Text = cip.Days.ToString() + " day(s)"; }
                    else if (cip.Hours > 0)
                    { MFL[5][i].Text = cip.Hours.ToString() + " hour(s)"; }
                    else if (cip.Minutes > 0)
                    { MFL[5][i].Text = cip.Minutes.ToString() + " Min"; }
                    
                    if (faucets[mfIndex][i].LastCooldown > DateTime.Now)
                    { MFB[i].BackColor = Color.FromArgb(0, 34, 34); }
                    else { MFB[i].BackColor = Color.FromArgb(0, 64, 64); }

                    MFP[i].Visible = true;
                }
                catch { }
            }
        }

        private List<Moneyfaucet> FilterFaucets(List<Moneyfaucet> faucets)
        {
            List<Moneyfaucet> newfaucets = new List<Moneyfaucet>();
            foreach (Moneyfaucet faucet in faucets)
            {
                if (faucet.MinSkinAmount <= nudMaxSkinAmount.Value && faucet.MaxSkinAmount >= nudMinSkinAmount.Value)
                {
                    if (faucet.MinWin <= nudMaxRewardValue.Value && faucet.MaxWin >= nudMinRewardValue.Value)
                    {
                        if (lblWinPercentage.Text == "<")
                        {
                            if (faucet.WinPercentage <= nudWinPercentage.Value)
                            {
                                newfaucets.Add(faucet);
                            }
                        }
                        else
                        {
                            if (faucet.WinPercentage >= nudWinPercentage.Value)
                            {
                                newfaucets.Add(faucet);
                            }
                        }
                    }
                }
            }
            return newfaucets;
        }

        private void SetFilterValues()
        {
            List<Moneyfaucet> moneyfaucets = RepoMF.GetAll(user);
            foreach (Moneyfaucet faucet in moneyfaucets)
            {
                if (faucet.MinWin < nudMinRewardValue.Value)
                {
                    nudMinRewardValue.Value = faucet.MinWin;
                }
                if (faucet.MaxWin > nudMaxRewardValue.Value)
                {
                    nudMaxRewardValue.Value = faucet.MaxWin;
                }
                if (faucet.MinSkinAmount < nudMinSkinAmount.Value)
                {
                    nudMinSkinAmount.Value = faucet.MinSkinAmount;
                }
                if (faucet.MaxSkinAmount > nudMaxSkinAmount.Value)
                {
                    nudMaxSkinAmount.Value = faucet.MaxSkinAmount;
                }
            }
            lblWinPercentage.Text = ">";
            nudWinPercentage.Value = 0;
        }

        private void UseFaucet(int index)
        {
            //If there is no cooldown
            if (faucets[mfIndex][index].LastCooldown < DateTime.Now)
            {
                //Use the faucet and put winnings in list
                List<Skin> skins = faucets[mfIndex][index].UseFaucet();
                //Check if the user got anny winnings
                if (skins != null)
                {
                    //cooldown the user and add skins to user's inventory
                    DateTime newCooldown = DateTime.Now + (new TimeSpan(0, faucets[mfIndex][index].DefaultCooldown, 0));
                    RepoUser.UpdateCooldown(user, faucets[mfIndex][index], newCooldown);
                    faucets[mfIndex][index].LastCooldown = newCooldown;

                    RepoSkin.InsertSkins(skins, user, false);

                    MessageBox.Show("You won! Skins have been added to your inventory");
                }
                else
                {
                    //cooldown the user and display message
                    DateTime newCooldown = DateTime.Now + (new TimeSpan(0, faucets[mfIndex][index].DefaultCooldown * faucets[mfIndex][index].CIP, 0));
                    RepoUser.UpdateCooldown(user, faucets[mfIndex][index], newCooldown);
                    faucets[mfIndex][index].LastCooldown = newCooldown;

                    MessageBox.Show("You lost");
                }
                Loadfaucets();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append((faucets[mfIndex][index].LastCooldown - DateTime.Now).Days + " Days, ");
                sb.Append((faucets[mfIndex][index].LastCooldown - DateTime.Now).Hours + " Hours, ");
                sb.Append((faucets[mfIndex][index].LastCooldown - DateTime.Now).Minutes + " Minutes and ");
                sb.Append((faucets[mfIndex][index].LastCooldown - DateTime.Now).Seconds + " Seconds");
                MessageBox.Show("There is a cooldown of " + sb.ToString() +  " on this moneyfaucet");
            }
                
        }



        //EVENT HANDLERS
        private void btnMFNext_Click(object sender, EventArgs e)
        {
            mfIndex++;
            Loadfaucets();
        }
        private void btnMFPrev_Click(object sender, EventArgs e)
        {
            mfIndex--;
            Loadfaucets();
        }
        
        private void btnToggleWinPercentage_Click(object sender, EventArgs e)
        {
            if (lblWinPercentage.Text == "<")
            {
                lblWinPercentage.Text = ">";
            }
            else
            {
                lblWinPercentage.Text = "<";
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Loadfaucets();
        }
        
        private void btnUseFaucet1_Click(object sender, EventArgs e)
        {
            UseFaucet(0);
        }
        private void btnUseFaucet2_Click(object sender, EventArgs e)
        {
            UseFaucet(1);
        }
        private void btnUseFaucet3_Click(object sender, EventArgs e)
        {
            UseFaucet(2);
        }
        private void btnUseFaucet4_Click(object sender, EventArgs e)
        {
            UseFaucet(3);
        }








        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------Style-----------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------

        //MENUBAR NAVIGATION
        private void btnHome_Click(object sender, EventArgs e)
        {
            loadedPage = 1;
            this.BackgroundImage = Image.FromFile(@"C:\Users\jelle\Desktop\S2 herstart\Periode 3\Killerapp\Applicaties\ClientApp\CSGOClient\CSGOClient\Images\MainBackground.png");
            pnInv.Visible = false;
            pnMoneyfaucet.Visible = false;
        }
        private void btnInventory_Click(object sender, EventArgs e)
        {
            if (loadedPage != 2)
            {
                loadedPage = 2;
                this.BackgroundImage = Image.FromFile(@"C:\Users\jelle\Desktop\S2 herstart\Periode 3\Killerapp\Applicaties\ClientApp\CSGOClient\CSGOClient\Images\Inventory.png");
                pnInv.Visible = true;
                pnMoneyfaucet.Visible = false;
                LoadInventoryPage();
            }
        }
        private void btnMoneyfaucet_Click(object sender, EventArgs e)
        {
            loadedPage = 3;
            this.BackgroundImage = Image.FromFile(@"C:\Users\jelle\Desktop\S2 herstart\Periode 3\Killerapp\Applicaties\ClientApp\CSGOClient\CSGOClient\Images\Moneyfaucet.png");
            pnInv.Visible = false;
            pnMoneyfaucet.Visible = true;
            LoadMoneyfaucetPage();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        //FORMS STYLE
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void Common_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatStyle = FlatStyle.Popup;
        }
        private void Common_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatStyle = FlatStyle.Flat;
        }
        private void Common_RemoveActiveControl(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void tbInvFilter_Enter(object sender, EventArgs e)
        {
            if (tbInvFilter.Text == "Enter‌ a‌ filter")
            {
                tbInvFilter.Clear();
                tbInvFilter.ForeColor = Color.FromArgb(153, 180, 209);
            }
        }
        private void tbInvFilter_Leave(object sender, EventArgs e)
        {
            if (tbInvFilter.Text.Length < 1)
            {
                tbInvFilter.Text = "Enter‌ a‌ filter";
                tbInvFilter.ForeColor = Color.FromArgb(70, 90, 130);
            }
        }

        private void Common_MouseHover(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    if (panel == invSlots[i])
                    {
                        if (chunkedSkins[invIndex][i] != null)
                        {
                            pnPopup.Visible = true;
                            pnPopup.BringToFront();
                            pnPopup.Location = this.PointToClient(Cursor.Position);

                            Skin s = chunkedSkins[invIndex][i];

                            pnPopup.BackColor = Color.FromArgb(s.Rarity.ColorR, s.Rarity.ColorG, s.Rarity.ColorB);
                            lblPopupName.Text = s.Weapon + " | " + s.Name;
                            lblPopupRarity.Text = s.Rarity.Name;
                            lblPopupWear.Text = "Wear: " + s.GetWear();
                            lblPopupFloat.Text = "Float: " + s.SkinFloat;
                            lblPopupCollection.Text = "Collection: " + s.Collection;
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void Common_MouseMove(object sender, MouseEventArgs e)
        {
            if (pnPopup.Visible && pnPopup.Location != this.PointToClient(Cursor.Position))
            {
                pnPopup.Visible = false;
                pnPopup.Location = new Point(20, 20);
                LoadInventoryItems();
            }
        }
    }
}
