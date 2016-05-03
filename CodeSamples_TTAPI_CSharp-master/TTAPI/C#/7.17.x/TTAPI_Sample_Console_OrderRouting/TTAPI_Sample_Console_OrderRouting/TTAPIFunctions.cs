// **********************************************************************************************************************
//
//	Copyright © 2005-2013 Trading Technologies International, Inc.
//	All Rights Reserved Worldwide
//
// 	* * * S T R I C T L Y   P R O P R I E T A R Y * * *
//
//  WARNING: This file and all related programs (including any computer programs, example programs, and all source code) 
//  are the exclusive property of Trading Technologies International, Inc. (“TT”), are protected by copyright law and 
//  international treaties, and are for use only by those with the express written permission from TT.  Unauthorized 
//  possession, reproduction, distribution, use or disclosure of this file and any related program (or document) derived 
//  from it is prohibited by State and Federal law, and by local law outside of the U.S. and may result in severe civil 
//  and criminal penalties.
//
// ************************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTAPI_Sample_Console_OrderRouting
{
    using System.Collections.ObjectModel;
    using TradingTechnologies.TTAPI;
    using TradingTechnologies.TTAPI.Tradebook;

    /// <summary>
    /// Main TT API class
    /// </summary>
    class TTAPIFunctions : IDisposable
    {
        /// <summary>
        /// Declare the TTAPI objects
        /// </summary>
        private UniversalLoginTTAPI m_apiInstance = null;
        private WorkerDispatcher m_disp = null;
        private bool m_disposed = false;
        private object m_lock = new object();
        private InstrumentLookupSubscription m_req = null;
        private PriceSubscription m_ps = null;
        private InstrumentTradeSubscription m_its = null;
        private string m_orderKey = "";
        private string m_username = "";
        private string m_password = "";

        TradeSubscription m_ts = null;
        private Boolean useTs = true;
        private Boolean useIts = false;
        private int numOrdersToSendWithIts = 5;
        private OrderType placeOrderType = OrderType.Limit;
            //OrderType.Limit;
        private MarketKey marketKey = MarketKey.Sfe;
            //MarketKey.Cme;
            //MarketKey.Sfe;
        private string contract = "SFE GB";
            //"ES";
            //"SFE GB";
        private string contractDate = "Mar16";
        private string acctName = "SFE";
            //"<DEFAULT>";
            //"SFE";
        private double buyPrice = 96.960;
            //202775;
            //96.960;



        /// <summary>
        /// Private default constructor
        /// </summary>
        private TTAPIFunctions()
        {
        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        public TTAPIFunctions(string u, string p)
        {
            m_username = u;
            m_password = p;
        }

        /// <summary>
        /// Create and start the Dispatcher
        /// </summary>
        public void Start()
        {
            // Attach a WorkerDispatcher to the current thread
            m_disp = Dispatcher.AttachWorkerDispatcher();
            m_disp.BeginInvoke(new Action(Init));
            m_disp.Run();
        }

        /// <summary>
        /// Initialize TT API
        /// </summary>
        public void Init()
        {
            // Use "Universal Login" Login Mode
            ApiInitializeHandler h = new ApiInitializeHandler(ttApiInitComplete);
            TTAPI.CreateUniversalLoginTTAPI(Dispatcher.Current, m_username, m_password, h);
        }

        /// <summary>
        /// Event notification for status of TT API initialization
        /// </summary>
        public void ttApiInitComplete(TTAPI api, ApiCreationException ex)
        {
            if (ex == null)
            {
                // Authenticate your credentials
                m_apiInstance = (UniversalLoginTTAPI)api;
                m_apiInstance.AuthenticationStatusUpdate += new EventHandler<AuthenticationStatusUpdateEventArgs>(apiInstance_AuthenticationStatusUpdate);
                m_apiInstance.Start();
            }
            else
            {
                Console.WriteLine("TT API Initialization Failed: {0}", ex.Message);
                Dispose();
            }
        }

        /// <summary>
        /// Event notification for status of authentication
        /// </summary>
        public void apiInstance_AuthenticationStatusUpdate(object sender, AuthenticationStatusUpdateEventArgs e)
        {
            if (e.Status.IsSuccess)
            {
                Console.WriteLine("Login Authenticated");
                // lookup an instrument
                m_req = new InstrumentLookupSubscription(m_apiInstance.Session, Dispatcher.Current,
                    new ProductKey(this.marketKey, ProductType.Future, this.contract),
                    this.contractDate);
                m_req.Update += new EventHandler<InstrumentLookupSubscriptionEventArgs>(m_req_Update);
                m_req.Start();

                if (useTs)
                {
                    m_ts = new TradeSubscription(m_apiInstance.Session, Dispatcher.Current, true, false, true, true);
                    m_ts.OrderBookDownload += new EventHandler<OrderBookDownloadEventArgs>(m_ts_OrderBookDownloaded);
                    m_ts.OrderAdded += new EventHandler<OrderAddedEventArgs>(m_ts_OrderAdded);
                    m_ts.FillListStart += new EventHandler<FillListEventArgs>(m_ts_FillListStart);
                    m_ts.FillListEnd += new EventHandler<FillListEventArgs>(m_ts_FillListEnd);
                    m_ts.Start();
                }
            }
            else
            {
                Console.WriteLine("TT Login failed: {0}", e.Status.StatusMessage);
                Dispose();
            }
        }

        /// <summary>
        /// Event notification for instrument lookup
        /// </summary>
        void m_req_Update(object sender, InstrumentLookupSubscriptionEventArgs e)
        {
            if (e.Instrument != null && e.Error == null)
            {
                // Instrument was found
                Console.WriteLine("Found: {0}", e.Instrument.Name);

                // Subscribe for Inside Market Data
                m_ps = new PriceSubscription(e.Instrument, Dispatcher.Current);
                m_ps.Settings = new PriceSubscriptionSettings(PriceSubscriptionType.InsideMarket);
                m_ps.FieldsUpdated += new FieldsUpdatedEventHandler(m_ps_FieldsUpdated);
                m_ps.Start();

                // Create a TradeSubscription to listen for order / fill events only for orders submitted through it
                if (useIts)
                {
                    m_its = new InstrumentTradeSubscription(m_apiInstance.Session, Dispatcher.Current, e.Instrument, true, false, false, false);
                    m_its.OrderBookDownload += new EventHandler<OrderBookDownloadEventArgs>(m_ts_OrderBookDownloaded);
                    m_its.OrderUpdated += new EventHandler<OrderUpdatedEventArgs>(m_ts_OrderUpdated);
                    m_its.OrderAdded += new EventHandler<OrderAddedEventArgs>(m_ts_OrderAdded);
                    m_its.OrderDeleted += new EventHandler<OrderDeletedEventArgs>(m_ts_OrderDeleted);
                    m_its.OrderFilled += new EventHandler<OrderFilledEventArgs>(m_ts_OrderFilled);
                    m_its.OrderRejected += new EventHandler<OrderRejectedEventArgs>(m_ts_OrderRejected);
                    m_its.FillListStart += new EventHandler<FillListEventArgs>(m_ts_FillListStart);
                    m_its.FillListEnd += new EventHandler<FillListEventArgs>(m_ts_FillListEnd);
                    //m_its.FillRecordAdded
                    m_its.Start();

                    for (int i = 0; i < this.numOrdersToSendWithIts; i++)
                    {
                        OrderProfile op = new OrderProfile(e.Instrument.GetValidOrderFeeds()[0], e.Instrument, acctName);
                        op.BuySell = BuySell.Buy;
                        op.AccountType = AccountType.A1;
                        op.OrderQuantity = Quantity.FromInt(e.Instrument, 1);
                        op.OrderType = this.placeOrderType;
                        if(this.placeOrderType == OrderType.Limit)
                        {
                            op.LimitPrice = Price.FromDouble(e.Instrument, this.buyPrice);
                        }
                        op.TimeInForce = new TimeInForce(TimeInForceCode.GoodTillCancel);
                        m_its.SendOrder(op);
                    }
                }
              
            }
            else if (e.IsFinal)
            {
                // Instrument was not found and TT API has given up looking for it
                Console.WriteLine("Cannot find instrument: {0}", e.Error.Message);
                Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void m_ts_FillListEnd(object sender, FillListEventArgs e)
        {
     //       if (e.FeedConnectionKey.GatewayKey.Name.ToString() == "SFE")
     //       {
                //   InstrumentTradeSubscription ts = (InstrumentTradeSubscription)sender;
                TradeSubscription ts = (TradeSubscription)sender;
                IDictionary<string, Fill> fills = ts.Fills;
                if (fills.Count != 0)
                {
                    Console.WriteLine("\r\nohhh yeah " + fills.Count + "\r\n");
                }
     //       }
            Console.WriteLine("FillListEnd hit " + e.FeedConnectionKey.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        private void m_ts_FillListStart(object sender, FillListEventArgs e)
        {
            if (e.FeedConnectionKey.GatewayKey.Name.ToString() == "SFE")
            {
                 //   InstrumentTradeSubscription ts = (InstrumentTradeSubscription)sender;
                TradeSubscription ts = (TradeSubscription)sender;
                IDictionary<string, Fill> fills = ts.Fills;
                if (fills.Count != 0)
                {
                    Console.WriteLine("\r\nohhh yeah " + fills.Count + "\r\n");
                }
            }
            Console.WriteLine("FillListStart hit " + e.FeedConnectionKey.ToString());
        }

        /// <summary>
        /// Event notification for price update
        /// </summary>
        void m_ps_FieldsUpdated(object sender, FieldsUpdatedEventArgs e)
        {
            if (e.Error == null)
            {
                /* Make sure that there is a valid bid
                if (e.Fields.GetBestBidPriceField().HasValidValue)
                {
                    if (m_orderKey == "")
                    {
                        System.Collections.ObjectModel.ReadOnlyCollection<OrderFeed> blah =
                            e.Fields.Instrument.GetValidOrderFeeds();
                        Dictionary<string,GatewayKey> blah2 = e.Fields.Instrument.GetValidGateways();
                        IDictionary<FeedConnectionKey,OrderFeed> blah3 = e.Fields.Instrument.Product.Market.OrderFeeds;
                        if(blah == null || blah2 == null || blah3 == null)
                        {
                            int mark = 9;
                        }
                        
                        // If there is no order working, submit one through the first valid order feed.
                        // You should use the order feed that is valid for your purposes.
                        OrderProfile op = new OrderProfile(e.Fields.Instrument.GetValidOrderFeeds()[0], e.Fields.Instrument, "SFE");
                        op.BuySell = BuySell.Buy;
                      //  op.AccountName = "SFE";
                        op.AccountType = AccountType.A1;
                        op.OrderQuantity = Quantity.FromInt(e.Fields.Instrument, 10);
                        op.OrderType = OrderType.Limit;
                        op.LimitPrice = e.Fields.GetBestBidPriceField().Value;

                        if (!m_its.SendOrder(op))
                        {
                            Console.WriteLine("Send new order failed.  {0}", op.RoutingStatus.Message);
                            Dispose();
                        }
                        else
                        {
                            m_orderKey = op.SiteOrderKey;
                            Console.WriteLine("Send new order succeeded.");
                        }
                    }
                    else if(m_its.Orders.ContainsKey(m_orderKey) && 
                        m_its.Orders[m_orderKey].LimitPrice != e.Fields.GetBestBidPriceField().Value)
                    {
                        // If there is a working order, reprice it if its price is not the same as the bid
                        OrderProfileBase op = m_its.Orders[m_orderKey].GetOrderProfile();
                        op.LimitPrice = e.Fields.GetBestBidPriceField().Value;
                        op.Action = OrderAction.Change;

                        if (!m_its.SendOrder(op))
                        {
                            Console.WriteLine("Send change order failed.  {0}", op.RoutingStatus.Message);
                        }
                        else
                        {
                            Console.WriteLine("Send change order succeeded.");
                        }
                    }
                } */
            }
            else
            {
                if (e.Error.IsRecoverableError == false)
                {
                    Console.WriteLine("Unrecoverable price subscription error: {0}", e.Error.Message);
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Called when TradeSubscription starts
        /// </summary>
        void m_ts_OrderBookDownloaded(object sender, OrderBookDownloadEventArgs e)
        {
            Console.WriteLine("Hit OrderBookDownloaded \r\n" );
        }

        /// <summary>
        /// Event notification for order rejected
        /// </summary>
        void m_ts_OrderRejected(object sender, OrderRejectedEventArgs e)
        {
            Console.WriteLine("Order was rejected.");
        }

        /// <summary>
        /// Event notification for order filled
        /// </summary>
        void m_ts_OrderFilled(object sender, OrderFilledEventArgs e)
        {
            if (e.FillType == FillType.Full)
            {
                Console.WriteLine("Order was fully filled for {0} at {1}.", e.Fill.Quantity, e.Fill.MatchPrice);
            }
            else
            {
                Console.WriteLine("Order was partially filled for {0} at {1}.", e.Fill.Quantity, e.Fill.MatchPrice);
            }

            Console.WriteLine("Average Buy Price = {0} : Net Position = {1} : P&L = {2}", m_its.ProfitLossStatistics.BuyAveragePrice,
                m_its.ProfitLossStatistics.NetPosition, m_its.ProfitLoss.AsPrimaryCurrency);

            
        }

        /// <summary>
        /// Event notification for order deleted
        /// </summary>
        void m_ts_OrderDeleted(object sender, OrderDeletedEventArgs e)
        {
            Console.WriteLine("Order was deleted.");
        }

        /// <summary>
        /// Event notification for order added
        /// </summary>
        void m_ts_OrderAdded(object sender, OrderAddedEventArgs e)
        {
            bool isDownloaded = e.Order.IsDownloaded;
            if (isDownloaded)
            {
                Console.Write("*");
            } 
            else
            {
                Console.WriteLine("\r\n*** Number of downloaded orders " + m_its.Orders.Count() + "\r\n");
            }
      //      Console.WriteLine("Order was added with price of {0}.", e.Order.LimitPrice);
        }

        /// <summary>
        /// Event notification for order update
        /// </summary>
        void m_ts_OrderUpdated(object sender, OrderUpdatedEventArgs e)
        {
            Console.WriteLine("Order was updated with price of {0}.", e.NewOrder.LimitPrice);
        }

        /// <summary>
        /// Shuts down the TT API
        /// </summary>
        public void Dispose()
        {
            lock(m_lock)
            {
                if (!m_disposed)
                {
                    // Unattached callbacks and dispose of all subscriptions
                    if (m_req != null)
                    {
                        m_req.Update -= m_req_Update;
                        m_req.Dispose();
                        m_req = null;
                    }
                    if (m_ps != null)
                    {
                        m_ps.FieldsUpdated -= m_ps_FieldsUpdated;
                        m_ps.Dispose();
                        m_ps = null;
                    }
                    if (m_its != null)
                    {
                        m_its.OrderUpdated -= m_ts_OrderUpdated;
                        m_its.OrderAdded -= m_ts_OrderAdded;
                        m_its.OrderDeleted -= m_ts_OrderDeleted;
                        m_its.OrderFilled -= m_ts_OrderFilled;
                        m_its.OrderRejected -= m_ts_OrderRejected;
                        m_its.Dispose();
                        m_its = null;
                    }

                    // Begin shutdown the TT API
                    TTAPI.ShutdownCompleted += new EventHandler(TTAPI_ShutdownCompleted);
                    TTAPI.Shutdown();

                    m_disposed = true;
                }
            }
        }

        /// <summary>
        /// Event notification for completion of TT API shutdown
        /// </summary>
        public void TTAPI_ShutdownCompleted(object sender, EventArgs e)
        {
            // Shutdown the Dispatcher
            if (m_disp != null)
            {
                m_disp.BeginInvokeShutdown();
                m_disp = null;
            }

            // Dispose of any other objects / resources
        }

    }
}
