using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                    FIELDS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        private int id;
        private long steam64ID;
        private string username;
        private string password;
        private bool nightMode;
        private decimal balance;
        private Inventory steamInventory;
        private Inventory siteInventory;



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                  PROPERTIES                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public long Steam64ID
        {
            get { return steam64ID; }
            set { steam64ID = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public bool NightMode
        {
            get { return nightMode; }
            set { nightMode = value; }
        }
        public decimal Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        public Inventory SteamInventory
        {
            get { return steamInventory; }
            set { steamInventory = value; }
        }
        public Inventory SiteInventory
        {
            get { return siteInventory; }
            set { siteInventory = value; }
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                 CONSTRUCTORS                                                                 //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //Login constructor
        public User(string Username, string Password)
        {
            username = Username;
            password = Password;
        }

        public User(long Steam64ID)
        {
            steam64ID = Steam64ID;
        }

        //Empty constructor
        public User()
        {

        }

        public User(int ID, bool Nightmode, decimal Balance)
        {
            id = ID;
            nightMode = Nightmode;
            balance = Balance;
        }

        //Main constructors
        public User(int ID, string Username, bool Nightmode, decimal Balance)
        {
            id = ID;
            username = Username;
            nightMode = Nightmode;
            balance = Balance;
        }

        public User(int ID, long Steam64ID, string Username, bool Nightmode, decimal Balance)
        {
            id = ID;
            steam64ID = Steam64ID;
            username = Username;
            nightMode = Nightmode;
            balance = Balance;
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                   METHODS                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------//
        public string getProfilePicBySteamID()
        {
            if(steam64ID > 0)
            {
                using (WebClient client = new WebClient())
                {
                    string htmlCode = client.DownloadString("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=DA12070200B49BB935D9ECCAAF3A61E7&steamids=" + Steam64ID);

                    int startIndex = htmlCode.IndexOf("avatarfull\": \"") + 14;
                    int endIndex = htmlCode.IndexOf("\",\n\t\t\t\t\"personastate\"");

                    return htmlCode.Substring(startIndex, (endIndex - startIndex));
                }

            }
            return "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/fe/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_full.jpg";
        }
    }
}
