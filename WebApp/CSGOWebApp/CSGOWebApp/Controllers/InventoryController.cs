using Models;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSGOWebApp.Controllers
{
    public class InventoryController : Controller
    {
        SkinRepository skinRepository = new SkinRepository(SkinFactory.GetContext(ContextType.MSSQL));
        
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                User user = (User)Session["User"];
                TradeViewModel withdrawViewModel = new TradeViewModel(24);

                withdrawViewModel.Inventory = skinRepository.GetAllFromInv(user, true);
                withdrawViewModel.SortInventory();
                Session["WVM"] = withdrawViewModel;
                return View(withdrawViewModel);
            }
            catch
            {
                return RedirectToAction("Index", "Account");
            }
        }


        [HttpPost]
        public ActionResult Index(TradeViewModel withdrawViewModel)
        {
            TradeViewModel sessionWithdrawViewModel = (TradeViewModel)Session["WVM"];
            sessionWithdrawViewModel.RarityFilter = withdrawViewModel.RarityFilter;
            sessionWithdrawViewModel.SortOption = withdrawViewModel.SortOption;
            sessionWithdrawViewModel.TextFilter = withdrawViewModel.TextFilter;
            sessionWithdrawViewModel.SelectedSkins = withdrawViewModel.SelectedSkins;

            if (Request.Form["ApplyFilters"] != null)
            {
                sessionWithdrawViewModel.SortInventory();
                sessionWithdrawViewModel.Index = 0;
            }
            else
            {
                foreach (Skin skin in sessionWithdrawViewModel.OrderedSkins[sessionWithdrawViewModel.Index])
                {
                    if (sessionWithdrawViewModel.SelectedSkins.Contains(skin.ID))
                    {
                        if (sessionWithdrawViewModel.FinalSkinList.Contains(skin))
                        { }
                        else
                        {
                            sessionWithdrawViewModel.FinalSkinList.Add(skin);
                        }
                    }
                    else
                    {
                        if (sessionWithdrawViewModel.FinalSkinList.Contains(skin))
                        {
                            sessionWithdrawViewModel.FinalSkinList.Remove(skin);
                        }
                    }
                }

                if (Request.Form["Next"] != null)
                {
                    sessionWithdrawViewModel.Index++;
                }
                else if (Request.Form["Previous"] != null)
                {
                    sessionWithdrawViewModel.Index--;
                }
                else if (Request.Form["Submit"] != null)
                {
                    Session["WithdrawList"] = sessionWithdrawViewModel.FinalSkinList;
                    return RedirectToAction("Confirm");
                }
                else
                {
                    return View("Index", "Home");
                }

                sessionWithdrawViewModel.SelectedSkins = new List<int>();

                int x = 0;
                foreach (Skin skin in sessionWithdrawViewModel.OrderedSkins[sessionWithdrawViewModel.Index])
                {
                    if (sessionWithdrawViewModel.FinalSkinList.Any(m => m.ID == skin.ID))
                    {
                        sessionWithdrawViewModel.SelectedSkins.Add(skin.ID);
                    }
                    else
                    {
                        sessionWithdrawViewModel.SelectedSkins.Add(0);
                    }
                    x++;
                }
            }
                

            Session["WVM"] = sessionWithdrawViewModel;
            return View("Index", sessionWithdrawViewModel);
        }


        public ActionResult Confirm()
        {
            List<Skin> skins = (List<Skin>)Session["WithdrawList"];
            return View(skins);
        }


        public ActionResult Withdraw()
        {
            skinRepository.TransferSkins((User)Session["User"], (List<Skin>)Session["WithdrawList"], false);
            Session.Remove("WithdrawList");
            Session.Remove("WVM");
            return RedirectToAction("Index", "Home");
        }
    }
}