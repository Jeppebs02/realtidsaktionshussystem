using AuctionHouse.ClassLibrary.Model;
namespace AuctionHouse.WebAPI.IBusinessLogic
{
    public interface IBidLogic
    {
        // Remeber that if bid amount == buyoutprice of the auction, the auction is closed and that bid counts as a buyout.
        Task<string> PlaceBidAsync(Bid bid, byte[] expectedAuctionVersion);
    }
}
