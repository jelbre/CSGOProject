using Models;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSGOWebApp.Controllers
{
    public class CoinflipController : Controller
    {
        CoinflipRepository coinflipRepository = new CoinflipRepository(CoinflipFactory.GetContext(ContextType.MSSQL));
        SkinRepository skinRepository = new SkinRepository(SkinFactory.GetContext(ContextType.MSSQL));
        UserRepository userRepository = new UserRepository(UserFactory.GetContext(ContextType.MSSQL));

        // GET: Coinflip
        public ActionResult Index()
        {
            CoinflipViewModel coinflipViewModel = new CoinflipViewModel(coinflipRepository.GetAll());
            return View(coinflipViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                User user = (User)Session["User"];
                CoinflipViewModel coinflipViewModel = new CoinflipViewModel();

                coinflipViewModel.Inventory = skinRepository.GetAllFromInv(user, true);
                coinflipViewModel.OrderedSkins = coinflipViewModel.ChunkBy(coinflipViewModel.Inventory.Skins, 16);
                Session["CVM"] = coinflipViewModel;
                return View(coinflipViewModel);
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpPost]
        public ActionResult Create(CoinflipViewModel coinflipViewModel)
        {
            CoinflipViewModel sessioncoinflipViewModel = (CoinflipViewModel)Session["CVM"];
            sessioncoinflipViewModel.SelectedSkins = coinflipViewModel.SelectedSkins;

            
            foreach (Skin skin in sessioncoinflipViewModel.OrderedSkins[sessioncoinflipViewModel.Index])
            {
                if (sessioncoinflipViewModel.SelectedSkins.Contains(skin.ID))
                {
                    if (sessioncoinflipViewModel.FinalSkinList.Contains(skin))
                    { }
                    else
                    {
                        sessioncoinflipViewModel.FinalSkinList.Add(skin);
                    }
                }
                else
                {
                    if (sessioncoinflipViewModel.FinalSkinList.Contains(skin))
                    {
                        sessioncoinflipViewModel.FinalSkinList.Remove(skin);
                    }
                }
            }

            if (Request.Form["Next"] != null)
            {
                sessioncoinflipViewModel.Index++;
            }
            else if (Request.Form["Previous"] != null)
            {
                sessioncoinflipViewModel.Index--;
            }
            else if (Request.Form["Submit"] != null)
            {
                if (sessioncoinflipViewModel.FinalSkinList.Count > 0)
                {
                    Session["CoinflipCreateList"] = sessioncoinflipViewModel.FinalSkinList;
                }
                else
                {
                    return RedirectToAction("Error", "Coinflip", "You did not select any skins to create a coinflip with.");
                }
                return RedirectToAction("ConfirmCreate");
            }
            else
            {
                return View("Index", "Home");
            }

            sessioncoinflipViewModel.SelectedSkins = new List<int>();

            int x = 0;
            foreach (Skin skin in sessioncoinflipViewModel.OrderedSkins[sessioncoinflipViewModel.Index])
            {
                if (sessioncoinflipViewModel.FinalSkinList.Any(m => m.ID == skin.ID))
                {
                    sessioncoinflipViewModel.SelectedSkins.Add(skin.ID);
                }
                else
                {
                    sessioncoinflipViewModel.SelectedSkins.Add(0);
                }
                x++;
            }


            Session["CVM"] = sessioncoinflipViewModel;
            return View("Create", sessioncoinflipViewModel);
        }

        public ActionResult ConfirmCreate()
        {
            List<Skin> skins = (List<Skin>)Session["CoinflipCreateList"];
            return View(skins);
        }

        public ActionResult Error(string errorMessage)
        {
            return View(errorMessage);
        }

        public ActionResult InsertCoinflip()
        {
            int coinflipID = coinflipRepository.CreateCoinflip((User)Session["USer"], (List<Skin>)Session["CoinflipCreateList"]);
            Session.Remove("CoinflipCreateList");
            Session.Remove("CVM");
            if (coinflipID == -1)
            {
                return RedirectToAction("Error", "Coinflip", "You already have a coinflip. Wait until your last coinflip finishes");
            }
            else
            {
                return RedirectToAction("View", new { ID = coinflipID });
            }
        }


        public ActionResult View(int ID)
        {
            Coinflip coinflip = coinflipRepository.GetByID(ID);
            Session["Coinflip"] = coinflip;
            return View(coinflip);
        }

        [HttpGet]
        public ActionResult Join()
        {
            try
            {
                User user = (User)Session["User"];
                CoinflipViewModel coinflipViewModel = new CoinflipViewModel();

                coinflipViewModel.Inventory = skinRepository.GetAllFromInv(user, true);
                coinflipViewModel.OrderedSkins = coinflipViewModel.ChunkBy(coinflipViewModel.Inventory.Skins, 16);
                Session["CVM"] = coinflipViewModel;
                if (coinflipRepository.PotentialJoin((Coinflip)Session["Coinflip"]))
                {
                    return View(coinflipViewModel);
                }
                else
                {
                    return RedirectToAction("Error", "Coinflip", "Something went wrong when joining the coinflip.");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpPost]
        public ActionResult Join(CoinflipViewModel coinflipViewModel)
        {
            CoinflipViewModel sessioncoinflipViewModel = (CoinflipViewModel)Session["CVM"];
            sessioncoinflipViewModel.SelectedSkins = coinflipViewModel.SelectedSkins;
            sessioncoinflipViewModel.ErrorMessage = null;

            foreach (Skin skin in sessioncoinflipViewModel.OrderedSkins[sessioncoinflipViewModel.Index])
            {
                if (sessioncoinflipViewModel.SelectedSkins.Contains(skin.ID))
                {
                    if (sessioncoinflipViewModel.FinalSkinList.Contains(skin))
                    { }
                    else
                    {
                        sessioncoinflipViewModel.FinalSkinList.Add(skin);
                    }
                }
                else
                {
                    if (sessioncoinflipViewModel.FinalSkinList.Contains(skin))
                    {
                        sessioncoinflipViewModel.FinalSkinList.Remove(skin);
                    }
                }
            }

            if (Request.Form["Next"] != null)
            {
                sessioncoinflipViewModel.Index++;
            }
            else if (Request.Form["Previous"] != null)
            {
                sessioncoinflipViewModel.Index--;
            }
            else if (Request.Form["Submit"] != null)
            {
                Coinflip coinflip = (Coinflip)Session["Coinflip"];
                if (sessioncoinflipViewModel.GetFinalSkinListPrice() >= coinflip.getBetPrice(0) * 0.95M)
                {
                    if (sessioncoinflipViewModel.GetFinalSkinListPrice() <= coinflip.getBetPrice(0) * 1.05M)
                    {
                        Session["CoinflipJoinList"] = sessioncoinflipViewModel.FinalSkinList;
                        return RedirectToAction("ConfirmJoin");
                    }
                    else
                    {
                        sessioncoinflipViewModel.ErrorMessage = "The total price of your selected skins ($" + sessioncoinflipViewModel.GetFinalSkinListPrice() +
                        ") was too high to meet the required maximal price of $" + (coinflip.getBetPrice(0) * 1.05M);
                    }
                }
                else
                {
                    sessioncoinflipViewModel.ErrorMessage = "The total price of your selected skins ($" + sessioncoinflipViewModel.GetFinalSkinListPrice() +
                        ") was too low to meet the required minimal price of $" + (coinflip.getBetPrice(0) * 0.95M);
                }
            }
            else
            {
                return View("Index", "Home");
            }

            sessioncoinflipViewModel.SelectedSkins = new List<int>();

            int x = 0;
            foreach (Skin skin in sessioncoinflipViewModel.OrderedSkins[sessioncoinflipViewModel.Index])
            {
                if (sessioncoinflipViewModel.FinalSkinList.Any(m => m.ID == skin.ID))
                {
                    sessioncoinflipViewModel.SelectedSkins.Add(skin.ID);
                }
                else
                {
                    sessioncoinflipViewModel.SelectedSkins.Add(0);
                }
                x++;
            }


            Session["CVM"] = sessioncoinflipViewModel;
            return View(sessioncoinflipViewModel);
        }

        public ActionResult ConfirmJoin()
        {
            List<Skin> skins = (List<Skin>)Session["CoinflipJoinList"];
            return View(skins);
        }

        public ActionResult JoinCoinflip()
        {
            Coinflip coinflip = (Coinflip)Session["Coinflip"];
            bool joined = coinflipRepository.JoinCoinflip(coinflip, (User)Session["User"], (List<Skin>)Session["CoinflipJoinList"]);
            Session.Remove("CoinflipJoinList");
            Session.Remove("CVM");
            return RedirectToAction("View", new { ID = coinflip.ID });
        }

        public ActionResult GetResult(Coinflip coinflip)
        {
            coinflipRepository.RewardWinner(coinflip);
            return RedirectToAction("View", new { ID = coinflip.ID });
        }
    }
}