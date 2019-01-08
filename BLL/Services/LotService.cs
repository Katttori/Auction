using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Exceptions;
using System.Threading;

namespace BLL.Services
{
    public class LotService : ILotService
    { 
        private readonly IUnitOfWork database;
        
        public LotService(IUnitOfWork uow)
        {
            database = uow;
        }

        public void CreateLot(int productId)
        {
            var product = database.Products.Get(productId);
            if (product == null)
                throw new NotFoundException();
            if (!product.IsConfirmed)
                throw new InvalidOperationException("Not confirmed");
            var alreadyLot = database.Lots.Find(x => x.ProductID == product.Id).FirstOrDefault();// == null ? false : true;
           
            if (alreadyLot != null)
                throw new InvalidOperationException("Product already in lot");
            if (product.IsSold)
                throw new InvalidOperationException("Already sold");
            Lot lot = new Lot
            {
                Product = product,
                BiddingStart = DateTime.Now,
                BiddingEnd = DateTime.Now.AddDays(14),
                ActualPrice = product.Price,
                Winner = product.Owner
            };
            database.Lots.Create(lot);
        }

        public void Dispose()
        {
            database.Dispose();
        }

        public void EditLot(LotDTO newLot)
        {
            if (newLot == null)
                throw new ArgumentNullException();
            var lot = database.Lots.Get(newLot.Id);
            if (lot == null)
                throw new NotFoundException();

            lot.BiddingEnd = newLot.BiddingEnd;
            database.Lots.Update(lot);
        }

        public  void EndBidding(int LotId)
        {
            var lot = database.Lots.Get(LotId);
            if (lot == null)
                throw new NotFoundException();
            if (lot.Product.IsSold)
                throw new InvalidOperationException("Cant end bidding on sold lot");
            if (lot.Winner != lot.Product.Owner)
            {
                lot.Winner.WonLots.Add(lot);
                lot.Product.IsSold = true;
            }
            else
                RemoveLot(LotId);
            database.Save();
        }

        public IEnumerable<LotDTO> GetActiveLots()
        {
            var lots = database.Lots.Find(lot => lot.BiddingEnd > DateTime.Now).ToList();
            return Mapper.Map<IEnumerable<Lot>, List<LotDTO>>(lots);
        }

        public IEnumerable<LotDTO> GetAllLots()
        {
            var lots = database.Lots.GetAll().ToList();
            return Mapper.Map<IEnumerable<Lot>, List<LotDTO>>(lots);
        }

        public LotDTO GetLot(int id)
        {
            var lot = database.Lots.Get(id);
            if (lot == null)
                throw new NotFoundException();
            return Mapper.Map<Lot, LotDTO>(lot);
        }

        public IEnumerable<LotDTO> GetLotsWithCategory(int categoryId)
        {
            var category = database.Categories.Get(categoryId);
            if (category == null)
                throw new NotFoundException();
            var lots = database.Lots.Find(lot => lot.Product.CategoryID == categoryId && !lot.Product.IsSold).ToList();
            return Mapper.Map<IEnumerable<Lot>, List<LotDTO>>(lots);
        }

        public IEnumerable<LotDTO> GetWonLots(string userId)
        {
            var user = database.Users.Get(userId);
            if (user == null)
                throw new NotFoundException();
            var wonLots = user.WonLots.Where(x => x.BiddingEnd < DateTime.Now); 
            return Mapper.Map<IEnumerable<Lot>, List<LotDTO>>(wonLots);
        }

        public void MakeBet(string userId, int lotId, decimal bet)
        {
            var user = database.Users.Get(userId);
            var lot = database.Lots.Get(lotId);
            if (user == null || lot == null)
                throw new NotFoundException();
            if (lot.Product.IsSold)
                throw new InvalidOperationException("Cant make bets on sold lot");
            lot.ActualPrice += bet;
            lot.Winner = user;
            database.Save();
        }

        public void RemoveLot(int id)
        {
            var lot = database.Lots.Get(id);
            if (lot == null)
                throw new NotFoundException();
            database.Lots.Delete(id);
        }
        
        //async Task EndBiddingAutomaticly(DateTime endDateTime, int id)
        //{
        //    var now = DateTime.Now;
        //    if (endDateTime > now)
        //        await Task.Delay(endDateTime - now);
        //    try
        //    {
        //        EndBidding(id);
        //    }
        //    catch (Exception)
        //    { }
        //}
    }
}
