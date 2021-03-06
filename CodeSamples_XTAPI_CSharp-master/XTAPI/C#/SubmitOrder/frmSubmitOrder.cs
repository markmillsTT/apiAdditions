/***************************************************************************
 *    
 *      Copyright (c) 2005 Trading Technologies International, Inc.
 *                     All Rights Reserved Worldwide
 *
 *        * * *   S T R I C T L Y   P R O P R I E T A R Y   * * *
 *
 * WARNING:  This file is the confidential property of Trading Technologies
 * International, Inc. and is to be maintained in strict confidence.  For
 * use only by those with the express written permission and license from
 * Trading Technologies International, Inc.  Unauthorized reproduction,
 * distribution, use or disclosure of this file or any program (or document)
 * derived from it is prohibited by State and Federal law, and by local law
 * outside of the U.S. 
 *
 ***************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using XTAPI;
using System.Linq;


namespace XTAPI_Samples
{
	/// <summary>
	/// SubmitOrder
    /// 
    /// This example demonstrates using the XTAPI to submit an order.  The order types
    /// available in the application are market, limit, stop market and stop limit.  
	/// </summary>
	public class frmSubmitOrder : Form
    {
        // Declare the XTAPI objects.
		private XTAPI.TTDropHandlerClass m_TTDropHandler = null;
		private XTAPI.TTInstrObj m_TTInstrObj = null;
        private XTAPI.TTInstrNotifyClass m_TTInstrNotify = null;
        private Button button1;
		private XTAPI.TTOrderSetClass m_TTOrderSet = null;

        /// <summary>
        /// Upon the application form loading, the TTDropHandler, TTOrderSet, and TTInstrNotify 
        /// objects are initialized, and the required events are subscribed.
        /// </summary>
		public frmSubmitOrder()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Instantiate the drag and drop handler class.					 
			m_TTDropHandler = new XTAPI.TTDropHandlerClass();
			
			// Register the active form for drag and drop.
			m_TTDropHandler.RegisterDropWindow((int) this.Handle);	
			
			// Associate the drop and drag callback event.
			m_TTDropHandler.OnNotifyDrop += new XTAPI._ITTDropHandlerEvents_OnNotifyDropEventHandler(this.m_TTDropHandler_OnNotifyDrop);
			
			// Instantiate the instrument notification class.
			m_TTInstrNotify = new XTAPI.TTInstrNotifyClass();
			
			// Subscribe to the TTInstrNotify events.
			m_TTInstrNotify.OnNotifyFound += new XTAPI._ITTInstrNotifyEvents_OnNotifyFoundEventHandler(this.m_TTInstrNotify_OnNotifyFound);
			m_TTInstrNotify.OnNotifyUpdate += new XTAPI._ITTInstrNotifyEvents_OnNotifyUpdateEventHandler(this.m_TTInstrNotify_OnNotifyUpdate);		

		}

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

		#region Windows Form Designer generated code

        private IContainer components;
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
            this.sbaStatus.Location = new System.Drawing.Point(0, 812);
            this.sbaStatus.Name = "sbaStatus";
            this.sbaStatus.Size = new System.Drawing.Size(905, 42);
            this.sbaStatus.SizingGrip = false;
            this.sbaStatus.TabIndex = 62;
            this.sbaStatus.Text = "Drag and Drop an instrument from the Market Grid in X_TRADER to this window.";
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
            this.gboInstrumentInfo.Location = new System.Drawing.Point(16, 103);
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
            this.lblProductType.Size = new System.Drawing.Size(160, 29);
            this.lblProductType.TabIndex = 38;
            this.lblProductType.Text = "Product Type:";
            this.lblProductType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(192, 89);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(200, 31);
            this.txtProduct.TabIndex = 35;
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(80, 89);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(96, 29);
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
            this.lblExchange.Text = "Exchange:";
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
            this.gboInstrumentMarketData.Location = new System.Drawing.Point(464, 103);
            this.gboInstrumentMarketData.Name = "gboInstrumentMarketData";
            this.gboInstrumentMarketData.Size = new System.Drawing.Size(336, 251);
            this.gboInstrumentMarketData.TabIndex = 64;
            this.gboInstrumentMarketData.TabStop = false;
            this.gboInstrumentMarketData.Text = "Instrument Market Data";
            // 
            // lblAskPrice
            // 
            this.lblAskPrice.Location = new System.Drawing.Point(16, 89);
            this.lblAskPrice.Name = "lblAskPrice";
            this.lblAskPrice.Size = new System.Drawing.Size(128, 29);
            this.lblAskPrice.TabIndex = 46;
            this.lblAskPrice.Text = "Ask Price:";
            this.lblAskPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAskPrice
            // 
            this.txtAskPrice.Location = new System.Drawing.Point(160, 89);
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
            this.lblLastPrice.Size = new System.Drawing.Size(128, 29);
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
            this.gboOrderEntry.Size = new System.Drawing.Size(784, 355);
            this.gboOrderEntry.TabIndex = 65;
            this.gboOrderEntry.TabStop = false;
            this.gboOrderEntry.Text = "Order Entry";
            // 
            // button1
            // 
            this.button1.Enabled = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(7, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 42);
            this.button1.TabIndex = 48;
            this.button1.Text = "Test";
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.txtOrderBook.Size = new System.Drawing.Size(384, 281);
            this.txtOrderBook.TabIndex = 45;
            // 
            // lblOrderType
            // 
            this.lblOrderType.Location = new System.Drawing.Point(16, 89);
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
            this.btnSell.Location = new System.Drawing.Point(224, 281);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(112, 42);
            this.btnSell.TabIndex = 43;
            this.btnSell.Text = "Sell";
            this.btnSell.Click += new System.EventHandler(this.SellButton_Click);
            // 
            // btnBuy
            // 
            this.btnBuy.Enabled = false;
            this.btnBuy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBuy.Location = new System.Drawing.Point(112, 281);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(112, 42);
            this.btnBuy.TabIndex = 42;
            this.btnBuy.Text = "Buy";
            this.btnBuy.Click += new System.EventHandler(this.BuyButton_Click);
            // 
            // lblStopPrice
            // 
            this.lblStopPrice.Location = new System.Drawing.Point(16, 222);
            this.lblStopPrice.Name = "lblStopPrice";
            this.lblStopPrice.Size = new System.Drawing.Size(128, 29);
            this.lblStopPrice.TabIndex = 41;
            this.lblStopPrice.Text = "Stop Price:";
            this.lblStopPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStopPrice
            // 
            this.txtStopPrice.Enabled = false;
            this.txtStopPrice.Location = new System.Drawing.Point(160, 222);
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
            "Stop Limit"});
            this.cboOrderType.Location = new System.Drawing.Point(160, 89);
            this.cboOrderType.Name = "cboOrderType";
            this.cboOrderType.Size = new System.Drawing.Size(176, 33);
            this.cboOrderType.TabIndex = 39;
            this.cboOrderType.SelectedIndexChanged += new System.EventHandler(this.orderTypeComboBox_SelectedIndexChanged);
            // 
            // lblQuantity
            // 
            this.lblQuantity.Location = new System.Drawing.Point(16, 177);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(128, 30);
            this.lblQuantity.TabIndex = 38;
            this.lblQuantity.Text = "Quantity:";
            this.lblQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Enabled = false;
            this.txtQuantity.Location = new System.Drawing.Point(160, 177);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(176, 31);
            this.txtQuantity.TabIndex = 37;
            // 
            // lblPrice
            // 
            this.lblPrice.Location = new System.Drawing.Point(16, 133);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(128, 29);
            this.lblPrice.TabIndex = 36;
            this.lblPrice.Text = "Price:";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPrice
            // 
            this.txtPrice.Enabled = false;
            this.txtPrice.Location = new System.Drawing.Point(160, 133);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(176, 31);
            this.txtPrice.TabIndex = 35;
            // 
            // lblNotProduction
            // 
            this.lblNotProduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotProduction.Location = new System.Drawing.Point(16, 63);
            this.lblNotProduction.Name = "lblNotProduction";
            this.lblNotProduction.Size = new System.Drawing.Size(784, 26);
            this.lblNotProduction.TabIndex = 67;
            this.lblNotProduction.Text = "This sample is NOT to be used in production or during conformance testing.";
            this.lblNotProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWarning
            // 
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.Location = new System.Drawing.Point(16, 17);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(784, 42);
            this.lblWarning.TabIndex = 66;
            this.lblWarning.Text = "WARNING!";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSubmitOrder
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 24);
            this.ClientSize = new System.Drawing.Size(905, 854);
            this.Controls.Add(this.lblNotProduction);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.gboOrderEntry);
            this.Controls.Add(this.gboInstrumentMarketData);
            this.Controls.Add(this.gboInstrumentInfo);
            this.Controls.Add(this.sbaStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Menu = this.mainMenu1;
            this.Name = "frmSubmitOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SubmitOrder";
            this.gboInstrumentInfo.ResumeLayout(false);
            this.gboInstrumentInfo.PerformLayout();
            this.gboInstrumentMarketData.ResumeLayout(false);
            this.gboInstrumentMarketData.PerformLayout();
            this.gboOrderEntry.ResumeLayout(false);
            this.gboOrderEntry.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			// Enable Visual Styles for XP Look and Feel.
			Application.EnableVisualStyles();
			Application.Run(new frmSubmitOrder());
		}

        /// <summary>
        /// This function is called when one or more instruments are dragged and dropped from 
        /// the Market Grid in X_TRADER.
        /// </summary>
		private void m_TTDropHandler_OnNotifyDrop()
		{	
			// Update the Status Bar text.
			sbaStatus.Text = "Drag & Drop detected.  Initializing instrument...";

			try
			{	
				if (m_TTInstrObj != null)	
				{
					// Detach previously attached instrument.
					m_TTInstrNotify.DetachInstrument(m_TTInstrObj);
                    m_TTInstrObj = null;
				}

                // Instantiate OrderSet
                m_TTOrderSet = new XTAPI.TTOrderSetClass();

                // 'Set the Net Limits to false
                m_TTOrderSet.Set("NetLimits", false);
                // Set normal level of detail in our order status events
                m_TTOrderSet.OrderStatusNotifyMode = XTAPI.enumOrderStatusNotifyMode.ORD_NOTIFY_NORMAL;
                // Add Event to OrderSet
                m_TTOrderSet.OnOrderSubmitted += new XTAPI._ITTOrderSetEvents_OnOrderSubmittedEventHandler(m_TTOrderSet_OnOrderSubmitted);
                m_TTOrderSet.OnOrderFilled += new XTAPI._ITTOrderSetEvents_OnOrderFilledEventHandler(m_TTOrderSet_OnOrderFilled);

                //'Attach the Orderset to the Instrument Notify Object
                m_TTInstrNotify.AttachOrderSet(m_TTOrderSet);

                //'Enable order entry, fills, order set updates
                m_TTOrderSet.EnableOrderSend = 1;
                m_TTOrderSet.EnableOrderFillData = 1;
                m_TTOrderSet.EnableOrderSetUpdates = 1;

                //'Open the order set
                m_TTOrderSet.Open(1);

                // Obtain the TTInstrObj from the TTDropHandler object.
                m_TTInstrObj = (XTAPI.TTInstrObj)m_TTDropHandler[1];

                m_TTInstrObj.OrderSet = m_TTOrderSet;

				// Attach the TTInstrObj to the TTInstrNotify for price update events.
				m_TTInstrNotify.AttachInstrument(m_TTInstrObj);
				
				// Open the TTInstrObj.
				m_TTInstrObj.Open(0);	// enable Market Depth:  1 - true, 0 - false

				// Clear drop handler list.
				m_TTDropHandler.Reset();

                System.Threading.Thread.Sleep(5000);
			}
			catch (Exception ex)
			{
				// Display exception message.
				MessageBox.Show(ex.Message, "Exception");
			}
		}

        private void m_TTOrderSet_OnOrderFilled(TTOrderObj pNewOrderObj, TTOrderObj pOldOrderObj, string sSiteOrderKey, enumOrderNotifyState eOrderState, int fillQty)
        {
            string orderUserTag1 = pNewOrderObj.get_Get("UserTag").ToString();
            string orderUserTag2 = pOldOrderObj.get_Get("UserTag").ToString();
            Console.WriteLine("OnOrderFilled hit " + orderUserTag1 + orderUserTag2);
        }

        private void m_TTOrderSet_OnOrderSubmitted(TTOrderObj pNewOrderObj, TTOrderObj pOldOrderObj, string sSiteOrderKey, enumOrderAction eOrderAction, int wrkQty, string sOrderType, string sOrderTraits)
        {
            string orderUserTag1 = pNewOrderObj.get_Get("UserTag").ToString();
            string orderUserTag2 = pOldOrderObj.get_Get("UserTag").ToString();
            Console.WriteLine("OnOrderFilled hit " + orderUserTag1 + orderUserTag2);
        }

        /// <summary>
        /// This function is called when an instrument is found after it is opened.
        /// Available customer names are populated, the TTOrderSet is opened and
        /// the order entry items are enabled
        /// </summary>
        /// <param name="pNotify">TTInstrNotify object</param>
        /// <param name="pInstr">TTInstrObj object</param>
		private void m_TTInstrNotify_OnNotifyFound(XTAPI.TTInstrNotify pNotify, XTAPI.TTInstrObj pInstr)
		{
			// Update the Status Bar text.
			sbaStatus.Text = "Instrument Found.";
			
			// Retrieve the instrument information using the TTInstrObj Get Properties.
			Array data = (Array) pInstr.get_Get("Exchange,Product,ProdType,Contract,Bid$,Ask$,Last$,Change$");

			txtExchange.Text = (string)data.GetValue(0);
			txtProduct.Text = (string)data.GetValue(1);
			txtProductType.Text = (string)data.GetValue(2);
			txtContract.Text = (string)data.GetValue(3);
		
			txtBidPrice.Text = (string)data.GetValue(4);	
			txtAskPrice.Text = (string)data.GetValue(5);
			txtLastPrice.Text = (string)data.GetValue(6);
			txtChange.Text = (string)data.GetValue(7);
            
			// Obtain the available customer names and add them to the ComboBox.
			XTAPI.TTOrderProfileClass orderProfile = new XTAPI.TTOrderProfileClass();
            foreach (string entry in orderProfile.Customers as Array)
            {
                cboCustomer.Items.Add(entry);
            }
		
			// Set the first item in both combo boxes.
            cboCustomer.SelectedIndex = 0;
            cboOrderType.SelectedIndex = 0;

			// Set the Net Limits to false.
			m_TTOrderSet.Set("NetLimits",false);
			// Open the TTOrderSet with send orders enabled.
		//	m_TTOrderSet.Open(1);

			// Enable the user interface items.
			txtQuantity.Enabled = true;
			cboOrderType.Enabled = true;
			cboCustomer.Enabled = true;
			btnBuy.Enabled = true;
			btnSell.Enabled = true;	
		}

        /// <summary>
        /// This function is called when an instrument update occurs (i.e. LTP changes).
        /// </summary>
        /// <param name="pNotify">TTInstrNotify object</param>
        /// <param name="pInstr">TTInstrObj object</param>
		private void m_TTInstrNotify_OnNotifyUpdate(XTAPI.TTInstrNotify pNotify, XTAPI.TTInstrObj pInstr)
		{
			// Retrieve the instrument information using the TTInstrObj Get Properties.
			Array data = (Array) pInstr.get_Get("Bid$,Ask$,Last$,Change$");
		
			// Populate the user interface textboxes.
			txtBidPrice.Text = (string)data.GetValue(0);	
			txtAskPrice.Text = (string)data.GetValue(1);
			txtLastPrice.Text = (string)data.GetValue(2);
			txtChange.Text = (string)data.GetValue(3);
		}

        /// <summary>
        /// This method is called when the user clicks the buy button.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
		private void BuyButton_Click(object sender, System.EventArgs e)
		{
			// Call the SendOrder function with a Buy request.
			SendOrder("Buy");
		}

        /// <summary>
        /// This method is called when the user clicks the sell button.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
		private void SellButton_Click(object sender, System.EventArgs e)
		{
			// Call the SendOrder function with a Sell request.
			SendOrder("Sell");
		}

        private void SendOrder(string buySell, TTInstrObj m_TTInstrObj2, int qty)
        {
            try
            {
                XTAPI.TTOrderProfileClass orderProfile = new XTAPI.TTOrderProfileClass();

                // Set the TTInstrObj to the TTOrderProfile.
                orderProfile.Instrument = m_TTInstrObj2;
                // Set the customer default property (e.g. "<Default>").
                orderProfile.Customer = "<Default>";//cboCustomer.SelectedItem.ToString();
                // Set for Buy or Sell.
                orderProfile.Set("BuySell", buySell);
                // Set the quantity.
                orderProfile.Set("Qty", qty.ToString());
                // Set the order type to "L" for a limit order.
                orderProfile.Set("OrderType", "M");
                // Set the limit order price.
                //orderProfile.Set("Limit$", txtPrice.Text.ToString());
                orderProfile.Set("OrderTag", "user input text");

                // Send the order by submitting the TTOrderProfile through the TTOrderSet.
                int submittedQuantity = m_TTOrderSet.get_SendOrder(orderProfile);

                // Obtain the SiteOrderKey for the last order submitted.
                //m_LastOrderSiteOrderKey = (string)orderProfile.get_GetLast("SiteOrderKey");

                // Print the SiteOrderKey to the user interface.
                //txtSiteOrderKey.Text = m_LastOrderSiteOrderKey;

                if (submittedQuantity != 0)
                {
                    MessageBox.Show("Order Submitted, never gets called :-(");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message, "AUTORISK Exception");
            }
        }

        /// <summary>
        /// This function sets up the TTOrderProfile and submits the order using the 
        /// TTOrderSet SendOrder method.
        /// </summary>
        /// <param name="buySell">The side of the market to place the order on.</param>
		private void SendOrder(string buySell)
		{
			// Set the TTInstrObj to the TTOrderProfile.
			XTAPI.TTOrderProfileClass orderProfile = new XTAPI.TTOrderProfileClass();
            string printInfo = "\r\n" + m_TTInstrObj.Exchange +
                                "\r\n" + m_TTInstrObj.GatewayType +
                                "\r\n";
			orderProfile.Instrument = m_TTInstrObj;
            //Set user defined tag on profile
            orderProfile.Set("UserTag","Some Tag");
			try
			{
				// Set the customer default property (e.g. "<Default>").
				orderProfile.Customer = cboCustomer.SelectedItem.ToString();  
				
				// Set for Buy or Sell.
				orderProfile.Set("BuySell",buySell);	

				// Set the quantity.
				orderProfile.Set("Qty",txtQuantity.Text.ToString());
			
				// Determine which Order Type is selected.
				if( cboOrderType.SelectedIndex == 0 )  // Market Order
				{
					// Set the order type to "M" for a market order.
					orderProfile.Set("OrderType","M");
				}
				else if( cboOrderType.SelectedIndex == 1 )  // Limit Order
				{
					// Set the order type to "L" for a limit order.
					orderProfile.Set("OrderType","L");	
					// Set the limit order price.
					orderProfile.Set("Limit$",txtPrice.Text.ToString());
				}
				else if( cboOrderType.SelectedIndex == 2 )  // Stop Market Order
				{
					// Set the order type to "M" for a market order.
					orderProfile.Set("OrderType","M");
					// Set the order restriction to "S" for a stop order.
					orderProfile.Set("OrderRestr","S");				
					// Set the stop price.
					orderProfile.Set("Stop$",txtStopPrice.Text.ToString());	
				}
				else if( cboOrderType.SelectedIndex == 3 )  // Stop Limit Order
				{
					// Set the order type to "L" for a limit order.
					orderProfile.Set("OrderType","L");
					// Set the order restriction to "S" for a stop order.
					orderProfile.Set("OrderRestr","S");		
					// Set the limit price.
					orderProfile.Set("Limit$",txtPrice.Text.ToString());
					// Set the stop price.
					orderProfile.Set("Stop$",txtStopPrice.Text.ToString());	
				}
				
				// Send the order by submitting the TTOrderProfile through the TTOrderSet.
				int submittedQty = m_TTOrderSet.get_SendOrder(orderProfile);
				
				// Display the submitted quantity in the user interface.
                txtOrderBook.Text += "Quantity Sent: " + submittedQty.ToString() + printInfo;
			}
			catch (Exception ex)
			{
			    MessageBox.Show(ex.Message + printInfo, "Exception");
			}
		}

        /// <summary>
        /// This function enables and disables the appropriate text boxes on the user interface.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
		private void orderTypeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if( cboOrderType.SelectedIndex == 2 || cboOrderType.SelectedIndex == 3)
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

			if( cboOrderType.SelectedIndex == 0 || cboOrderType.SelectedIndex == 2)
			{
				// Clear and disable the price text box if the selected order type is market or stop market.
				txtPrice.Clear();
				txtPrice.Enabled = false;
			}
			else
			{
				// Enable the price text box if the if the selected order type is limit or stop limit.
				txtPrice.Enabled = true;
			}
		}

        /// <summary>
        /// Display the About dialog box.
        /// </summary>
        /// <param name="sender">Object which fires the method</param>
        /// <param name="e">Event arguments of the callback</param>
		private void AboutMenuItem_Click(object sender, System.EventArgs e)
		{
			About aboutForm = new About();
			aboutForm.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Array returnData = (Array)instr.get_Get("buyTotalQty"); 

            int netpos = (int)m_TTInstrObj.OrderSet.get_Get("NetPos");

            int[] val = ((object[])m_TTInstrObj.get_Get("Ask&,Bid&")).Cast<int>().ToArray();
            
            XTAPI.TTOrderProfile profile = new XTAPI.TTOrderProfile();
            profile.Instrument = m_TTInstrObj;
            System.Collections.Generic.Dictionary<string, object> profSettings = 
                new System.Collections.Generic.Dictionary<string, object>();
            int priceTicks = 0;
            profSettings["OrderType"] = "M";
            if (netpos < 0)
            {
                priceTicks = val[1];
                profSettings["BuySell"] = "B";
            }
            else
            {
                priceTicks = val[0];
                profSettings["BuySell"] = "S";
            }

            profSettings["Price&"] = priceTicks;
            profSettings["Qty"] = Math.Abs(netpos);
            profile.Set(string.Join(",", profSettings.Keys.ToArray()), profSettings.Values.ToArray());
            profile.Customer = "<Default>";
            if (m_TTOrderSet.get_SendOrder(profile) < 1)
            {
                //error 

                //DefaultLogTracer.WriteLine("Error while closing orders: " + instr.Contract, LogLevel.ERROR);
                MessageBox.Show("Error while closing orders");
            }
            else
            {
                //ok 
                string closeKey = (string)profile.get_GetLast("SiteKey");
            } 
        }
	}
}