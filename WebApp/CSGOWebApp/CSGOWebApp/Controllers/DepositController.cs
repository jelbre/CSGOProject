using Models;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSGOWebApp.Controllers
{
    public class DepositController : Controller
    {
        SkinRepository skinRepository = new SkinRepository(SkinFactory.GetContext(ContextType.MSSQL));
        
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                User user = (User)Session["User"];
                TradeViewModel depositViewModel = new TradeViewModel(16);

                depositViewModel.Inventory = skinRepository.GetAllFromInv(user, false);
                depositViewModel.SortInventory();
                Session["DVM"] = depositViewModel;
                return View(depositViewModel);
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [HttpPost]
        public ActionResult Index(TradeViewModel depositViewModel)
        {
            TradeViewModel sessionDepositViewModel = (TradeViewModel)Session["DVM"];
            sessionDepositViewModel.RarityFilter = depositViewModel.RarityFilter;
            sessionDepositViewModel.SortOption = depositViewModel.SortOption;
            sessionDepositViewModel.TextFilter = depositViewModel.TextFilter;
            sessionDepositViewModel.SelectedSkins = depositViewModel.SelectedSkins;

            if (Request.Form["ApplyFilters"] != null)
            {
                sessionDepositViewModel.SortInventory();
                sessionDepositViewModel.Index = 0;
            }
            else
            {
                foreach (Skin skin in sessionDepositViewModel.OrderedSkins[sessionDepositViewModel.Index])
                {
                    if (sessionDepositViewModel.SelectedSkins.Contains(skin.ID))
                    {
                        if (sessionDepositViewModel.FinalSkinList.Contains(skin))
                        { }
                        else
                        {
                            sessionDepositViewModel.FinalSkinList.Add(skin);
                        }
                    }
                    else
                    {
                        if (sessionDepositViewModel.FinalSkinList.Contains(skin))
                        {
                            sessionDepositViewModel.FinalSkinList.Remove(skin);
                        }
                    }
                }

                if (Request.Form["Next"] != null)
                {
                    sessionDepositViewModel.Index++;
                }
                else if (Request.Form["Previous"] != null)
                {
                    sessionDepositViewModel.Index--;
                }
                else if (Request.Form["Submit"] != null)
                {
                    Session["DepositList"] = sessionDepositViewModel.FinalSkinList;
                    return RedirectToAction("Confirm");
                }
                else
                {
                    return View("Index", "Home");
                }

                sessionDepositViewModel.SelectedSkins = new List<int>();

                int x = 0;
                foreach (Skin skin in sessionDepositViewModel.OrderedSkins[sessionDepositViewModel.Index])
                {
                    if (sessionDepositViewModel.FinalSkinList.Any(m => m.ID == skin.ID))
                    {
                        sessionDepositViewModel.SelectedSkins.Add(skin.ID);
                    }
                    else
                    {
                        sessionDepositViewModel.SelectedSkins.Add(0);
                    }
                    x++;
                }
            }


            Session["DVM"] = sessionDepositViewModel;
            return View("Index", sessionDepositViewModel);
        }


        public ActionResult Confirm()
        {
            List<Skin> skins = (List<Skin>)Session["DepositList"];
            return View(skins);
        }


        public ActionResult Deposit()
        {
            skinRepository.TransferSkins((User)Session["User"], (List<Skin>)Session["DepositList"], true);
            Session.Remove("DepositList");
            Session.Remove("DVM");
            return RedirectToAction("Index", "Home");
        }
    }
}