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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Generic.SortedDictionary;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

using TradingTechnologies.TTAPI;
using TradingTechnologies.TTAPI.Autospreader;
using TradingTechnologies.TTAPI.Tradebook;
using TradingTechnologies.TTAPI.WinFormsHelpers;
using TradingTechnologies.TTAPI.CustomerDefaults;

namespace TTAPI_Samples
{
    /// <summary>
    /// SubmitOrder
    /// 
    /// This example demonstrates using the TT API to submit an order.  The order types
    /// available in the application are market, limit, stop market and stop limit.  
    /// </summary>
    public class frmSubmitOrder : Form
    {
        // Declare private TTAPI member variables.
        string ttUserId = "MARK";
        string ttPassword = "12345678";
        private XTraderModeTTAPI m_TTAPI = null;
        private CustomerDefaultsSubscription m_customerDefaultsSubscription = null;
        private InstrumentTradeSubscription m_instrumentTradeSubscription = null;
    //    private TradeSubscription m_instrumentTradeSubscription = null;
        private PriceSubscription m_priceSubscription = null;
        private bool m_isShutdown = false, m_shutdownInProcess = false;
        private TextBox txtPrice2;
        private TextBox txtQuantity2;
        private Label lblQuantity2;
        private Label Price2;
        private SortedDictionary<String, OrderProfile> ocoOrders = new SortedDictionary<string, OrderProfile>();
        private Button button1;
        private int countOrderTag = 0;

      //  private Instrument instrument = null;
        List<Instrument> foundInstruments = new List<Instrument>();
        List<MarksSpreadObject> allSpreads = new List<MarksSpreadObject>();
        TradeSubscription m_TradeSubscription = null;
        SpreadDetailSubscription sd_subscrtiption = null;
        FillsSubscription fillSub = null;
        IDictionary<InstrumentKey, List<SpreadLegDetails>> parentInstr_Legs_Table = new SortedDictionary<InstrumentKey, List<SpreadLegDetails>>();
        string changingName = "Filip Test";
        double inst1Price, inst2Price, spreadPrice = 0;

        public frmSubmitOrder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Init and start TT API.
        /// </summary>
        /// <param name="instance">XTraderModeTTAPI instance</param>
        /// <param name="ex">Any exception generated from the ApiCreationException</param>
        public void ttApiInitHandler(TTAPI api, ApiCreationException ex)
        {
            if (ex == null)
            {
                m_TTAPI = (XTraderModeTTAPI)api;
                m_TTAPI.ConnectionStatusUpdate += new EventHandler<ConnectionStatusUpdateEventArgs>(ttapiInstance_ConnectionStatusUpdate);
                m_TTAPI.Start();

                m_customerDefaultsSubscription = new CustomerDefaultsSubscription(m_TTAPI.Session, Dispatcher.Current);
                m_customerDefaultsSubscription.CustomerDefaultsChanged += new EventHandler(m_customerDefaultsSubscription_CustomerDefaultsChanged);
                m_customerDefaultsSubscription.Start();
            }
            else if (!ex.IsRecoverable)
            {
                MessageBox.Show("API Initialization Failed: " + ex.Message);
            }
        }

        /// <summary>
        /// ConnectionStatusUpdate callback.
        /// Give feedback to the user that there was an issue starting up and connecting to XT.
        /// </summary>
        void ttapiInstance_ConnectionStatusUpdate(object sender, ConnectionStatusUpdateEventArgs e)
        {
            if (e.Status.IsSuccess)
            {
                this.Enabled = true;
            }
            else
            {
                MessageBox.Show(String.Format("ConnectionStatusUpdate: {0}", e.Status.StatusMessage));
            }
        }

        /// <summary>
        /// Dispose of all the TT API objects and shutdown the TT API 
        /// </summary>
        public void shutdownTTAPI()
        {
            if (!m_shutdownInProcess)
            {
                // Dispose of all request objects
                if (m_customerDefaultsSubscription != null)
                {
                    m_customerDefaultsSubscription.CustomerDefaultsChanged -= m_customerDefaultsSubscription_CustomerDefaultsChanged;
                    m_customerDefaultsSubscription.Dispose();
                    m_customerDefaultsSubscription = null;
                }

                if (m_instrumentTradeSubscription != null)
                {
                    m_instrumentTradeSubscription.OrderAdded -= m_instrumentTradeSubscription_OrderAdded;
                    m_instrumentTradeSubscription.OrderRejected -= m_instrumentTradeSubscription_OrderRejected;
                    m_instrumentTradeSubscription.Dispose();
                    m_instrumentTradeSubscription = null;
                }

                if (m_priceSubscription != null)
                {
                    m_priceSubscription.FieldsUpdated -= m_priceSubscription_FieldsUpdated;
                    m_priceSubscription.Dispose();
                    m_priceSubscription = null;
                }

                TTAPI.ShutdownCompleted += new EventHandler(TTAPI_ShutdownCompleted);
                TTAPI.Shutdown();
            }

            // only run shutdown once
            m_shutdownInProcess = true;
        }

        /// <summary>
        /// Event fired when the TT API has been successfully shutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TTAPI_ShutdownCompleted(object sender, EventArgs e)
        {
            m_isShutdown = true;
            Close();
        }

        /// <summary>
        /// Suspends the FormClosing event until the TT API has been shutdown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!m_isShutdown)
            {
                e.Cancel = true;
                shutdownTTAPI();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private IContainer components;
        private Label label1;
        private ComboBox comboBoxOrderFeed;
        private System.Windows.Forms.StatusBar sbaStatus;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.GroupBox gboInstrumentInfo;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblExchange;
        private System.Windows.Forms.TextBox txtContract;
        private System.Windows.Forms.Label lblContract;
        private System.Windows.Forms.TextBox txtExchange;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.GroupBox gboInstrumentMarketData;
        private System.Windows.Forms.Label lblAskPrice;
        private System.Windows.Forms.TextBox txtAskPrice;
        private System.Windows.Forms.TextBox txtBidPrice;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label lblBidPrice;
        private System.Windows.Forms.Label lblLastPrice;
        private System.Windows.Forms.TextBox txtLastPrice;
        private System.Windows.Forms.GroupBox gboOrderEntry;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblStopPrice;
        private System.Windows.Forms.Label lblOrderType;
        private System.Windows.Forms.Label lblNotProduction;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.TextBox txtChange;
        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.ComboBox cboOrderType;
        private System.Windows.Forms.TextBox txtStopPrice;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cboCustomer;
        private System.Windows.Forms.TextBox txtOrderBook;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sbaStatus = new System.Windows.Forms.StatusBar();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.gboInstrumentInfo = new System.Windows.Forms.GroupBox();
            this.lblProductType = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblExchange = new System.Windows.Forms.Label();
            this.txtContract = new System.Windows.Forms.TextBox();
            this.lblContract = new System.Windows.Forms.Label();
            this.txtExchange = new System.Windows.Forms.TextBox();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.gboInstrumentMarketData = new System.Windows.Forms.GroupBox();
            this.lblAskPrice = new System.Windows.Forms.Label();
            this.txtAskPrice = new System.Windows.Forms.TextBox();
            this.txtBidPrice = new System.Windows.Forms.TextBox();
            this.lblChange = new System.Windows.Forms.Label();
            this.txtChange = new System.Windows.Forms.TextBox();
            this.lblBidPrice = new System.Windows.Forms.Label();
            this.lblLastPrice = new System.Windows.Forms.Label();
            this.txtLastPrice = new System.Windows.Forms.TextBox();
            this.gboOrderEntry = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtQuantity2 = new System.Windows.Forms.TextBox();
            this.lblQuantity2 = new System.Windows.Forms.Label();
            this.txtPrice2 = new System.Windows.Forms.TextBox();
            this.Price2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxOrderFeed = new System.Windows.Forms.ComboBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cboCustomer = new System.Windows.Forms.ComboBox();
            this.txtOrderBook = new System.Windows.Forms.TextBox();
            this.lblOrderType = new System.Windows.Forms.Label();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnBuy = new System.Windows.Forms.Button();
            this.lblStopPrice = new System.Windows.Forms.Label();
            this.txtStopPrice = new System.Windows.Forms.TextBox();
            this.cboOrderType = new System.Windows.Forms.ComboBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblNotProduction = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.gboInstrumentInfo.SuspendLayout();
            this.gboInstrumentMarketData.SuspendLayout();
            this.gboOrderEntry.SuspendLayout();
            this.SuspendLayout();
            // 
            // sbaStatus
            // 
            this.sbaStatus.Location = new System.Drawing.Point(0, 1543);
            this.sbaStatus.Name = "sbaStatus";
            this.sbaStatus.Size = new System.Drawing.Size(1085, 40);
            this.sbaStatus.SizingGrip = false;
            this.sbaStatus.TabIndex = 62;
            this.sbaStatus.Text = "X_TRADER must be running to use this application.";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAbout});
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 0;
            this.mnuAbout.Text = "About...";
            this.mnuAbout.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // gboInstrumentInfo
            // 
            this.gboInstrumentInfo.Controls.Add(this.lblProductType);
            this.gboInstrumentInfo.Controls.Add(this.txtProduct);
            this.gboInstrumentInfo.Controls.Add(this.lblProduct);
            this.gboInstrumentInfo.Controls.Add(this.lblExchange);
            this.gboInstrumentInfo.Controls.Add(this.txtContract);
            this.gboInstrumentInfo.Controls.Add(this.lblContract);
            this.gboInstrumentInfo.Controls.Add(this.txtExchange);
            this.gboInstrumentInfo.Controls.Add(this.txtProductType);
            this.gboInstrumentInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gboInstrumentInfo.Location = new System.Drawing.Point(16, 104);
            this.gboInstrumentInfo.Name = "gboInstrumentInfo";
            this.gboInstrumentInfo.Size = new System.Drawing.Size(432, 251);
            this.gboInstrumentInfo.TabIndex = 63;
            this.gboInstrumentInfo.TabStop = false;
            this.gboInstrumentInfo.Text = "Instrument Information";
            // 
            // lblProductType
            // 
            this.lblProductType.Location = new System.Drawing.Point(16, 133);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(160, 30);
            this.lblProductType.TabIndex = 38;
            this.lblProductType.Text = "Product Type:";
            this.lblProductType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(192, 88);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(200, 31);
            this.txtProduct.TabIndex = 35;
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(80, 88);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(96, 30);
            this.lblProduct.TabIndex = 36;
            this.lblProduct.Text = "Product:";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExchange
            // 
            this.lblExchange.Location = new System.Drawing.Point(48, 44);
            this.lblExchange.Name = "lblExchange";
            this.lblExchange.Size = new System.Drawing.Size(128, 30);
            this.lblExchange.TabIndex = 34;
            this.lblExchange.Text = "Market:";
            this.lblExchange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContract
            // 
            this.txtContract.Location = new System.Drawing.Point(192, 177);
            this.txtContract.Name = "txtContract";
            this.txtContract.Size = new System.Drawing.Size(200, 31);
            this.txtContract.TabIndex = 39;
            // 
            // lblContract
            // 
            this.lblContract.Location = new System.Drawing.Point(64, 177);
            this.lblContract.Name = "lblContract";
            this.lblContract.Size = new System.Drawing.Size(112, 30);
            this.lblContract.TabIndex = 40;
            this.lblContract.Text = "Contract:";
            this.lblContract.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExchange
            // 
            this.txtExchange.Location = new System.Drawing.Point(192, 44);
            this.txtExchange.Name = "txtExchange";
            this.txtExchange.Size = new System.Drawing.Size(200, 31);
            this.txtExchange.TabIndex = 33;
            // 
            // txtProductType
            // 
            this.txtProductType.Location = new System.Drawing.Point(192, 133);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(200, 31);
            this.txtProductType.TabIndex = 37;
            // 
            // gboInstrumentMarketData
            // 
            this.gboInstrumentMarketData.Controls.Add(this.lblAskPrice);
            this.gboInstrumentMarketData.Controls.Add(this.txtAskPrice);
            this.gboInstrumentMarketData.Controls.Add(this.txtBidPrice);
            this.gboInstrumentMarketData.Controls.Add(this.lblChange);
            this.gboInstrumentMarketData.Controls.Add(this.txtChange);
            this.gboInstrumentMarketData.Controls.Add(this.lblBidPrice);
            this.gboInstrumentMarketData.Controls.Add(this.lblLastPrice);
            this.gboInstrumentMarketData.Controls.Add(this.txtLastPrice);
            this.gboInstrumentMarketData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gboInstrumentMarketData.Location = new System.Drawing.Point(464, 104);
            this.gboInstrumentMarketData.Name = "gboInstrumentMarketData";
            this.gboInstrumentMarketData.Size = new System.Drawing.Size(336, 251);
            this.gboInstrumentMarketData.TabIndex = 64;
            this.gboInstrumentMarketData.TabStop = false;
            this.gboInstrumentMarketData.Text = "Instrument Market Data";
            // 
            // lblAskPrice
            // 
            this.lblAskPrice.Location = new System.Drawing.Point(16, 88);
            this.lblAskPrice.Name = "lblAskPrice";
            this.lblAskPrice.Size = new System.Drawing.Size(128, 30);
            this.lblAskPrice.TabIndex = 46;
            this.lblAskPrice.Text = "Ask Price:";
            this.lblAskPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAskPrice
            // 
            this.txtAskPrice.Location = new System.Drawing.Point(160, 88);
            this.txtAskPrice.Name = "txtAskPrice";
            this.txtAskPrice.Size = new System.Drawing.Size(144, 31);
            this.txtAskPrice.TabIndex = 45;
            // 
            // txtBidPrice
            // 
            this.txtBidPrice.Location = new System.Drawing.Point(160, 44);
            this.txtBidPrice.Name = "txtBidPrice";
            this.txtBidPrice.Size = new System.Drawing.Size(144, 31);
            this.txtBidPrice.TabIndex = 41;
            // 
            // lblChange
            // 
            this.lblChange.Location = new System.Drawing.Point(16, 177);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(128, 30);
            this.lblChange.TabIndex = 52;
            this.lblChange.Text = "Change:";
            this.lblChange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtChange
            // 
            this.txtChange.Location = new System.Drawing.Point(160, 177);
            this.txtChange.Name = "txtChange";
            this.txtChange.Size = new System.Drawing.Size(144, 31);
            this.txtChange.TabIndex = 51;
            // 
            // lblBidPrice
            // 
            this.lblBidPrice.Location = new System.Drawing.Point(16, 44);
            this.lblBidPrice.Name = "lblBidPrice";
            this.lblBidPrice.Size = new System.Drawing.Size(128, 30);
            this.lblBidPrice.TabIndex = 42;
            this.lblBidPrice.Text = "Bid Price:";
            this.lblBidPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLastPrice
            // 
            this.lblLastPrice.Location = new System.Drawing.Point(16, 133);
            this.lblLastPrice.Name = "lblLastPrice";
            this.lblLastPrice.Size = new System.Drawing.Size(128, 30);
            this.lblLastPrice.TabIndex = 50;
            this.lblLastPrice.Text = "Last Price:";
            this.lblLastPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastPrice
            // 
            this.txtLastPrice.Location = new System.Drawing.Point(160, 133);
            this.txtLastPrice.Name = "txtLastPrice";
            this.txtLastPrice.Size = new System.Drawing.Size(144, 31);
            this.txtLastPrice.TabIndex = 49;
            // 
            // gboOrderEntry
            // 
            this.gboOrderEntry.Controls.Add(this.button1);
            this.gboOrderEntry.Controls.Add(this.txtQuantity2);
            this.gboOrderEntry.Controls.Add(this.lblQuantity2);
            this.gboOrderEntry.Controls.Add(this.txtPrice2);
            this.gboOrderEntry.Controls.Add(this.Price2);
            this.gboOrderEntry.Controls.Add(this.label1);
            this.gboOrderEntry.Controls.Add(this.comboBoxOrderFeed);
            this.gboOrderEntry.Controls.Add(this.lblCustomer);
            this.gboOrderEntry.Controls.Add(this.cboCustomer);
            this.gboOrderEntry.Controls.Add(this.txtOrderBook);
            this.gboOrderEntry.Controls.Add(this.lblOrderType);
            this.gboOrderEntry.Controls.Add(this.btnSell);
            this.gboOrderEntry.Controls.Add(this.btnBuy);
            this.gboOrderEntry.Controls.Add(this.lblStopPrice);
            this.gboOrderEntry.Controls.Add(this.txtStopPrice);
            this.gboOrderEntry.Controls.Add(this.cboOrderType);
            this.gboOrderEntry.Controls.Add(this.lblQuantity);
            this.gboOrderEntry.Controls.Add(this.txtQuantity);
            this.gboOrderEntry.Controls.Add(this.lblPrice);
            this.gboOrderEntry.Controls.Add(this.txtPrice);
            this.gboOrderEntry.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gboOrderEntry.Location = new System.Drawing.Point(16, 369);
            this.gboOrderEntry.Name = "gboOrderEntry";
            this.gboOrderEntry.Size = new System.Drawing.Size(784, 589);
            this.gboOrderEntry.TabIndex = 65;
            this.gboOrderEntry.TabStop = false;
            this.gboOrderEntry.Text = "Order Entry";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(-6, 414);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 43);
            this.button1.TabIndex = 54;
            this.button1.Text = "Test";
            // 
            // txtQuantity2
            // 
            this.txtQuantity2.Enabled = false;
            this.txtQuantity2.Location = new System.Drawing.Point(160, 311);
            this.txtQuantity2.Name = "txtQuantity2";
            this.txtQuantity2.Size = new System.Drawing.Size(176, 31);
            this.txtQuantity2.TabIndex = 53;
            // 
            // lblQuantity2
            // 
            this.lblQuantity2.Location = new System.Drawing.Point(16, 310);
            this.lblQuantity2.Name = "lblQuantity2";
            this.lblQuantity2.Size = new System.Drawing.Size(128, 31);
            this.lblQuantity2.TabIndex = 52;
            this.lblQuantity2.Text = "Quantity 2:";
            this.lblQuantity2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrice2
            // 
            this.txtPrice2.Enabled = false;
            this.txtPrice2.Location = new System.Drawing.Point(160, 270);
            this.txtPrice2.Name = "txtPrice2";
            this.txtPrice2.Size = new System.Drawing.Size(176, 31);
            this.txtPrice2.TabIndex = 51;
            this.txtPrice2.TextChanged += new System.EventHandler(this.txtPrice2_TextChanged);
            // 
            // Price2
            // 
            this.Price2.Location = new System.Drawing.Point(16, 270);
            this.Price2.Name = "Price2";
            this.Price2.Size = new System.Drawing.Size(128, 30);
            this.Price2.TabIndex = 50;
            this.Price2.Text = "Price 2:";
            this.Price2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 39);
            this.label1.TabIndex = 49;
            this.label1.Text = "Order Feed:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxOrderFeed
            // 
            this.comboBoxOrderFeed.DisplayMember = "Name";
            this.comboBoxOrderFeed.Enabled = false;
            this.comboBoxOrderFeed.Items.AddRange(new object[] {
            "Market",
            "Limit",
            "Stop Market",
            "Stop Limit"});
            this.comboBoxOrderFeed.Location = new System.Drawing.Point(160, 94);
            this.comboBoxOrderFeed.Name = "comboBoxOrderFeed";
            this.comboBoxOrderFeed.Size = new System.Drawing.Size(176, 33);
            this.comboBoxOrderFeed.TabIndex = 48;
            // 
            // lblCustomer
            // 
            this.lblCustomer.Location = new System.Drawing.Point(16, 44);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(128, 30);
            this.lblCustomer.TabIndex = 47;
            this.lblCustomer.Text = "Customer:";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCustomer
            // 
            this.cboCustomer.DisplayMember = "Customer";
            this.cboCustomer.Enabled = false;
            this.cboCustomer.Location = new System.Drawing.Point(160, 44);
            this.cboCustomer.Name = "cboCustomer";
            this.cboCustomer.Size = new System.Drawing.Size(176, 33);
            this.cboCustomer.TabIndex = 46;
            // 
            // txtOrderBook
            // 
            this.txtOrderBook.Location = new System.Drawing.Point(368, 44);
            this.txtOrderBook.Multiline = true;
            this.txtOrderBook.Name = "txtOrderBook";
            this.txtOrderBook.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOrderBook.Size = new System.Drawing.Size(384, 335);
            this.txtOrderBook.TabIndex = 45;
            // 
            // lblOrderType
            // 
            this.lblOrderType.Location = new System.Drawing.Point(16, 144);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(128, 29);
            this.lblOrderType.TabIndex = 44;
            this.lblOrderType.Text = "Order Type:";
            this.lblOrderType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSell
            // 
            this.btnSell.Enabled = false;
            this.btnSell.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSell.Location = new System.Drawing.Point(224, 414);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(112, 43);
            this.btnSell.TabIndex = 43;
            this.btnSell.Text = "Sell";
            this.btnSell.Click += new System.EventHandler(this.SellButton_Click);
            // 
            // btnBuy
            // 
            this.btnBuy.Enabled = false;
            this.btnBuy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBuy.Location = new System.Drawing.Point(112, 414);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(112, 43);
            this.btnBuy.TabIndex = 42;
            this.btnBuy.Text = "Buy";
            this.btnBuy.Click += new System.EventHandler(this.BuyButton_Click);
            // 
            // lblStopPrice
            // 
            this.lblStopPrice.Location = new System.Drawing.Point(16, 355);
            this.lblStopPrice.Name = "lblStopPrice";
            this.lblStopPrice.Size = new System.Drawing.Size(128, 30);
            this.lblStopPrice.TabIndex = 41;
            this.lblStopPrice.Text = "Stop Price:";
            this.lblStopPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStopPrice
            // 
            this.txtStopPrice.Enabled = false;
            this.txtStopPrice.Location = new System.Drawing.Point(160, 355);
            this.txtStopPrice.Name = "txtStopPrice";
            this.txtStopPrice.Size = new System.Drawing.Size(176, 31);
            this.txtStopPrice.TabIndex = 40;
            // 
            // cboOrderType
            // 
            this.cboOrderType.Enabled = false;
            this.cboOrderType.Items.AddRange(new object[] {
            "Market",
            "Limit",
            "Stop Market",
            "Stop Limit",
            "IOC",
            "OCO",
            "Trailing Stop Order",
            "Time Sliced",
            "AutoSpreader"});
            this.cboOrderType.Location = new System.Drawing.Point(160, 144);
            this.cboOrderType.Name = "cboOrderType";
            this.cboOrderType.Size = new System.Drawing.Size(176, 33);
            this.cboOrderType.TabIndex = 39;
            this.cboOrderType.SelectedIndexChanged += new System.EventHandler(this.orderTypeComboBox_SelectedIndexChanged);
            // 
            // lblQuantity
            // 
            this.lblQuantity.Location = new System.Drawing.Point(16, 228);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(128, 30);
            this.lblQuantity.TabIndex = 38;
            this.lblQuantity.Text = "Quantity:";
            this.lblQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Enabled = false;
            this.txtQuantity.Location = new System.Drawing.Point(160, 228);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(176, 31);
            this.txtQuantity.TabIndex = 37;
            // 
            // lblPrice
            // 
            this.lblPrice.Location = new System.Drawing.Point(16, 188);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(128, 30);
            this.lblPrice.TabIndex = 36;
            this.lblPrice.Text = "Price:";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrice
            // 
            this.txtPrice.Enabled = false;
            this.txtPrice.Location = new System.Drawing.Point(160, 188);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(176, 31);
            this.txtPrice.TabIndex = 35;
            // 
            // lblNotProduction
            // 
            this.lblNotProduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotProduction.Location = new System.Drawing.Point(16, 63);
            this.lblNotProduction.Name = "lblNotProduction";
            this.lblNotProduction.Size = new System.Drawing.Size(784, 25);
            this.lblNotProduction.TabIndex = 67;
            this.lblNotProduction.Text = "This sample is NOT to be used in production or during conformance testing.";
            this.lblNotProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWarning
            // 
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.Location = new System.Drawing.Point(16, 16);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(784, 43);
            this.lblWarning.TabIndex = 66;
            this.lblWarning.Text = "WARNING!";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSubmitOrder
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 24);
            this.ClientSize = new System.Drawing.Size(1085, 1583);
            this.Controls.Add(this.lblNotProduction);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.gboOrderEntry);
            this.Controls.Add(this.gboInstrumentMarketData);
            this.Controls.Add(this.gboInstrumentInfo);
            this.Controls.Add(this.sbaStatus);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Menu = this.mainMenu1;
            this.Name = "frmSubmitOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SubmitOrder";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmSubmitOrder_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.frmSubmitOrder_DragOver);
            this.gboInstrumentInfo.ResumeLayout(false);
            this.gboInstrumentInfo.PerformLayout();
            this.gboInstrumentMarketData.ResumeLayout(false);
            this.gboInstrumentMarketData.PerformLayout();
            this.gboOrderEntry.ResumeLayout(false);
            this.gboOrderEntry.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region Misc 

        /// <summary>
        /// populate the OrderFeed drop down menu.
        /// </summary>
        /// <remarks>
        /// comboBoxOrderFeed DisplayMember is set to Name to display the OrderFeed's Name property.
        /// </remarks>
        /// <param name="instrument">Instrument to find valid order feeds.</param>
        private void populateOrderFeedDropDown(Instrument instrument)
        {
            comboBoxOrderFeed.Items.Clear();
            foreach (OrderFeed orderFeed in instrument.GetValidOrderFeeds())
            {
                comboBoxOrderFeed.Items.Add(orderFeed);
            }
        }

        /// <summary>
        /// CustomerDefaultsChanged subscription callback.
        /// Update the Customer combo box.
        /// </summary>
        void m_customerDefaultsSubscription_CustomerDefaultsChanged(object sender, EventArgs e)
        {
            cboCustomer.Items.Clear();

            CustomerDefaultsSubscription cds = sender as CustomerDefaultsSubscription;
            foreach (CustomerDefaultEntry entry in cds.CustomerDefaults)
            {
                cboCustomer.Items.Add(entry);
            }
        }

        /// <summary>
        /// This function enables and disables the appropriate
        /// text boxes on the user interface.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
        private void orderTypeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboOrderType.SelectedIndex == 2 || cboOrderType.SelectedIndex == 3 || cboOrderType.SelectedIndex == 6)
            {
                // Enable the stop price text box if the selected order type is stop limit or stop market.
                txtStopPrice.Enabled = true;
            }
            else
            {
                // Clear and disable the stop price text box if not stop limit or stop market.
                txtStopPrice.Clear();
                txtStopPrice.Enabled = false;
            }

            if (cboOrderType.SelectedIndex == 0 || cboOrderType.SelectedIndex == 2)
            {
                // Clear and disable the price text box if the selected order type is market or stop market.
                txtPrice.Clear();
                txtPrice.Enabled = false;
            }
            else if (cboOrderType.SelectedIndex == 5)
            {
                // Enable the price text box if the if the selected order type is limit or stop limit.
                txtPrice.Enabled = true;
                txtPrice2.Enabled = true;
                txtQuantity2.Enabled = true;
            }
            else
            {
                // Enable the price text box if the if the selected order type is limit or stop limit.
                txtPrice.Enabled = true;
            }
        }

        private void txtPrice2_TextChanged(object sender, System.EventArgs e)
        {

        }
        #endregion

        #region SendOrder

        /// <summary>
        /// This function is called when the user clicks the buy button.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
        private void BuyButton_Click(object sender, System.EventArgs e)
        {
            // Call the SendOrder function with a Buy request.
        //    SendOrder(BuySell.Buy,1,5,"Stop",207500,203350);
            SendOrder(BuySell.Buy);
        }

        /// <summary>
        /// This function is called when the user clicks the sell button.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
        private void SellButton_Click(object sender, System.EventArgs e)
        {
            // Call the SendOrder function with a Sell request.
            SendOrder(BuySell.Sell);
        }

        /// <summary>
        /// This function sets up the OrderProfile and submits
        /// the order using the InstrumentTradeSubscription SendOrder method.
        /// </summary>
        /// <param name="buySell">The side of the market to place the order on.</param>
        private void SendOrder(BuySell buySell)
        {
            try
            {
                //Instrument inst = m_instrumentTradeSubscription.Instrument;
                Instrument inst = foundInstruments.ToArray()[0];
           //     Instrument inst = this.instrument;

                OrderFeed orderFeed = comboBoxOrderFeed.SelectedItem as OrderFeed;
                CustomerDefaultEntry customer = cboCustomer.SelectedItem as CustomerDefaultEntry;

                OrderProfile orderProfile = new OrderProfile(orderFeed, inst, customer.Customer);
                // Make second order profile for other OCO order
                OrderProfile orderProfile2 = new OrderProfile(orderFeed, inst, customer.Customer);
                    

                // Set for Buy or Sell.
                orderProfile.BuySell = buySell;
                orderProfile2.BuySell = buySell;

                // Set the quantity.
                orderProfile.QuantityToWork = Quantity.FromString(inst, txtQuantity.Text);

                // Determine which Order Type is selected.
                if (cboOrderType.SelectedIndex == 0)  // Market Order
                {
                    // Set the order type to "Market" for a market order.
                    orderProfile.OrderType = OrderType.Market;
                }
                else if (cboOrderType.SelectedIndex == 1)  // Limit Order
                {
                    // Set the order type to "Limit" for a limit order.
                    orderProfile.OrderType = OrderType.Limit;
                    // Set the limit order price.
                    //       orderProfile.LimitPrice = Price.FromString(inst, txtPrice.Text);
                    orderProfile.LimitPrice = Price.FromDouble(inst, 129.075, Rounding.Nearest);
                }
                else if (cboOrderType.SelectedIndex == 2)  // Stop Market Order
                {
                    // Set the order type to "Market" for a market order.
                    orderProfile.OrderType = OrderType.Market;
                    // Set the order modifiers to "Stop" for a stop order.
                    orderProfile.Modifiers = OrderModifiers.Stop;
                    // Set the stop price.
                    orderProfile.StopPrice = Price.FromString(inst, txtStopPrice.Text);
                }
                else if (cboOrderType.SelectedIndex == 3)  // Stop Limit Order
                {
                    // Set the order type to "Limit" for a limit order.
                    orderProfile.OrderType = OrderType.Limit;
                    // Set the order modifiers to "Stop" for a stop order.
                    orderProfile.Modifiers = OrderModifiers.Stop;
                    // Set the limit order price.
                    orderProfile.LimitPrice = Price.FromString(inst, txtPrice.Text);
                    // Set the stop price.
                    orderProfile.StopPrice = Price.FromString(inst, txtStopPrice.Text);
                }
                else if (cboOrderType.SelectedIndex == 4)  // IOC Order
                {
                    // Set the order type to "Limit" for a limit order.
                    orderProfile.OrderType = OrderType.Limit;
                    // Set the limit order price.
                    orderProfile.LimitPrice = Price.FromString(inst, txtPrice.Text);
                    // Set the order type to "Limit" for a limit order.
                    orderProfile.Restriction = OrderRestriction.Ioc;
                }
                else if (cboOrderType.SelectedIndex == 5)  // OCO Order
                {
                    // **ORDER 1**
                    // Set the order type to "Limit" for a limit order.
                    orderProfile.OrderType = OrderType.Limit;
                    // Set the limit order price.
                    orderProfile.LimitPrice = Price.FromString(inst, txtPrice.Text);
                    
                    // Tag for order profile data
                    orderProfile.OrderTag = countOrderTag.ToString();
          
                    // Keep record of OCO order pair
                    ocoOrders.Add(orderProfile.OrderTag, (OrderProfile)orderProfile.CreateNewOrderProfile());
                    countOrderTag += 1;

                    // **ORDER 2**
                    // Set the order type to "Limit" for a limit order.
                    orderProfile2.OrderType = OrderType.Limit;
                    // Set the limit order price.
                    orderProfile2.LimitPrice = Price.FromString(inst, txtPrice2.Text);
                    // Set the quantity.
                    orderProfile2.QuantityToWork = Quantity.FromString(inst, txtQuantity2.Text);

                    // Keep record of OCO order pair
                    orderProfile2.OrderTag = countOrderTag.ToString();
                   
                    // Order p = m_instrumentTradeSubscription.Orders[orderProfile.SiteOrderKey];

                    // Keep record of OCO order pair
                    ocoOrders.Add(orderProfile2.OrderTag, (OrderProfile)orderProfile2.CreateNewOrderProfile());
                    countOrderTag += 1;

                    // Send Order 2
                    m_instrumentTradeSubscription.SendOrder(orderProfile2);
                }
                else if (cboOrderType.SelectedIndex == 6)  // Trailing Stop Order
                {
                    // Set the order type to "Limit" for a limit order.
                    orderProfile.OrderType = OrderType.Limit;
                    // Set the order modifiers to "Stop" for a stop order.
                    orderProfile.SyntheticOrderModifier = SyntheticOrderModifier.TrailingLimit;
                    // Set Offset from Market Price
                    orderProfile.PriceMode = PriceMode.Offset;
                    // Send Order everytime SameSide of Market changes
                    orderProfile.TriggerPriceType = TriggerPriceType.SameSide;
                    // Set the Trailing Offset
                    short offset;
                    short.TryParse(txtPrice.Text, out offset);
                    orderProfile.TrailingOffset = offset;
                    // Trick Limit Price to reflect Market Price
                    if(buySell == BuySell.Sell){
                        orderProfile.LimitPrice = m_priceSubscription.Fields.GetBestAskPriceField(orderProfile.QuantityToWork).Value;
                    } else {
                        orderProfile.LimitPrice = m_priceSubscription.Fields.GetBestBidPriceField(orderProfile.QuantityToWork).Value;
                    }
                    
                }
                    // Broken time duration
                else if (cboOrderType.SelectedIndex == 7)
                {
                    orderProfile.OrderType = OrderType.Limit;
                    orderProfile.SlicerType = SlicerType.TimeDuration;
                    orderProfile.PriceMode = PriceMode.None;
                    orderProfile.StartTime = new DateTime(2016, 3, 15, 12, 12, 2);
                    orderProfile.EndTime = new DateTime(2016,3,20,12,12,5);
                    orderProfile.LimitPriceType = LimitPriceType.SameSide;
                    orderProfile.LimitOffset = 2;
                    TimeSpan tspan = orderProfile.EndTime - orderProfile.StartTime;
                    orderProfile.InterSliceDelay = 100;
                    orderProfile.InterSliceDelayTimeUnits = TimeUnits.MSec;
                    orderProfile.EndTimeAction = EndTimeAction.GoToMarket;
                    orderProfile.LeftoverAction = LeftoverAction.Leave;
                    orderProfile.LeftoverActionTime = LeftoverActionTime.AtEnd;
                }
                
                    //Autospreader of first two instruments dragged 1/-1
                else if (cboOrderType.SelectedIndex == 8)
                {
                    createAndSendSpread(foundInstruments[0], foundInstruments[1]);
                }
                // Send the order.

                m_TradeSubscription.SendOrder(orderProfile);

                // Update the GUI.
                txtOrderBook.Text += String.Format("Send {0} {1}|{2}@{3}{4}",
                    orderProfile.SiteOrderKey,
                    orderProfile.BuySell.ToString(),
                    orderProfile.QuantityToWork.ToString(),
                    orderProfile.OrderType == OrderType.Limit ? orderProfile.LimitPrice.ToString() : "Market Price",
                    System.Environment.NewLine);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        #region Autospreader Order 
        
        private void createAndSendSpread(Instrument instr1, Instrument instr2)
        {

            sd_subscrtiption = new SpreadDetailSubscription(m_TTAPI.Session, Dispatcher.Current);
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
            sld1.CustomerName = "CME MEXDER";
            sld2.CustomerName = "CME MEXDER";

            MarksSpreadObject spreadObject = new MarksSpreadObject(sd, sld1, instr1, sld2, instr2);
            allSpreads.Add(spreadObject);

            // Create Price Subscriptions for each leg to get estimated trading prices of spread
            PriceSubscription leg1PriceSub = new PriceSubscription(instr1, Dispatcher.Current);
            PriceSubscription leg2PriceSub = new PriceSubscription(instr2, Dispatcher.Current);

            leg1PriceSub.FieldsUpdated += new FieldsUpdatedEventHandler(m_leg_priceSubscription_FieldsUpdated);
            leg2PriceSub.FieldsUpdated += new FieldsUpdatedEventHandler(m_leg_priceSubscription_FieldsUpdated);

            leg1PriceSub.Start();
            leg2PriceSub.Start();

            // Create Fill Subscription to track the fills that occur for each leg
            fillSub = new FillsSubscription(m_TTAPI.Session, Dispatcher.Current);
            fillSub.FillAdded += new EventHandler<FillAddedEventArgs>(m_spreadTracking_fillSubscription_FillAdded);

            // Create an Instrument corresponding to the synthetic spread
            CreateAutospreaderInstrumentRequest casReq = new CreateAutospreaderInstrumentRequest(m_TTAPI.Session, Dispatcher.Current, sd);
            casReq.Completed += new EventHandler<CreateAutospreaderInstrumentRequestEventArgs>(casReq_Completed);
            casReq.Tag = "Yo Momma";
            casReq.Submit();
        }

        private void m_spreadTracking_fillSubscription_FillAdded(object sender, FillAddedEventArgs e)
        {
            if(e.Fill.IsSseChildFill)
            {
                string thinking = e.Fill.InstrumentDetails.Name;
                string stillThinking = e.Fill.SiteOrderKey;
                Console.WriteLine("Parent SeriesKey for Fill: " + e.Fill.InstrumentKey.SeriesKey);
                foreach (MarksSpreadObject spread in allSpreads)
                {
                    Console.WriteLine("Parent Instrument is AutoSpreader Spread: " + e.Fill.InstrumentKey.IsAutospreader);
                    Console.WriteLine("Parent Instrument is AutoSpreader Spread (from MarksSpreadObject): " + spread.sd.InstrumentKey.IsAutospreader);
                    Console.WriteLine("Parent SeriesKey (from MarksSpreadObject): " + spread.sd.InstrumentKey.SeriesKey);
                    Console.WriteLine("Parent Instrument name (from MarksSpreadObject): " + spread.sd.Name);

                    Console.WriteLine("Child Leg1 Instrument is AutoSpreader Spread (from MarksSpreadObject): " + spread.sld1.InstrumentKey.IsAutospreader);
                    Console.WriteLine("Child Leg1 SeriesKey (from MarksSpreadObject): " + spread.sld1.InstrumentKey.SeriesKey);
                    Console.WriteLine("Child Leg1 Instrument name (from MarksSpreadObject): " + spread.inst1.Name);

                    Console.WriteLine("Child Leg2 Instrument is AutoSpreader Spread (from MarksSpreadObject): " + spread.sld2.InstrumentKey.IsAutospreader);
                    Console.WriteLine("Child Leg2 SeriesKey (from MarksSpreadObject): " + spread.sld2.InstrumentKey.SeriesKey);
                    Console.WriteLine("Child Leg2 Instrument name (from MarksSpreadObject): " + spread.inst2.Name);
                }
            }
        }

        private void m_leg_priceSubscription_FieldsUpdated(object sender, FieldsUpdatedEventArgs e)
        {
            foreach (MarksSpreadObject spread in allSpreads)
            {
                if (String.Equals(e.Fields.InstrumentDetails.Name, spread.inst1.InstrumentDetails.Name, StringComparison.InvariantCultureIgnoreCase) )
                {
                    Double.TryParse(e.Fields.GetDirectBidPriceField().FormattedValue, out inst1Price); 
                }
                else if (String.Equals(e.Fields.InstrumentDetails.Name, spread.inst2.InstrumentDetails.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    Double.TryParse(e.Fields.GetDirectBidPriceField().FormattedValue, out inst2Price);
                }

                if (inst1Price != 0 && inst2Price != 0)
                {
                    spreadPrice = inst1Price + inst2Price;
                    txtChange.Text = "Price of Buy Order: " + spreadPrice.ToString();
                }    
            }
        }
        
        void sds_SpreadDetailsAdded(object sender, SpreadDetailsEventArgs e)
        {
            Console.WriteLine("sds_SpreadDetailsAdded() hit");
            //TEST - see if update works after firing
            if(allSpreads.Count != 0)
            {
                MarksSpreadObject spreadObj = allSpreads.ToArray()[0];
                SpreadDetails sd = spreadObj.sd;
                SpreadLegDetails sld1 = spreadObj.sld1;
                SpreadLegDetails sld2 = spreadObj.sld2;

                // This block modifies the first created spread
                //sd.Legs.Remove(1);
                //sld2.PriceMultiplier = 1.1;
                //sld2.ActiveQuoting = false;
                //sd.Legs.Append(sld2);

                AutospreaderManager manager = m_TTAPI.AutospreaderManager;
                //manager.UpdateSpreadDetails(sd);
                //update spread
                //CreateAutospreaderInstrumentRequest casReq = new CreateAutospreaderInstrumentRequest(m_TTAPI.Session, Dispatcher.Current, sd);
                //casReq.Submit();


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
            if (e.Instrument != null)
            {
                // deploy the Autospreader Instrument to the specified ASE
                e.Instrument.TradableStatusChanged += new EventHandler<TradableStatusChangedEventArgs>(Instrument_TradableStatusChanged);

                LaunchReturnCode lrc = e.Instrument.LaunchToOrderFeed(e.Instrument.GetValidOrderFeeds()[0]); ;
                if (lrc == LaunchReturnCode.Success)
                {
                    Console.WriteLine("Autospreader launch was a success according to casReq_Completed");
                    foreach (MarksSpreadObject spread in allSpreads)
                    {
                        if(e.Instrument.SpreadDetails != null)
                        {
                            // Double check that AutospreaderInstrument has the same SeriesKey as the parent
                            if(String.Equals(e.Instrument.SpreadDetails.InstrumentKey.SeriesKey, spread.sd.InstrumentKey.SeriesKey, StringComparison.InvariantCultureIgnoreCase))
                            {
                                // Place Market Order on the AutoSpreader Instrument
                                OrderProfile parentOrder = new OrderProfile(e.Instrument.GetValidOrderFeeds()[0], e.Instrument);
                                parentOrder.OrderType = OrderType.Market;
                                parentOrder.BuySell = BuySell.Buy;
                                m_TradeSubscription.SendOrder(parentOrder);
                                Console.WriteLine("Parent SeriesKey from MarksSpreadObject: " + spread.sd.InstrumentKey.SeriesKey);
                                Console.WriteLine("Parent SeriesKey from casReq_Completed: " + e.Instrument.SpreadDetails.InstrumentKey.SeriesKey);
                            }
                        }
                    }
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

            public MarksSpreadObject(SpreadDetails sd, SpreadLegDetails sl1d, Instrument inst1, SpreadLegDetails sl2d, Instrument inst2)
            {
                this.sd = sd;
                this.sld1 = sl1d;
                this.sld2 = sl2d;
                this.inst1 = inst1;
                this.inst2 = inst2;
            }
        }

        private void Instrument_TradableStatusChanged(object sender, TradableStatusChangedEventArgs e)
        {
            Console.WriteLine("Autospreader launch was a success? " + e.Value);
        }

        #endregion

        #region Fill Update

        void m_instrumentTradeSubscription_OrderFilled(object sender, OrderFilledEventArgs e)
        {
            foreach (MarksSpreadObject spread in allSpreads)
            {
                Console.WriteLine("SeriesKey from OrderFilledEventArgs: " + e.Fill.InstrumentKey.SeriesKey);
                Console.WriteLine("Parent Instrument is AutoSpreader Spread: " + e.Fill.InstrumentKey.IsAutospreader);
                Console.WriteLine("Parent Instrument is AutoSpreader Spread (from MarksSpreadObject): " + spread.sd.InstrumentKey.IsAutospreader);
                Console.WriteLine("Parent SeriesKey (from MarksSpreadObject): " + spread.sd.InstrumentKey.SeriesKey);
                Console.WriteLine("Parent Instrument name (from MarksSpreadObject): " + spread.sd.Name);

                Console.WriteLine("Child Leg1 Instrument is AutoSpreader Spread (from MarksSpreadObject): " + spread.sld1.InstrumentKey.IsAutospreader);
                Console.WriteLine("Child Leg1 SeriesKey (from MarksSpreadObject): " + spread.sld1.InstrumentKey.SeriesKey);
                Console.WriteLine("Child Leg1 Instrument name (from MarksSpreadObject): " + spread.inst1.Name);

                Console.WriteLine("Child Leg2 Instrument is AutoSpreader Spread (from MarksSpreadObject): " + spread.sld2.InstrumentKey.IsAutospreader);
                Console.WriteLine("Child Leg2 SeriesKey (from MarksSpreadObject): " + spread.sld2.InstrumentKey.SeriesKey);
                Console.WriteLine("Child Leg2 Instrument name (from MarksSpreadObject): " + spread.inst2.Name);
            }
        }

        #endregion

        #region Price Subscription - updates

        /// <summary>
        /// PriceSubscription FieldsUpdated event.
        /// </summary>
        void m_priceSubscription_FieldsUpdated(object sender, FieldsUpdatedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.UpdateType == UpdateType.Snapshot)
                {
                    updatePrices(e.Fields);
                }
                else if (e.UpdateType == UpdateType.Update)
                {
                    updatePrices(e.Fields);
                }
            }
            else
            {
                Console.WriteLine(String.Format("PriceSubscription FieldsUpdated Error: {0}", e.Error.Message));
            }
        }

        /// <summary>
        /// Update the price information.
        /// </summary>
        /// <param name="fields">PriceSubscriptionFields</param>
        private void updatePrices(PriceSubscriptionFields fields)
        {
            txtBidPrice.Text = fields.GetBestBidPriceField().Value.ToString();
            txtAskPrice.Text = fields.GetBestAskPriceField().Value.ToString();
            txtLastPrice.Text = fields.GetLastTradedPriceField().Value.ToString();
            //txtChange.Text = fields.GetNetChangeField().Value.ToString();
            QuantityField quant = fields.GetTotalTradedQuantityField();
        }

        #endregion

        #region trade subscription updates/Update GUI for TradeSubscription events.


        private void m_instrumentTradeSubscription_OrderUpdated(object sender, OrderUpdatedEventArgs e)
        {

            //// Copy Global Dictionary of Orders from the InstrumentTradeSubscription ...
            //// ... to the local method - everytime there is an update, the most up-to-date Order list is copied
            KeyValuePair<string, Order>[] localCopyOfOrders = new KeyValuePair<string, Order>[m_instrumentTradeSubscription.Orders.Count];
            m_instrumentTradeSubscription.Orders.CopyTo(localCopyOfOrders,0);
            

            //foreach (string siteOrderKey in localCopyOfOrders.Keys)
            //{
            //    if (m_instrumentTradeSubscription.Orders.ContainsKey(siteOrderKey))
            //    {
            //        Order order = null; // This is populated in next line
            //        bool orderWithKeyWasFound = localCopyOfOrders.TryGetValue(siteOrderKey, out order);
            //        if ()
            //        {
            //            // ... process order 
            //        }
            //    }orderWithKeyWasFound
            //} 



            InstrumentKey changingOrderKey = e.NewOrder.InstrumentKey;
            if(e.NewOrder.CanceledQuantity != 0 && 
                parentInstr_Legs_Table.ContainsKey(changingOrderKey) )
            {


            }
            Console.WriteLine("Turkish Delights");
        }

        /// <summary>
        /// OrderRejected InstrumentTradeSubscription callback.
        /// </summary>
        /// <param name="sender">Sender (InstrumentTradeSubscription)</param>
        /// <param name="e">OrderRejectedEventArgs</param>
        void m_instrumentTradeSubscription_OrderRejected(object sender, OrderRejectedEventArgs e)
        {
            // X is going to give it to ya
            // X gonna give it to ya - DMX
            // Dragula - Rob Zombie
            // "Dig through the ditches, and burn through the witches - and slam in the back of the... am I taking it up?"

            txtOrderBook.Text += String.Format("Rejected {0} {1}{2}",
                e.Order.SiteOrderKey,
                e.Message,
                System.Environment.NewLine);
        }

        /// <summary>
        /// OrderAdded InstrumentTradeSubscription callback.
        /// </summary>
        /// <param name="sender">Sender (InstrumentTradeSubscription)</param>
        /// <param name="e">OrderAddedEventArgs</param>
        void m_instrumentTradeSubscription_OrderAdded(object sender, OrderAddedEventArgs e)
        {
            txtOrderBook.Text += String.Format("Added {0} {1}|{2}@{3}{4}",
                e.Order.SiteOrderKey,
                e.Order.BuySell.ToString(),
                e.Order.OrderQuantity.ToString(),
                e.Order.OrderType == OrderType.Limit ? e.Order.LimitPrice.ToString() : "Market Price",
                System.Environment.NewLine);
        }
        #endregion

        #region FindInstrument
        /// <summary>
        /// Function to find a list of InstrumentKeys.
        /// </summary>
        /// <param name="keys">List of InstrumentKeys.</param>
        public void FindInstrument(IList<InstrumentKey> keys)
        {
            foreach (InstrumentKey key in keys)
            {
                //HACK
             //   foundInstruments.Add(key);

                // Update the Status Bar text.
                UpdateStatusBar(String.Format("TT API FindInstrument {0}", key.ToString()));
                
                InstrumentLookupSubscription instrRequest = new InstrumentLookupSubscription(m_TTAPI.Session, Dispatcher.Current, key);
                instrRequest.Update += instrRequest_Completed;
                instrRequest.Tag = key.ToString();
                instrRequest.Start();
                
                // Only allow the first instrument.
                break;
            }
        }

        /// <summary>
        /// Instrument request completed event.
        /// </summary>
        void instrRequest_Completed(object sender, InstrumentLookupSubscriptionEventArgs e)
        {
            if (e.IsFinal && e.Instrument != null)
            {
                try
                {
                    UpdateStatusBar(String.Format("TT API FindInstrument {0}", e.Instrument.Name));
                    instrumentFound(e.Instrument);
                }
                catch (Exception err)
                {
                    UpdateStatusBar(String.Format("TT API FindInstrument Exception: {0}", err.Message));
                }
            }
            else if (e.IsFinal)
            {
                UpdateStatusBar(String.Format("TT API FindInstrument Instrument Not Found: {0}", e.Error));
            }
            else
            {
                UpdateStatusBar(String.Format("TT API FindInstrument Instrument Not Found: (Still Searching) {0}", e.Error));
            }
        }

        /// <summary>
        /// Create subscriptions and update the GUI.
        /// </summary>
        /// <param name="instrument">Instrument to create subscriptions with.</param>
        private void instrumentFound(Instrument instrument)
        {
            foundInstruments.Add(instrument);
            //HACK
            if (foundInstruments.Count == 2)
            {

                txtExchange.Text = instrument.Key.MarketKey.Name;
                txtProduct.Text = instrument.Key.ProductKey.Name;
                txtProductType.Text = instrument.Key.ProductKey.Type.Name;
                txtContract.Text = instrument.Name;

                //LegList legs = instrument.GetLegs();
                //if(legs.Count == 2)
                //{
                //    IEnumerator<Leg> enumerator = legs.GetEnumerator();
                //    enumerator.MoveNext();

                //    Instrument inst1 = enumerator.Current.Instrument;
                //    InstrumentTradeSubscription legSub1 = new InstrumentTradeSubscription(m_TTAPI.Session, Dispatcher.Current,
                //        inst1);
                //    legSub1.OrderFilled += new EventHandler<OrderFilledEventArgs>(m_instrumentTradeSubscription_OrderFilled);
                //    legSub1.Start();

                //    enumerator.MoveNext();

                //    InstrumentTradeSubscription legSub2 = new InstrumentTradeSubscription(m_TTAPI.Session, Dispatcher.Current,
                //        enumerator.Current.Instrument);
                //    legSub2.OrderFilled += new EventHandler<OrderFilledEventArgs>(m_instrumentTradeSubscription_OrderFilled);
                //    legSub2.Start();
                //}

                m_priceSubscription = new PriceSubscription(instrument, Dispatcher.Current);
                m_priceSubscription.FieldsUpdated += new FieldsUpdatedEventHandler(m_priceSubscription_FieldsUpdated);
                m_priceSubscription.Start();

                m_TradeSubscription = new TradeSubscription(m_TTAPI.Session, Dispatcher.Current);
                m_TradeSubscription.OrderAdded += new EventHandler<OrderAddedEventArgs>(m_instrumentTradeSubscription_OrderAdded);
                m_TradeSubscription.OrderRejected += new EventHandler<OrderRejectedEventArgs>(m_instrumentTradeSubscription_OrderRejected);
                m_TradeSubscription.OrderFilled += new EventHandler<OrderFilledEventArgs>(m_instrumentTradeSubscription_OrderFilled);
                m_TradeSubscription.OrderUpdated += new EventHandler<OrderUpdatedEventArgs>(m_instrumentTradeSubscription_OrderUpdated);
                m_TradeSubscription.Start();
                
                populateOrderFeedDropDown(instrument);

                // Enable the user interface items.
                txtQuantity.Enabled = true;
                cboOrderType.Enabled = true;
                comboBoxOrderFeed.Enabled = true;
                cboCustomer.Enabled = true;
                btnBuy.Enabled = true;
                btnSell.Enabled = true;
            }
        }
        
        #endregion

        #region Drag and Drop
        /// <summary>
        /// Form drag and drop event handler.
        /// The form must enable "AllowDrop" for these events to fire.
        /// </summary>
        private void frmSubmitOrder_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.HasInstrumentKeys())
            {
                FindInstrument(e.Data.GetInstrumentKeys());
            }
        }

        /// <summary>
        /// Form drag over event handler.
        /// The form must enable "AllowDrop" for these events to fire.
        /// </summary>
        private void frmSubmitOrder_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.HasInstrumentKeys())
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        #endregion

        #region UpdateStatusBar
        /// <summary>
        /// Update the status bar and write the message to the console in a thread safe way.
        /// </summary>
        /// <param name="message">Message to update the status bar with.</param>
        delegate void UpdateStatusBarCallback(string message);
        public void UpdateStatusBar(string message)
        {
            if (this.InvokeRequired)
            {
                UpdateStatusBarCallback statCB = new UpdateStatusBarCallback(UpdateStatusBar);
                this.Invoke(statCB, new object[] { message });
            }
            else
            {
                // Update the status bar.
                sbaStatus.Text = message;

                // Also write this message to the console.
                Console.WriteLine(message);
            }
        }
        #endregion

        /// <summary>
        /// Display the About dialog box.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
        private void AboutMenuItem_Click(object sender, System.EventArgs e)
        {
            AboutDTS aboutForm = new AboutDTS();
            aboutForm.ShowDialog(this);
        }

        #endregion
    }
}