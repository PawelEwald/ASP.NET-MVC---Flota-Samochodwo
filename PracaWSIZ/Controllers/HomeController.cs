using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using PracaWSIZ.Helpers;
using PracaWSIZ.Models;

namespace PracaWSIZ.Controllers
{
    public class HomeController : Controller
    {


        //----------------------------------------------------------------------------------------
        //AwariaMechaniczne

        public ActionResult AwariaMechaniczna(int id)
        {
            ViewBag.idSamochodu = id;
            SamochodyPDEntities db = new SamochodyPDEntities();
            AwariaMechaniczna viewModel = new AwariaMechaniczna();
            var sumaCena = db.AwariaMechaniczna;

            ViewBag.koszt = (from m in sumaCena
                             where m.id_samochod == id
                             select m.cena).DefaultIfEmpty().Sum();


            ViewBag.lista = db.AwariaMechaniczna.Where(a => a.id_samochod == id);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AwariaMechaniczna(AwariaMechaniczna model, int id)
        {
            if (ModelState.IsValid)
            {
                SamochodyPDEntities db = new SamochodyPDEntities();
                db.Entry(model).State = EntityState.Added;
                model.id_samochod = id;
                db.SaveChanges();
                return RedirectToAction("AwariaMechaniczna", id);
            }
            return RedirectToAction("AwariaMechaniczna",id);
        }

        //----------------------------------------------------------------------------------------
        //AwariaBlacharska

        public ActionResult AwariaBlacharska(int id)
        {
            ViewBag.idSamochodu = id;
            SamochodyPDEntities db = new SamochodyPDEntities();
            AwariaBlacharska viewModel = new AwariaBlacharska();

            var sumaCena = db.AwariaBlacharska;

            ViewBag.koszt = (from m in sumaCena
                             where m.id_samochod == id
                             select m.cena).DefaultIfEmpty().Sum();

            ViewBag.lista = db.AwariaBlacharska.Where(a => a.id_samochod == id);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AwariaBlacharska(AwariaBlacharska model, int id)
        {
            if (ModelState.IsValid)
            {
                SamochodyPDEntities db = new SamochodyPDEntities();
                db.Entry(model).State = EntityState.Added;
                model.id_samochod = id;
                db.SaveChanges();
                return RedirectToAction("AwariaBlacharska", id);
            }

            return RedirectToAction("AwariaBlacharska", id);
        }


        //----------------------------------------------------------------------------------------
        //DodajCzynnosciDoAuta


        public ActionResult DodajCzynnosciDoAuta(int id)
        {
            ViewBag.idSamochodu = id;
            SamochodyPDEntities db = new SamochodyPDEntities();
            DetailsViewModel viewModel = new DetailsViewModel();
            viewModel.czynnosciAcutCoDotyczyLista = db.CzynnosciAutoCoDotyczy.Where(a => a.id_samochod == id);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DodajCzynnosciDoAuta(DetailsViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                SamochodyPDEntities db = new SamochodyPDEntities();
                

                var nowySamochod = db.Samochody;
                var dataKupna = (from ns in nowySamochod.Where(a => a.id == id)
                                 select ns.dataRejestracji).First();

                CzynnosciAutoCoDotyczy czynnosciAutoCoDotyczy = new CzynnosciAutoCoDotyczy();
                czynnosciAutoCoDotyczy.id_samochod = id;
                czynnosciAutoCoDotyczy.czynnoscKM = model.CzynnosciAutoCoDotyczyVM.czynnoscKM;
                czynnosciAutoCoDotyczy.czynnoscMiesiace = model.CzynnosciAutoCoDotyczyVM.czynnoscMiesiace;
                czynnosciAutoCoDotyczy.nazwaCzynnosci = model.CzynnosciAutoCoDotyczyVM.nazwaCzynnosci;
                db.CzynnosciAutoCoDotyczy.Add(czynnosciAutoCoDotyczy);
                db.SaveChanges();

                var czWyk = db.CzynnosciAutoCoDotyczy;
                int idCzynnosciWykonane = (from cw in czWyk
                                           select cw.id).DefaultIfEmpty().Max();

                CzynnosciWykonane czynnosciWykonane = new CzynnosciWykonane();
                czynnosciWykonane.id_czynnosc = (idCzynnosciWykonane);
                czynnosciWykonane.stanKm = 0;
                czynnosciWykonane.stanData = dataKupna;
                czynnosciWykonane.cena = 0;
                czynnosciWykonane.opis = "data pierwszej rejestracji";
                db.CzynnosciWykonane.Add(czynnosciWykonane);

                db.SaveChanges();
                return RedirectToAction("DodajCzynnosciDoAuta");
            }
            return View(model);
        }


        //----------------------------------------------------------------------------------------
        //TankowanieSamochodu

        public ActionResult TankowanieSamochodu(int id_samochod)
        {
            SamochodyPDEntities db = new SamochodyPDEntities();
            var tankowanieLitry = db.Tankowanie;


            int? stanKmMax = (from tank in tankowanieLitry
                              where tank.id_samochod == id_samochod
                              select tank.Km).DefaultIfEmpty().Max();

            ViewBag.stanKmMax = stanKmMax;
            return View();
        }

        [HttpPost]
        public ActionResult TankowanieSamochodu(Tankowanie tankowanie, int id = 2)
        {
            if (ModelState.IsValid)
            {
                SamochodyPDEntities db = new SamochodyPDEntities();
                var tankowanieLitry = db.Tankowanie;


                int? stanKmMax = (from tank in tankowanieLitry
                                   where tank.id_samochod == tankowanie.id_samochod
                                   select tank.Km).DefaultIfEmpty().Max();


                var helper = new RefuelingHelper();

                bool isMilageOk = helper.IsMilageOk(stanKmMax, tankowanie.Km);

                if (isMilageOk)
                {
                    db.Tankowanie.Add(tankowanie);
                    db.SaveChanges();
                    return RedirectToAction("SamochodyLista");
                }
                else
                {
                    ViewBag.Przebieg = "Ostatnio podany przebieg był wyższy od obecnie podanego. Popraw to!!";
                }



            }
            return View(tankowanie);
        }


        //----------------------------------------------------------------------------------------
        //Delete



        public ActionResult Delete(int id = 0)
        {
            SamochodyPDEntities db = new SamochodyPDEntities();
            Sprzedane sprzedane = db.Sprzedane.Find(id);
            sprzedane.sprzedane1 = false;
            db.SaveChanges();
            return RedirectToAction("SamochodyLista");
        }

        //----------------------------------------------------------------------------------------
        //Edit
        public ActionResult Edit(int id = 0)
        {
            SamochodyPDEntities db = new SamochodyPDEntities();
            Samochody samochody = db.Samochody.Find(id);
            if (samochody == null)
            {
                return HttpNotFound();
            }
            return View(samochody);
        }

        [HttpPost]
        public ActionResult Edit(Samochody samochody)
        {
            if (ModelState.IsValid)
            {
                SamochodyPDEntities db = new SamochodyPDEntities();
                db.Entry(samochody).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SamochodyLista");
            }
            return View(samochody);
        }

        //----------------------------------------------------------------------------------------
        //CreateNewCar

        public ActionResult CreateNewCar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNewCar(SamochodyTankowanieVievModel model)
        {
            if (!ModelState.IsValid)
                return View("CreateNewCar", model);
            else
            {
                SamochodyPDEntities db = new SamochodyPDEntities();

                Samochody sam = new Samochody();
                Tankowanie tank = new Tankowanie();

                int id_samochody_Max = db.Samochody.Max(d => d.id) + 1;

                sam.marka = model.samochodyVM.marka;
                sam.model = model.samochodyVM.model;
                sam.nrRejestracyjny = model.samochodyVM.nrRejestracyjny;
                sam.nazwozie = model.samochodyVM.nazwozie;
                sam.paliwo = model.samochodyVM.paliwo;
                sam.konie = model.samochodyVM.konie;
                sam.pojemnosc = model.samochodyVM.pojemnosc;
                sam.rokProd = model.samochodyVM.rokProd;
                sam.dataRejestracji = model.samochodyVM.dataRejestracji;

                tank.Km = model.tankowanieVM.Km;
                tank.litry = 0;
                tank.cena = 0;
                tank.id_samochod = id_samochody_Max;
                tank.Km = model.tankowanieVM.Km;

                db.Samochody.Add(sam);
                db.Tankowanie.Add(tank);

                db.SaveChanges();

                return RedirectToAction("SamochodyLista");
            }
        }

        //----------------------------------------------------------------------------------------
        //SamochodyLista

        public ActionResult SamochodyLista()
        {
            SamochodyPDEntities db = new SamochodyPDEntities();
            var samochody = db.Samochody;
            var sprzedane = db.Sprzedane;

            var samochodySprzedaneViewModel = from s in samochody
                                              join sp in sprzedane on s.id equals sp.id_samochod
                                              where sp.sprzedane1 == true
                                              orderby s.marka, s.model, s.rokProd ascending
                                              select new SamochodySprzedaneViewModel { samochodyVM = s, sprzedaneVM = sp };
            return View(samochodySprzedaneViewModel);
        }

        //----------------------------------------------------------------------------------------
        //Details

        public ActionResult Details(int id)
        {
            SamochodyPDEntities db = new SamochodyPDEntities();
            var litry = db.Tankowanie.Where(d => d.id_samochod == id).DefaultIfEmpty().Sum(d => d.litry);
            var kmMax = db.Tankowanie.Where(d => d.id_samochod == id).DefaultIfEmpty().Max(d => d.Km);
            var kmMin = db.Tankowanie.Where(d => d.id_samochod == id).DefaultIfEmpty().Min(d => d.Km);

            ViewBag.idSamochodu = id;
            ViewBag.spalanie = Math.Round(litry / (kmMax - kmMin) * 100 * 10) / 10;

            var tankowanie = db.Tankowanie;
            var czynnosciAuto = db.CzynnosciAutoCoDotyczy;
            var czynnosciWykonane = db.CzynnosciWykonane;


            DateTime lokalnaData = DateTime.UtcNow.ToLocalTime();


            var viewModel_Tabela = from ca in czynnosciAuto.Where(x => x.id_samochod == id)
                                   join cw in (
                                       from u in czynnosciWykonane
                                       group u by u.id_czynnosc into g
                                       select new
                                       {
                                           g.Key,
                                           uKm = g.Max(p => p.stanKm),
                                           uData = g.Max(p => p.stanData)
                                       }
                                   ) on ca.id equals cw.Key
                                   join t in (
                                      from u in tankowanie
                                      group u by u.id_samochod into g
                                      select new
                                      {
                                          g.Key,
                                          uuKm = g.Max(p => p.Km)
                                      }
                                   ) on ca.id_samochod equals t.Key

                                   select new ToDoList
                                   {
                                       nazwaCzynnosci = ca.nazwaCzynnosci,
                                       coIleKm = ca.czynnoscKM,
                                       coIleMcy = ca.czynnoscMiesiace,
                                       max_Km = cw.uKm,
                                       max_Data = cw.uData,
                                       stan_Km = t.uuKm,
                                       doZrobieniaKm = ca.czynnoscKM - (t.uuKm - cw.uKm),
                                       doZrobieniaMcy = ca.czynnoscMiesiace
                                                      - (12*(lokalnaData.Year - cw.uData.Value.Year)
                                                      + (lokalnaData.Month - cw.uData.Value.Month))

                                   };

 
            ViewBag.Tabela_ZrobioneOstatnio = viewModel_Tabela;



            //dropdownlist
            int selectedId = 1;
            ViewBag.Czynnosci = new SelectList(db.CzynnosciAutoCoDotyczy.Where(m => m.id_samochod == id).DefaultIfEmpty(), "id", "nazwaCzynnosci", selectedId);



            return View();


        }
        [HttpPost]
        public ActionResult Details(DetailsViewModel model, int id)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", id);
            }

            else
            {
                SamochodyPDEntities db = new SamochodyPDEntities();

                //CzynnosciWykonane czynnosci = new CzynnosciWykonane();
                //CzynnosciAutoCoDotyczy czynnCoDotyczy = new CzynnosciAutoCoDotyczy();

                db.CzynnosciWykonane.Add(model.czynnosciWykonane);

                db.SaveChanges();
                return RedirectToAction("Details", id);
            }
        }

        //----------------------------------------------------------------------------------------
        //Login

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Uzytkownik objUser)
        {
            if (ModelState.IsValid)
            {
                using (SamochodyPDEntities db = new SamochodyPDEntities())
                {
                    var obj = db.Uzytkownik.Where(a => a.uzytkownik1.Equals(objUser.uzytkownik1) && a.haslo.Equals(objUser.haslo)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.id.ToString();
                        Session["UserName"] = obj.uzytkownik1.ToString();
                        Session["Imie"] = obj.imie.ToString();
                        Session["Nazwisko"] = obj.nazwisko.ToString();
                        return RedirectToAction("UserDashBoard");
                    }
                }
            }
            return View(objUser);
        }

        //----------------------------------------------------------------------------------------
        //Logout

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }


        //----------------------------------------------------------------------------------------
        //UserDashBoard


        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        //----------------------------------------------------------------------------------------
        //Index


        public ActionResult Index()
        {
            SamochodyPDEntities db = new SamochodyPDEntities();
            var posiadaneSamochody = db.Samochody;
            return View(posiadaneSamochody);
        }


        //----------------------------------------------------------------------------------------
        //Contact


        public ActionResult Contact()
        {
            return View();
        }


    }
}