﻿using PdaHub.Models;
using System;
using System.Threading.Tasks;

namespace PdaHub.Services
{
    public partial class StockService
    {
        private delegate Task<SucessResponseModel<StockInOutDetailModel>> SendUpdateDelegate();
        private async Task<SucessResponseModel<StockInOutDetailModel>> TryCatch(SendUpdateDelegate model)
        {
            try
            {
                return await model();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
