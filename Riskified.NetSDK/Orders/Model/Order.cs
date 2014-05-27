﻿using System;
using Newtonsoft.Json;
using Riskified.NetSDK.Utils;

namespace Riskified.NetSDK.Orders
{

    public class Order
    {
        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="email">The email used for contact in the order</param>
        /// <param name="customer">The customer information</param>
        /// <param name="paymentDetails">The payment details</param>
        /// <param name="billingAddress">Billing address</param>
        /// <param name="shippingAddress">Shipping address</param>
        /// <param name="lineItems">An array of all products in the order</param>
        /// <param name="shippingLines">An array of all shipping details for the order</param>
        /// <param name="gateway">The payment gateway that was used</param>
        /// <param name="customerBrowserIp">The customer browser ip that was used for the order</param>
        /// <param name="currency">A three letter code (ISO 4217) for the currency used for the payment</param>
        /// <param name="totalPrice">The sum of all the prices of all the items in the order, taxes and discounts included</param>
        /// <param name="createdAt">The date and time when the order was created</param>
        /// <param name="updatedAt">The date and time when the order was last modified</param>
        /// <param name="discountCodes">An array of objects, each one containing information about an item in the order (optional)</param>
        /// <param name="totalDiscounts">The total amount of the discounts on the Order (optional)</param>
        /// <param name="cartToken">Unique identifier for a particular cart or session that is attached to a particular order. The same ID should be passed in the Beacon JS (optional)</param>
        /// <param name="totalPriceUsd">The price in USD (optional)</param>
        /// <param name="closedAt">The date and time when the order was closed. If the order was closed (optional)</param>
        /// <param name="cancelledAt">The date and time when the order was cancelled (optional)</param>
        /// <param name="cancelReason">If the order was cancelled, the value will be one of the following:
        /// "customer": The customer changed or cancelled the order.
        /// "fraud": The order was fraudulent.
        /// "inventory": Items in the order were not in inventory.
        /// "other": The order was cancelled for a reason not in the list above.
        /// (optional)</param>
        public Order(int merchantOrderId, string email, Customer customer, PaymentDetails paymentDetails,
            AddressInformation billingAddress, AddressInformation shippingAddress, LineItem[] lineItems,
            ShippingLine[] shippingLines,
            string gateway, string customerBrowserIp, string currency, double totalPrice, DateTime createdAt,
            DateTime updatedAt,
            DiscountCode[] discountCodes = null, double? totalDiscounts = null, string cartToken = null,
            double? totalPriceUsd = null,
            DateTime? closedAt = null, DateTime? cancelledAt = null, string cancelReason = null)
        {
            InputValidators.ValidatePositiveValue(merchantOrderId,"Merchant Order ID");
            Id = merchantOrderId;
            InputValidators.ValidateObjectNotNull(lineItems,"Line Items");
            LineItems = lineItems;
            InputValidators.ValidateObjectNotNull(shippingLines, "Shipping Lines");
            ShippingLines = shippingLines;
            InputValidators.ValidateObjectNotNull(paymentDetails, "Payment Details");
            PaymentDetails = paymentDetails;
            InputValidators.ValidateObjectNotNull(billingAddress, "Billing Address");
            BillingAddress = billingAddress;
            InputValidators.ValidateObjectNotNull(shippingAddress, "Shipping Address");
            ShippingAddress = shippingAddress;
            InputValidators.ValidateObjectNotNull(customer, "Customer");
            Customer = customer;
            InputValidators.ValidateEmail(email);
            Email = email;
            InputValidators.ValidateIp(customerBrowserIp);
            CustomerBrowserIp = customerBrowserIp;
            InputValidators.ValidateCurrency(currency);
            Currency = currency;
            InputValidators.ValidateZeroOrPositiveValue(totalPrice,"Total Price");
            TotalPrice = totalPrice;
            InputValidators.ValidateValuedString(gateway,"Gateway");
            Gateway = gateway;
            InputValidators.ValidateDateNotDefault(createdAt, "Created At");
            CreatedAt = createdAt;
            InputValidators.ValidateDateNotDefault(updatedAt, "Updated At");
            UpdatedAt = updatedAt;
            
            // optional fields
            DiscountCodes = discountCodes;
            TotalPriceUsd = totalPriceUsd;
            TotalDiscounts = totalDiscounts;
            CartToken = cartToken;
            if (closedAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(closedAt.Value, "Closed At");
                ClosedAt = closedAt;
            }
            if(cancelledAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(cancelledAt.Value, "Cancelled At");
                CancelledAt = cancelledAt;
            }
            CancelReason = cancelReason;

        }

        [JsonProperty(PropertyName = "cancel_reason", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string CancelReason { get; set; }

        [JsonProperty(PropertyName = "cancelled_at", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CancelledAt { get; set; }

        [JsonProperty(PropertyName = "cart_token", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public string CartToken { get; set; }

        [JsonProperty(PropertyName = "closed_at", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ClosedAt { get; set; }

        [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "currency", Required = Required.Always)]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "email", Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "gateway", Required = Required.Always)]
        public string Gateway { get; set; }

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "total_discounts", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalDiscounts { get; set; }

        [JsonProperty(PropertyName = "total_price", Required = Required.Always)]
        public double? TotalPrice { get; set; }

        [JsonProperty(PropertyName = "total_price_usd",Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalPriceUsd { get; set; }

        [JsonProperty(PropertyName = "updated_at", Required = Required.Always)]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "browser_ip", Required = Required.Always)]
        public string CustomerBrowserIp { get; set; }
        
        [JsonProperty(PropertyName = "discount_codes", Required = Required.Default,NullValueHandling = NullValueHandling.Ignore)]
        public DiscountCode[] DiscountCodes { get; set; }

        [JsonProperty(PropertyName = "line_items", Required = Required.Always)]
        public LineItem[] LineItems { get; set; }

        [JsonProperty(PropertyName = "shipping_lines", Required = Required.Always)]
        public ShippingLine[] ShippingLines { get; set; }

        [JsonProperty(PropertyName = "payment_details", Required = Required.Always)]
        public PaymentDetails PaymentDetails { get; set; }

        [JsonProperty(PropertyName = "billing_address", Required = Required.Always)]
        public AddressInformation BillingAddress { get; set; }

        [JsonProperty(PropertyName = "shipping_address", Required = Required.Always)]
        public AddressInformation ShippingAddress { get; set; }

        [JsonProperty(PropertyName = "customer", Required = Required.Always)]
        public Customer Customer { get; set; }
    }

}
