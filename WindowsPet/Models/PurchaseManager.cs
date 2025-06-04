using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WindowsPet.Models
{
    internal class PurchaseManager
    {
        private static PurchaseManager? _instance;
        public static PurchaseManager? Instance =>_instance ??= new();
        /*
        public async Task? OnPurchasePet(int id)
        {
            decimal price = 0;
            if (AppDbContext.Instance.IsPetOwnByUser(id))
            {
                return;
            }
            if (AppDbContext.Instance.GetPopularPet(id)!=null)
            {
                price = AppDbContext.Instance.GetPopularPet(id)!.Price;
            }
            await JsonSerialize.SerializeAndSendJson(new PetPurchase{
                UserToken = CurrentUser.Token!,
                PetId = id,
                Credit = CurrentUser.Credit,
                Price = price
            });
        }*/
    }
}
