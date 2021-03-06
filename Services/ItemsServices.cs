﻿using PdaHub.Models;
using PdaHub.Repositories;
using System;
using System.Threading.Tasks;

namespace PdaHub.Services
{
    public partial class ItemsServices : IItemsServices
    {
        private readonly IItemsRepository _itemsRepository;
        public ItemsServices(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
        public Task<SucessResponseModel<ItemDetailsResponseModel>> GetPosItem(string barcode)
            => TryCatch(async () =>
            {
                barcode = barcode.Trim();
                var dbModel = await _itemsRepository.GetPosItem(barcode);
                var catModel = await _itemsRepository.GetItemSection(barcode);
                var specialItemModel = await _itemsRepository.itemSpecial(barcode);
                ValidatePosItem(dbModel);
                ItemDetailsResponseModel item = new ItemDetailsResponseModel
                {
                    ArabicName = dbModel.a_name,
                    Barcode = dbModel.barcode.Trim(),
                    EnglishName = HasArabicLetters(dbModel.l_name)? string.Empty: dbModel.l_name ,
                    Price = dbModel.sell_price.Value,
                    PrintDate = DateTime.Today,
                    CategoryName = catModel.a_name,
                };
                if (specialItemModel is not null)
                { item.IsSpecial = true; item.Notes = specialItemModel.Notes; }

                SucessResponseModel<ItemDetailsResponseModel> output = new(item, @$"Last price update on {dbModel.last_modified_time.ToShortDateString()}" +
                    $"-{dbModel.last_modified_time.ToShortTimeString()}", MessageType.Information);
                return output;
            });

    }
}
