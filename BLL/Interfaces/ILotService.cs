using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILotService
    {
        void CreateLot(int productId);
        void RemoveLot(int id);
        void EditLot(LotDTO newLot);
        IEnumerable<LotDTO> GetAllLots();
        IEnumerable<LotDTO> GetActiveLots();
        LotDTO GetLot(int id);
        void MakeBet(string userId, int lotId, decimal bet);
        void EndBidding(int id);
        IEnumerable<LotDTO> GetLotsWithCategory(int categoryId);
        IEnumerable<LotDTO> GetWonLots(string userId);
        Task EndBiddingWhenTimeExpired(int id);
        void Dispose();
    }
}
