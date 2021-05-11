// SalesOrderManger.cs
// Copyright © 2018-2021 NextStep IT Training. All rights reserved.
//

using System;
using TC3.Models;
using EveryoneIsAuthorized.Client;

namespace TC3.Services {

    public class SalesOrderManager {

        private AlwaysAuthorize authorizer;

        public SalesOrderManager() {

            authorizer = new AlwaysAuthorize();
        }

        public string CompletePurchase(SalesOrder salesOrder, CardInfo cardInfo) {

            // This is a controller that follows four steps to complete the sale.

            string authorizationCode = authorizer.Authorize((double)salesOrder.Total, cardInfo.Number);

            if (authorizationCode != null) {

                UpdateSalesOrder(salesOrder, cardInfo, authorizationCode);
            }

            return authorizationCode;
        }

        private string MaskCardNumber(string cardNumber) {

            string result = cardNumber;

            if (result.Length > 4) {

                // The String constructor can repeat a sequence!

                result = (new String('*', result.Length - 4)) + result.Substring(result.Length - 4);
            }

            return result;
        }

        private void UpdateSalesOrder(SalesOrder salesOrder, CardInfo cardInfo, string authorizationCode) {

            salesOrder.CardNumber = MaskCardNumber(cardInfo.Number);
            salesOrder.CardExpires = cardInfo.Expires;
            salesOrder.CardAuthorized = authorizationCode;
            salesOrder.Filled = DateTime.Now;
        }
    }
}
