﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Riskified.NetSDK.Logging;
using Riskified.NetSDK.Exceptions;
using Riskified.NetSDK.Notifications;
using System.Threading.Tasks;

namespace Riskified.SDK.Sample
{
    public class NotificationServerExample
    {
        private static NotificationsHandler notificationServer;

        public static void ReceiveNotificationsExample()
        {
            string merchantNotificationsWebhook = ConfigurationManager.AppSettings["NotificationsWebhookUrl"];
            
            Console.WriteLine("Local Notifications server url set in the config file: " + merchantNotificationsWebhook);
            Console.WriteLine("Press 'r' to register notification webhook, 'u' to UNregister notification webhook, 's' to start the notifications server, 'rs' to register and start the server, else to skip all");
            string key = Console.ReadLine();
            switch(key)
            {
                case "r":
                    RegisterWebhook(merchantNotificationsWebhook);
                    break;
                case "u":
                    UnregisterWebhook();
                    break;
                case "s":
                    StartServer(merchantNotificationsWebhook);
                    break;
                case "rs":
                    RegisterWebhook(merchantNotificationsWebhook);
                    StartServer(merchantNotificationsWebhook);
                    break;
                default:
                    Console.WriteLine("Unknown key - skipping notifications webhook");
                    break;
            }

            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            
        }

        public static void StopNotificationServer()
        {
            // make sure you shut down the notification server on system shut down
            if (notificationServer != null)
                notificationServer.StopReceiveNotifications();
        }

        private static void RegisterWebhook(string merchantNotificationsWebhook)
        {
            string domain = ConfigurationManager.AppSettings["MerchantDomain"];
            string authToken = ConfigurationManager.AppSettings["MerchantAuthenticationToken"];
            string riskifiedHostUrl = ConfigurationManager.AppSettings["RiskifiedHostUrl"];

           
            Console.WriteLine("Trying to register local notifications webhook: " + merchantNotificationsWebhook);
            try
            {
                NotificationRegistrationResult result = NotificationsHandler.RegisterMerchantNotificationsWebhook(riskifiedHostUrl, merchantNotificationsWebhook, authToken, domain);
                if (result.IsSuccessful)
                    Console.WriteLine("Registration successful: " + result.SuccessfulResult.Message);
                else
                {
                    Console.WriteLine("Registration unsuccessful: " + result.FailedResult.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to register notification webhook on riskified server: " + e.Message);
            }
        }

        private static void UnregisterWebhook()
        {
            string domain = ConfigurationManager.AppSettings["MerchantDomain"];
            string authToken = ConfigurationManager.AppSettings["MerchantAuthenticationToken"];
            string riskifiedHostUrl = ConfigurationManager.AppSettings["RiskifiedHostUrl"];

            Console.WriteLine("Trying to unregister any existing notification webhooks");
            try
            {
                NotificationRegistrationResult result = NotificationsHandler.UnRegisterMerchantNotificationWebhooks(riskifiedHostUrl, authToken, domain);
                if (result.IsSuccessful)
                    Console.WriteLine("Unregistration successful: " + result.SuccessfulResult.Message);
                else
                {
                    Console.WriteLine("Unregistration unsuccessful: " + result.FailedResult.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to unregister notification webhook on riskified server: " + e.Message);
            }
        }

        private static void StartServer(string merchantNotificationsWebhook)
        {
            string domain = ConfigurationManager.AppSettings["MerchantDomain"];
            string authToken = ConfigurationManager.AppSettings["MerchantAuthenticationToken"];

            // setup of a notification server listening to incoming notification from riskified
            // the webhook is the url on the local server which the httpServer will be listening at
            // make sure the url is correct (internet reachable ip/address and port, firewall rules etc.)
            notificationServer = new NotificationsHandler(merchantNotificationsWebhook, NotificationReceived, authToken, domain);
            // the call to notifier.ReceiveNotifications() is blocking and will not return until we call StopReceiveNotifications 
            // so we run it on a different task in this example
            var t = new Task(notificationServer.ReceiveNotifications);
            t.Start();
            Console.WriteLine("Notification server up and running and listening to notifications on webhook: " + merchantNotificationsWebhook);
        }

        /// <summary>
        /// A sample notifications callback from the NotificationHandler
        /// Will be called each time a new notification is received at the local webhook
        /// </summary>
        /// <param name="notification">The notification object that was received</param>
        private static void NotificationReceived(Notification notification)
        {
            Console.WriteLine("New " + notification.Status + " Notification Received for order with ID:" + notification.OrderId + " With description: " + notification.Description);
        }
    }
}
