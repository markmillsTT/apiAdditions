﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTAPI_Sample_Console_ASEOrderRouting
{
    using TradingTechnologies.TTAPI;
    using TradingTechnologies.TTAPI.Tradebook;
    using TradingTechnologies.TTAPI.Autospreader;

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
        private InstrumentLookupSubscription m_req1 = null;
        private InstrumentLookupSubscription m_req2 = null;
        private CreateAutospreaderInstrumentRequest m_casReq = null;
        private PriceSubscription m_ps = null;
        private ASInstrumentTradeSubscription m_ts = null;
        private string m_orderKey = "";
        private Dictionary<int, Instrument> m_spreadLegKeys = new Dictionary<int, Instrument>();
        private string m_ASEGateway = "ASE-A";
        private string m_username = "MARK";
        private string m_password = "12345678";

        List<MarksSpreadObject> allSpreads = new List<MarksSpreadObject>();
        TradeSubscription some_TradeSubscription = null;
        SpreadDetailSubscription sd_subscrtiption = null;
        IDictionary<InstrumentKey, List<SpreadLegDetails>> parentInstr_Legs_Table = new SortedDictionary<InstrumentKey, List<SpreadLegDetails>>();
        string changingName = "From ASE Console App - Mark Mills addition 2";


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
                // lookup the leg instruments

                ProductKey prodKeyLeg = new ProductKey(MarketKey.Cme, ProductType.Future, "ES");

                // We will use a dictionary to hold all instrument requests and update it when each instrument is found.
                // Once all lookup requests for the legs are complete, we can continue with the creation of the spread.
                // tagValue will be used in the dictionary to identify each lookup request.

                int tagValue = 1000;

                m_req1 = new InstrumentLookupSubscription(m_apiInstance.Session, Dispatcher.Current, prodKeyLeg, "Jun16");
                m_req1.Tag = tagValue;
                m_spreadLegKeys.Add(tagValue, null);
                m_req1.Update += new EventHandler<InstrumentLookupSubscriptionEventArgs>(m_req_Update);
                m_req1.Start();

                tagValue++;

                m_req2 = new InstrumentLookupSubscription(m_apiInstance.Session, Dispatcher.Current, prodKeyLeg, "Sep16");
                m_req2.Tag = tagValue;
                m_spreadLegKeys.Add(tagValue, null);
                m_req2.Update += new EventHandler<InstrumentLookupSubscriptionEventArgs>(m_req_Update);
                m_req2.Start();
            }
            else
            {
                Console.WriteLine("TT Login failed: {0}", e.Status.StatusMessage);
                Dispose();
            }
        }

        /// <summary>
        /// Helper function that determines if all leg instruments have been found
        /// </summary>
        private bool HaveWeFoundAllLegs()
        {
            if (m_spreadLegKeys.Count == 0)
            {
                return false;
            }

            foreach (Instrument instrument in m_spreadLegKeys.Values)
            {
                if (instrument == null)
                {
                    return false;
                }
            }

            return true;
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

                // Update the dictionary to indicate that the instrument was found.
                InstrumentLookupSubscription instrLookupSub = sender as InstrumentLookupSubscription;

                if (m_spreadLegKeys.ContainsKey((int)instrLookupSub.Tag))
                {
                    m_spreadLegKeys[(int)instrLookupSub.Tag] = e.Instrument;
                }
            }
            else if (e.IsFinal)
            {
                // Instrument was not found and TT API has given up looking for it
                Console.WriteLine("Cannot find instrument: {0}", e.Error.Message);
                Dispose();
            }

            // If we have found all of the leg instruments, proceed with the creation of the spread.
            if (HaveWeFoundAllLegs())
            {
                Console.WriteLine("All leg instruments have been found.  Creating the spread...");

                // SpreadDetails related properties
                SpreadDetails spreadDetails = new SpreadDetails();
                spreadDetails.Name = "My Spread";

                int i = 0;
                List<SpreadLegDetails> sldList = new List<SpreadLegDetails>();
                List<Instrument> instList = new List<Instrument>();
                // Add the legs to the SpreadDetails
                foreach (Instrument instrument in m_spreadLegKeys.Values)
                {
                    // In this example, the order is routed to the first order feed in the list of valid order feeds.
                    // You should use the order feed that is appropriate for your purposes.
                    SpreadLegDetails spreadlegDetails = new SpreadLegDetails(instrument, instrument.GetValidOrderFeeds()[0].ConnectionKey);
                    spreadlegDetails.SpreadRatio = (i % 2 == 0) ? 1 : -1;
                    spreadlegDetails.PriceMultiplier = (i % 2 == 0) ? 1 : -1;
                    spreadlegDetails.CustomerName = "<DEFAULT>";

                    spreadDetails.Legs.Append(spreadlegDetails);
                    i++;

                    instList.Add(instrument);
                    sldList.Add(spreadlegDetails);
                }

                // Mark Mills Addition - Activate after testing - also remove createSpread(..) and other code from m_ts_OrderAdded(..)
                // Make loop to create spreads for even number of instruments and even number of spread details
                if ( instList.Count() % 2 == 0 && sldList.Count() % 2 == 0 )
                {
                    for (int k = 0; k < instList.Count() - 1; k++)
                    {
                        Instrument[] instrumentArray = instList.ToArray();
                        SpreadLegDetails[] sldArray = sldList.ToArray();
                        MarksSpreadObject mySpreadObject = new MarksSpreadObject(spreadDetails, sldArray[k], sldArray[k+1],
                            instrumentArray[k], instrumentArray[k+1]);
                        allSpreads.Add(mySpreadObject);
                        createAndSendSpread(mySpreadObject.inst1, mySpreadObject.inst2);
                    }

                    // Create an AutospreaderInstrument corresponding to the synthetic spread
                    m_casReq = new CreateAutospreaderInstrumentRequest(m_apiInstance.Session, Dispatcher.Current, spreadDetails);
                    m_casReq.Completed += new EventHandler<CreateAutospreaderInstrumentRequestEventArgs>(m_casReq_Completed);
                    m_casReq.Submit();
                }
            }
        }

        /// <summary>
        /// Event notification for AutospreaderInstrument creation
        /// </summary>
        public void m_casReq_Completed(object sender, CreateAutospreaderInstrumentRequestEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Instrument != null)
                {
                    // In this example, the AutospreaderInstrument is launched to ASE-A.
                    // You should use the order feed that is appropriate for your purposes.
                    OrderFeed oFeed = e.Instrument.GetValidOrderFeeds()[0];
                    if (oFeed.IsTradingEnabled)
                    {
                        // deploy the Autospreader Instrument to the specified ASE
                        e.Instrument.TradableStatusChanged += new EventHandler<TradableStatusChangedEventArgs>(Instrument_TradableStatusChanged);
                        LaunchReturnCode lrc = e.Instrument.LaunchToOrderFeed(oFeed);
                        if (lrc != LaunchReturnCode.Success)
                        {
                            Console.WriteLine("Launch request was unsuccessful");
                        }
                    }
                }
            }
            else
            {
                // AutospreaderInstrument creation failed
                Console.WriteLine("AutospreaderInstrument creation failed: " + e.Error.Message);
            }
        }

        /// <summary>
        /// Helper function for finding an OrderFeed given a gateway name
        /// </summary>
        OrderFeed GetOrderFeedByName(Instrument instr, string gateway)
        {
            foreach (OrderFeed oFeed in instr.GetValidOrderFeeds())
            {
                // Mark Mills Addition
                //if (oFeed.Name.Equals(gateway))
                //{
                    return oFeed;
                //}
            }

            return (OrderFeed)null;
        }

        /// <summary>
        /// Event notification for AutospreaderInstrument launch
        /// </summary>
        void Instrument_TradableStatusChanged(object sender, TradableStatusChangedEventArgs e)
        {
            if (e.Value)
            {
                // launch of AutospreaderInstrument was successful
                AutospreaderInstrument instr = sender as AutospreaderInstrument;

                // Subscribe for Inside Market Data
                m_ps = new PriceSubscription(instr, Dispatcher.Current);
                m_ps.Settings = new PriceSubscriptionSettings(PriceSubscriptionType.InsideMarket);
                m_ps.FieldsUpdated += new FieldsUpdatedEventHandler(m_ps_FieldsUpdated);
                m_ps.Start();

                // Create an ASTradeSubscription to listen for order / fill events only for orders submitted through it
                m_ts = new ASInstrumentTradeSubscription(m_apiInstance.Session, Dispatcher.Current, instr, true, true, false, false);
                m_ts.OrderUpdated += new EventHandler<OrderUpdatedEventArgs>(m_ts_OrderUpdated);
                m_ts.OrderAdded += new EventHandler<OrderAddedEventArgs>(m_ts_OrderAdded);
                m_ts.OrderDeleted += new EventHandler<OrderDeletedEventArgs>(m_ts_OrderDeleted);
                m_ts.OrderFilled += new EventHandler<OrderFilledEventArgs>(m_ts_OrderFilled);
                m_ts.OrderRejected += new EventHandler<OrderRejectedEventArgs>(m_ts_OrderRejected);
                m_ts.Start();
            }
            else
            {
                Console.WriteLine("Launch of AutospreaderInstrument to {0} was unsuccessful.", e.OrderFeed.Name);
            }
        }

        /// <summary>
        /// Event notification for price update
        /// </summary>
        void m_ps_FieldsUpdated(object sender, FieldsUpdatedEventArgs e)
        {
            /* Mark Mills Addition - only submit my Autospreader Order
            if (e.Error == null)
            {
                // Make sure that there is a valid bid
                if (e.Fields.GetBestBidPriceField().HasValidValue)
                {
                    if (m_orderKey == "")
                    {
                        // In this example, the order is submitted to ASE-A.
                        // You should use the order feed that is appropriate for your purposes.

                        AutospreaderSyntheticOrderProfile op = new AutospreaderSyntheticOrderProfile(((AutospreaderInstrument)e.Fields.Instrument).GetValidGateways()[m_ASEGateway], 
                            (AutospreaderInstrument)e.Fields.Instrument);
                        op.BuySell = BuySell.Buy;
                        op.OrderQuantity = Quantity.FromInt(e.Fields.Instrument, 10);
                        op.OrderType = OrderType.Limit;
                        op.LimitPrice = e.Fields.GetBestBidPriceField().Value;

                        if (!m_ts.SendOrder(op))
                        {
                            Console.WriteLine("Send new order failed.  {0}", op.RoutingStatus.Message);
                        }
                        else
                        {
                            m_orderKey = op.SiteOrderKey;
                            Console.WriteLine("Send new order succeeded.");
                        }
                    }
                    else if (m_ts.Orders.ContainsKey(m_orderKey) &&
                        m_ts.Orders[m_orderKey].LimitPrice != e.Fields.GetBestBidPriceField().Value)
                    {
                        // If there is a working order, reprice it if its price is not the same as the bid
                        AutospreaderSyntheticOrderProfile op = m_ts.Orders[m_orderKey].GetOrderProfile() as AutospreaderSyntheticOrderProfile;
                        op.LimitPrice = e.Fields.GetBestBidPriceField().Value;
                        op.Action = OrderAction.Change;

                        if (!m_ts.SendOrder(op))
                        {
                            Console.WriteLine("Send change order failed.  {0}", op.RoutingStatus.Message);
                        }
                        else
                        {
                            Console.WriteLine("Send change order succeeded.");
                        }
                    }
                }
            }
            else
            {
                if (e.Error.IsRecoverableError == false)
                {
                    Console.WriteLine("Unrecoverable price subscription error: {0}", e.Error.Message);
                    Dispose();
                }
            } */
        }

        /// <summary>
        /// Event notification for order rejected
        /// </summary>
        void m_ts_OrderRejected(object sender, OrderRejectedEventArgs e)
        {
            if (e.Order.SiteOrderKey == m_orderKey)
            {
                // Our parent order has been rejected
                Console.WriteLine("Our parent order has been rejected: {0}", e.Message);
            }
            else if (e.Order.SyntheticOrderKey == m_orderKey)
            {
                // A child order of our parent order has been rejected
                Console.WriteLine("A child order of our parent order has been rejected: {0}", e.Message);
            }
        }

        /// <summary>
        /// Event notification for order filled
        /// </summary>
        void m_ts_OrderFilled(object sender, OrderFilledEventArgs e)
        {
            if (e.Fill.SiteOrderKey == m_orderKey)
            {
                // Our parent order has been filled
                Console.WriteLine("Our parent order has been " + (e.Fill.FillType == FillType.Full ? "fully" : "partially") + " filled");
            }
            else if (e.Fill.ParentKey == m_orderKey)
            {
                // A child order of our parent order has been filled
                Console.WriteLine("A child order of our parent order has been " + (e.Fill.FillType == FillType.Full ? "fully" : "partially") + " filled");
            }

            Console.WriteLine("Average Buy Price = {0} : Net Position = {1} : P&L = {2}", m_ts.ProfitLossStatistics.BuyAveragePrice,
                m_ts.ProfitLossStatistics.NetPosition, m_ts.ProfitLoss.AsPrimaryCurrency);
        }

        /// <summary>
        /// Event notification for order deleted
        /// </summary>
        void m_ts_OrderDeleted(object sender, OrderDeletedEventArgs e)
        {
            if (e.DeletedUpdate.SiteOrderKey == m_orderKey)
            {
                // Our parent order has been deleted
                Console.WriteLine("Our parent order has been deleted: {0}", e.Message);
            }
            else if (e.DeletedUpdate.SyntheticOrderKey == m_orderKey)
            {
                // A child order of our parent order has been deleted
                Console.WriteLine("A child order of our parent order has been deleted: {0}", e.Message);
            }
        }

        /// <summary>
        /// Event notification for order added
        /// </summary>
        void m_ts_OrderAdded(object sender, OrderAddedEventArgs e)
        {
            // A Mark Mills Production
            MarksSpreadObject[] marksSpreadObjects = allSpreads.ToArray();
            for(int i = 0; i < marksSpreadObjects.Length; i++){
                //marksSpreadObjects[i].addOrderProfile(e.Order.GetOrderProfile);
                createAndSendSpread(marksSpreadObjects[i].inst1, marksSpreadObjects[i].inst2);
            }
           
            if (e.Order.SiteOrderKey == m_orderKey)
            {
                // Our parent order has been added
                Console.WriteLine("Our parent order has been added: {0}", e.Message);
            }
            else if (e.Order.SyntheticOrderKey == m_orderKey)
            {
                // A child order of our parent order has been added
                Console.WriteLine("A child order of our parent order has been added: {0}", e.Message);
            }
        }

        #region Autospreader Order

        private void createAndSendSpread(Instrument instr1, Instrument instr2)
        {

            sd_subscrtiption = new SpreadDetailSubscription(m_apiInstance.Session, Dispatcher.Current);
            sd_subscrtiption.SpreadDetailsAdded += new EventHandler<SpreadDetailsEventArgs>(sds_SpreadDetailsAdded);
            sd_subscrtiption.SpreadDetailsDeleted += new EventHandler<SpreadDetailsEventArgs>(sds_SpreadDetailsDeleted);
            sd_subscrtiption.SpreadDetailsUpdated += new EventHandler<SpreadDetailsEventArgs>(sds_SpreadDetailsUpdated);
            sd_subscrtiption.Start();

            // Create the SpreadDetails object
            SpreadDetails sd = new SpreadDetails();

            // Define the spread properties
            sd.Name = this.changingName;

            // Create the spread legs
            SpreadLegDetails sld1 = new SpreadLegDetails(instr1, instr1.GetValidOrderFeeds()[0].ConnectionKey);
            SpreadLegDetails sld2 = new SpreadLegDetails(instr2, instr2.GetValidOrderFeeds()[0].ConnectionKey);

            // Define the spread leg properties
            sld1.SpreadRatio = 1;
            sld2.SpreadRatio = -1;

            // Add the spread legs to the spread
            sd.Legs.Append(sld1);
            sd.Legs.Append(sld2);
            sld1.PriceMultiplier = 1;
            sld2.PriceMultiplier = -1;
            sld1.CustomerName = "";
            sld2.CustomerName = "";

            MarksSpreadObject spreadObject = new MarksSpreadObject(sd, sld1, sld2, instr1, instr2);
            allSpreads.Add(spreadObject);

            // Create an Instrument corresponding to the synthetic spread
            CreateAutospreaderInstrumentRequest casReq = new CreateAutospreaderInstrumentRequest(m_apiInstance.Session, Dispatcher.Current, sd);
            casReq.Completed += new EventHandler<CreateAutospreaderInstrumentRequestEventArgs>(casReq_Completed);
            casReq.Submit();
        }

        void sds_SpreadDetailsAdded(object sender, SpreadDetailsEventArgs e)
        {
            Console.WriteLine("sds_SpreadDetailsAdded() hit");
            //TEST - see if update works after firing
            if (allSpreads.Count != 0)
            {
                MarksSpreadObject spreadObj = allSpreads.ToArray()[0];

                if (spreadObj.orderProfile != null)
                {

                    SpreadDetails sd = spreadObj.sd;
                    SpreadLegDetails sld1 = spreadObj.sld1;
                    SpreadLegDetails sld2 = spreadObj.sld2;
                    OrderProfile orderProfile = spreadObj.orderProfile;

                    //orderProfile.QuantityToWork = Quantity.FromInt(orderProfile.;
                    sd.Legs.Remove(1);
                    sld2.PriceMultiplier = 1.1;
                    sld2.ActiveQuoting = false;
                    sd.Legs.Append(sld2);

                    AutospreaderManager manager = m_apiInstance.AutospreaderManager;
                    manager.UpdateSpreadDetails(sd);
                    //update spread
                    //CreateAutospreaderInstrumentRequest casReq = new CreateAutospreaderInstrumentRequest(m_TTAPI.Session, Dispatcher.Current, sd);
                    //casReq.Submit();
                }

            }
        }

        void sds_SpreadDetailsDeleted(object sender, SpreadDetailsEventArgs e)
        {
            Console.WriteLine("sds_SpreadDetailsDeleted() hit");
        }

        void sds_SpreadDetailsUpdated(object sender, SpreadDetailsEventArgs e)
        {
            Console.WriteLine("sds_SpreadDetailsUpdated() hit");
        }

        private void casReq_Completed(object sender, CreateAutospreaderInstrumentRequestEventArgs e)
        {
            //e.Instrument;
            if (e.Instrument != null)
            {
                // deploy the Autospreader Instrument to the specified ASE
                e.Instrument.TradableStatusChanged += new EventHandler<TradableStatusChangedEventArgs>(TradableStatusChanged);

                LaunchReturnCode lrc = e.Instrument.LaunchToOrderFeed(e.Instrument.GetValidOrderFeeds()[0]); ;
                if (lrc == LaunchReturnCode.Success)
                {
                    Console.WriteLine("Autospreader launch was a success according to casReq_Completed");
                }
            }
        }

        public class MarksSpreadObject
        {
            public SpreadDetails sd = null;
            public SpreadLegDetails sld1 = null;
            public SpreadLegDetails sld2 = null;
            public Instrument inst1 = null;
            public Instrument inst2 = null;
            public OrderProfile orderProfile = null;

            public MarksSpreadObject(SpreadDetails sd, SpreadLegDetails sl1d, SpreadLegDetails sl2d, Instrument inst1, Instrument inst2)
            {
                this.sd = sd;
                this.sld1 = sl1d;
                this.sld2 = sl2d;
                this.inst1 = inst1;
                this.inst2 = inst2;
            }

            public void addOrderProfile(OrderProfile orderProfile)
            {
                this.orderProfile = orderProfile;
            }
        }

        private void TradableStatusChanged(object sender, TradableStatusChangedEventArgs e)
        {
            Console.WriteLine("Autospreader launch was a success? " + e.Value);
        }

        #endregion

        /// <summary>
        /// Event notification for order update
        /// </summary>
        void m_ts_OrderUpdated(object sender, OrderUpdatedEventArgs e)
        {
            if (e.OldOrder.SiteOrderKey == m_orderKey)
            {
                // Our parent order has been updated
                Console.WriteLine("Our parent order has been updated: {0}", e.Message);
            }
            else if (e.OldOrder.SyntheticOrderKey == m_orderKey)
            {
                // A child order of our parent order has been updated
                Console.WriteLine("A child order of our parent order has been updated: {0}", e.Message);
            }
        }

        /// <summary>
        /// Shuts down the TT API
        /// </summary>
        public void Dispose()
        {
            lock (m_lock)
            {
                if (!m_disposed)
                {
                    // Unattached callbacks and dispose of all subscriptions
                    if (m_req1 != null)
                    {
                        m_req1.Update -= m_req_Update;
                        m_req1.Dispose();
                        m_req1 = null;
                    }
                    if (m_req2 != null)
                    {
                        m_req2.Update -= m_req_Update;
                        m_req2.Dispose();
                        m_req2 = null;
                    }
                    if (m_ps != null)
                    {
                        m_ps.FieldsUpdated -= m_ps_FieldsUpdated;
                        m_ps.Dispose();
                        m_ps = null;
                    }
                    if (m_ts != null)
                    {
                        m_ts.OrderAdded -= m_ts_OrderAdded;
                        m_ts.OrderDeleted -= m_ts_OrderDeleted;
                        m_ts.OrderFilled -= m_ts_OrderFilled;
                        m_ts.OrderRejected -= m_ts_OrderRejected;
                        m_ts.OrderUpdated -= m_ts_OrderUpdated;
                        m_ts.Dispose();
                        m_ts = null;
                    }
                    if (m_casReq != null)
                    {
                        m_casReq.Completed -= m_casReq_Completed;
                        m_casReq.Dispose();
                        m_casReq = null;
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
