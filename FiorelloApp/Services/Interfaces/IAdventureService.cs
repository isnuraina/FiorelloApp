using FiorelloApp.Models;
using FiorelloApp.ViewModels;

namespace FiorelloApp.Services.Interfaces
{
    public interface IAdventureService
    {
        int GetBasketCountAdventure();
        List<Adventure> GetBasketListAdventure();
        int GetBasketTotalAdventure();
    }
}
