﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TTAPI_Console_PriceSubscription_MT
{
    using TradingTechnologies.TTAPI;
    using TradingTechnologies.TTAPI.Tradebook;

    /// <summary>
    /// Class for strategy #2
    /// </summary>
    class Strategy2 : IDisposable
    {
        /// <summary>
        /// Declare the TTAPI objects
        /// </summary>
        private UniversalLoginTTAPI m_apiInstance = null;
        private WorkerDispatcher m_disp = null;
        private object m_lock = new object();
        private bool m_disposed = false;

        private List<InstrumentLookupSubscription> m_lreq = new List<InstrumentLookupSubscription>();
        private List<TimeAndSalesSubscription> m_ltsSub = new List<TimeAndSalesSubscription>();
        private List<ContractDetails> m_lcd = new List<ContractDetails>();
        // [TS_INFO] 
        // Our tradebook info
        private TradeSubscription m_ts;
        // [TS_INFO]

        /// <summary>
        /// Private default constructor
        /// </summary>
        private Strategy2()
        {
        }

        /// <summary>
        /// Primary constructor
        /// </summary>
        public Strategy2(UniversalLoginTTAPI api, List<ContractDetails> lcd)
        {
            m_apiInstance = api;
            m_lcd = lcd;
        }

        /// <summary>
        /// Cleans up TT API objects
        /// </summary>
        public void Dispose()
        {
            lock (m_lock)
            {
                if (!m_disposed)
                {
                    // Unattached callbacks and dispose of all subscriptions
                    foreach (InstrumentLookupSubscription req in m_lreq)
                    {
                        if (req != null)
                        {
                            req.Update -= req_Update;
                            req.Dispose();
                        }
                    }
                    foreach (TimeAndSalesSubscription tsSub in m_ltsSub)
                    {
                        if (tsSub != null)
                        {
                            tsSub.Update -= tsSub_Update;
                            tsSub.Dispose();
                        }
                    }

                    // Shutdown the Dispatcher
                    if (m_disp != null)
                    {
                        m_disp.BeginInvokeShutdown();
                        m_disp = null;
                    }

                    m_disposed = true;
                }
            }
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
        /// Perform the instrument lookups
        /// </summary>
        public void Init()
        {
            // [TS_INFO]
            // Create a Trade Subscription to track orders in an Order Book
            m_ts = new TradeSubscription(m_apiInstance.Session, Dispatcher.Current);
            m_ts.OrderAdded += new EventHandler<OrderAddedEventArgs>(ts_OrderAdded);
            m_ts.OrderFilled += new EventHandler<OrderFilledEventArgs>(ts_OrderFilled);
            // [TS_INFO]

            foreach (ContractDetails cd in m_lcd)
            {
                InstrumentLookupSubscription req = new InstrumentLookupSubscription(
                    m_apiInstance.Session, Dispatcher.Current,
                    new ProductKey(cd.m_marketKey, cd.m_productType, cd.m_product), cd.m_contract);
                req.Update += new EventHandler<InstrumentLookupSubscriptionEventArgs>(req_Update);
                m_lreq.Add(req);
                req.Start();
            }
        }

        /// <summary>
        /// Event notification for instrument lookup
        /// </summary>
        public void req_Update(object sender, InstrumentLookupSubscriptionEventArgs e)
        {
            if (e.Instrument != null && e.Error == null)
            {
                // Start a Time & Sales subscription
                TimeAndSalesSubscription tsSub = new TimeAndSalesSubscription(e.Instrument, Dispatcher.Current);
                tsSub.Update += new EventHandler<TimeAndSalesEventArgs>(tsSub_Update);
                m_ltsSub.Add(tsSub);
                tsSub.Start();

                // [TS_INFO] TODO
                // Send 5 lot orders to each instrument 5 ticks from Market Price
                OrderProfile op = new OrderProfile(e.Instrument.GetValidOrderFeeds()[0], e.Instrument);
                op.OrderType = OrderType.Limit;
                op.TrailingOffset = 5;
          //      op.OrderQuantity = 5;
                m_ts.SendOrder(op); // send order from ts
                // [TS_INFO]

            }
            else if (e.IsFinal)
            {
                // Instrument was not found and TT API has given up looking for it
                Console.WriteLine("Cannot find instrument: " + e.Error.Message);
                Dispose();
            }
        }

        // [TS_INFO]
        public void ts_OrderAdded(object sender, OrderAddedEventArgs e)
        {

        }

        public void ts_OrderFilled(object sender, OrderFilledEventArgs e)
        {

        }
        // [TS_INFO]

        /// <summary>
        /// Event notification for trade updates
        /// </summary>
        public void tsSub_Update(object sender, TimeAndSalesEventArgs e)
        {
            // process the update
            if (e.Error == null)
            {
                foreach (TimeAndSalesData tsData in e.Data)
                {
                    Console.WriteLine("{0} : {1} --> LTP/LTQ : {2}/{3}",
                        Thread.CurrentThread.Name, e.Instrument.Name, tsData.TradePrice, tsData.TradeQuantity);
                }

                // Add strategy logic here
            }
        }
    }
}
