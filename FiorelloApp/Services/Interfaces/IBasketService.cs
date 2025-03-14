using FiorelloApp.ViewModels;

namespace FiorelloApp.Services.Interfaces
{
    public interface IBasketService
    {
        int GetBasketCount();
        List<BasketVM> GetBasketList();
        int GetBasketTotal();
    }
}
