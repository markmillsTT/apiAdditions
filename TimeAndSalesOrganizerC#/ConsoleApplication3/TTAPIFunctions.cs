using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeAndSalesOrganizer;

namespace TTAPI_Init_Worker_UL
{
    using TradingTechnologies.TTAPI;
    // Work notes
    // Working currently on making seperate "Record timings"
    // 1. Somehow set standard format for time record
    // 2. Create parser
    // 3. Create tokenizer for the sample application input
    // 4. Create application

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
        private System.IO.StreamWriter file;
        private string startTime;
        private Boolean firstTime = true;


        /// <summary>
        /// Private default constructor
        /// </summary>
        public TTAPIFunctions()
        {
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
            TTAPI.CreateUniversalLoginTTAPI(Dispatcher.Current, AppProperties.m_username, AppProperties.m_password, h);
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
                // Add code here to begin working with the TT API
                InstrumentLookupSubscription instSub = new InstrumentLookupSubscription(m_apiInstance.Session, Dispatcher.Current,
                  AppProperties.prod1Key, AppProperties.contract1Key);
                instSub.Update += new EventHandler<InstrumentLookupSubscriptionEventArgs>(instrumentUpdate);
                instSub.Start();
            }
            else
            {
                Console.WriteLine("TT Login failed: {0}", e.Status.StatusMessage);
                Dispose();
            }
        }

        void instrumentUpdate(object sender, InstrumentLookupSubscriptionEventArgs e)
        {
            if (e.Instrument != null && e.Error == null)
            {
                
                PriceSubscription ps = new PriceSubscription(e.Instrument, Dispatcher.Current);
                ps.Settings = new PriceSubscriptionSettings(PriceSubscriptionType.InsideMarket);
                ps.FieldsUpdated += new FieldsUpdatedEventHandler(ps_FieldsUpdated);
                ps.Start();

                TimeAndSalesSubscription tsSub = new TimeAndSalesSubscription(e.Instrument, Dispatcher.Current);
                tsSub.Update += new EventHandler<TimeAndSalesEventArgs>(timeAndSalesUpdate);
                tsSub.Start();
            }
            else if (e.IsFinal)
            {
                // Instrument was not found and TT API has given up looking for it
            }
            else
            {
                // Instrument was not found yet - but still searching
            }
        }

        void timeAndSalesUpdate(object sender, TimeAndSalesEventArgs e)
        {
            string write = "";
            foreach (TimeAndSalesData tsData in e.Data)
            {
                DateTime ltt = tsData.TimeStamp;
                Price ltp = tsData.TradePrice;
                Quantity ltq = tsData.TradeQuantity;

                write += (ltt.ToFileTimeUtc() - this.startTime) + "," + ltp.ToString() + "," + ltq.ToString() + "\n";

                if(firstTime)
                {
                    this.startTime = ltt.ToLongDateString();
                    firstTime = false;
                }
     
            }
            using (file = new System.IO.StreamWriter(AppProperties.fileLoc, true))
            {
                file.Write(write);
                
                file.WriteLine("Init " + System.DateTime.Now);
                file.WriteLine("Init " + this.startTime);
            }
            file.Close();
        }

        void ps_FieldsUpdated(object sender, FieldsUpdatedEventArgs e)
        {
            if (e.Error == null)
            {
                Console.WriteLine("Instrment: {0}", e.Fields.Instrument.Name);
                Price lp = (Price)e.Fields[FieldId.LastTradedPrice].Value;
                Console.WriteLine("Price: {0}\nOffsetPrice: {1}", lp.ToString(), lp.GetTickPrice(2).ToString());
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