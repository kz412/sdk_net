﻿using System;
using Newtonsoft.Json;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model.Orders
{
    public class OrderCancellation : AbstractOrder
    {
        /// <summary>
        /// Creates an order cancellation 
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="cancelledAt">The date and time when the order was cancelled (optional)</param>
        /// <param name="cancelReason">If the order was cancelled, the value will be one of the following:
        /// "customer": The customer changed or cancelled the order.
        /// "fraud": The order was fraudulent.
        /// "inventory": Items in the order were not in inventory.
        /// "other": The order was cancelled for a reason not in the list above. </param>
        public OrderCancellation(int merchantOrderId, DateTime cancelledAt, string cancelReason) : base(merchantOrderId)
        {
            InputValidators.ValidateDateNotDefault(cancelledAt, "Cancelled At");
            CancelledAt = cancelledAt;
            InputValidators.ValidateValuedString(cancelReason,"Cancel Reason");
            CancelReason = cancelReason;
        }


        [JsonProperty(PropertyName = "cancel_reason", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CancelReason { get; set; }

        [JsonProperty(PropertyName = "cancelled_at", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CancelledAt { get; set; }
    }
}