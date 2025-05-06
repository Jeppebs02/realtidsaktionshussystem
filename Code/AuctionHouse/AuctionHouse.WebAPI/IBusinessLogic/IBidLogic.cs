using AuctionHouse.ClassLibrary.Model;
namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IBidLogic
    {
        Task<bool> PlaceBidAsync(Bid bid);
    }
}
