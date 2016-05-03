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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace XTAPI_Samples
{

	/// <summary>
	/// OrderSelector
    /// 
    /// This example demonstrates using the XTAPI to filter the order updates using the
    /// TTOrderSelector object.  
    /// 
    /// Note:	Separate TTOrderSet objects are created for submitting orders and
    /// 		receiving order updates.  This is the recommended practice, as orders 
    /// 		sent on a filtered TTOrderSet must match the filter, or will be 
    /// 		rejected.
	/// </summary>
	public class frmPositionGrouper : Form
    {
        // Declare the XTAPI objects.
		private XTAPI.TTOrderSetClass orderBookSet = null;
		private XTAPI.TTOrderSelector orderBookSelector = null;
        private List<XTAPI.TTOrderSetClass> subsetList = new List<XTAPI.TTOrderSetClass>();

        //Keep Track of OrderSets that come in by instrument
        private SortedDictionary<String, XTAPI.TTOrderSetClass> inst_ordersMap =
            new SortedDictionary<String, XTAPI.TTOrderSetClass>();

        // Windows Form Objects
        private GroupBox groupBox1;
        private ListBox InstrList;
        private GroupBox groupBox2;
        private Button button2;
        private Button button1;
        private TreeView treeView1;

        /// <summary>
        /// Upon the application form loading, the TTDropHandler and TTInstrNotify 
        /// objects are initialized, and the required events are subscribed.
        /// </summary>
        public frmPositionGrouper()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

            downloadOrderBook();
            
		}

        private void downloadOrderBook()
        {
            orderBookSelector = new XTAPI.TTOrderSelector();

            // Filter orders placed through X_TRADER.
            orderBookSelector.AddTest("OrderSource", "0");

            // Filter orders placed through XTAPI.
            orderBookSelector.AddTest("OrderSource", "3");

            // Filter sell orders.
            orderBookSelector.AddTest("IsSell", "True");

            // Filter buy orders.
            orderBookSelector.AddTest("IsBuy", "True");

            // OR all filters
            orderBookSelector.AllMatchesRequired = 0;
            orderBookSelector.AllowAnyMatches = 1;

            // Instantiate a TTOrderSet object for receiving orders.
            orderBookSet = new XTAPI.TTOrderSetClass();
            orderBookSet.OnOrderSetUpdate += new XTAPI._ITTOrderSetEvents_OnOrderSetUpdateEventHandler(orderBookDownloadHandler);

            // Enable the TTOrderTracker and order updates.
            orderBookSet.EnableOrderUpdateData = 1;
            orderBookSet.EnableOrderSetUpdates = 1;

            orderBookSet.OrderSelector = orderBookSelector;
            orderBookSet.Open(0);
        }

        private void orderBookDownloadHandler(XTAPI.TTOrderSet pOrderSet)
        {
            // Obtain the Next TTOrderTrackerObj object from the TTOrderSet.
            XTAPI.TTOrderTrackerObj tempOrderTrackerObj = pOrderSet.NextOrderTracker;
            List<string> updateInstList = new List<string>();

            while(tempOrderTrackerObj != null)
            {
                // Test if an Old Order (past state) exists.
                if (tempOrderTrackerObj.HasOldOrder != 0)
                {
                    // Obtain the TTOrderObj from the TTOrderTrackerObj.
                    XTAPI.TTOrderObj tempOldOrder = tempOrderTrackerObj.OldOrder;
                    string instName = (string)tempOldOrder.get_Get("Instr");
                    updateInstList.Add((string)instName);
                }

                // Test if an New Order (current state) exists.
                if (tempOrderTrackerObj.HasNewOrder != 0)
                {
                    // Obtain the TTOrderObj from the TTOrderTrackerObj.
                    XTAPI.TTOrderObj tempNewOrder = tempOrderTrackerObj.NewOrder;
                    string instName = (string)tempNewOrder.get_Get("Instr");

                    if(!inst_ordersMap.ContainsKey(instName))
                    {
                        inst_ordersMap.Add(instName, new XTAPI.TTOrderSetClass());
                        updateInstList.Add(instName);
                    }

                }

                tempOrderTrackerObj = pOrderSet.NextOrderTracker;
            }

            // Refresh all instrument OrderSets and Groups
            updateInstrumentAndGroupOrderSets(updateInstList);
        }

        private void updateInstrumentAndGroupOrderSets(List<string> updateInstList)
        {
            foreach (string instr in updateInstList)
            {
                if(!inst_ordersMap.ContainsKey(instr))
                {
                    inst_ordersMap.Add(instr, new XTAPI.TTOrderSetClass());
                }
                XTAPI.TTOrderSetClass instOrderSet = null;
                inst_ordersMap.TryGetValue(instr, out instOrderSet);
                instOrderSet.OnOrderSetUpdate += new XTAPI._ITTOrderSetEvents_OnOrderSetUpdateEventHandler(instrOrderSetUpdate);

                XTAPI.TTOrderSelector selectorSecondCheck = instOrderSet.CreateOrderSelector;
                XTAPI.TTOrderSelector selectorFirstCheck = instOrderSet.CreateOrderSelector;

                // Filter orders placed through X_TRADER.
                selectorFirstCheck.AddTest("OrderSource", "0");

                // Filter orders placed through XTAPI.
                selectorFirstCheck.AddTest("OrderSource", "3");
                // OR Filter
                selectorFirstCheck.AllMatchesRequired = 0;
                selectorFirstCheck.AllowAnyMatches = 1;

                //Find instrument set
                selectorSecondCheck.AddTest("Instr", instr);
                // Not needed - but AND Filter
                selectorSecondCheck.AllMatchesRequired = 1;
                selectorSecondCheck.AllowAnyMatches = 0;

                //Combine Filters
                selectorSecondCheck.AddSelector(selectorFirstCheck);

                instOrderSet.OrderSelector = selectorSecondCheck;

                inst_ordersMap.Remove(instr);
                inst_ordersMap.Add(instr,instOrderSet);

                instOrderSet.Open(0);
            }
        }

        private void instrOrderSetUpdate(XTAPI.TTOrderSet pOrderSet)
        {
            SortedDictionary<string, XTAPI.TTOrderSetClass>.KeyCollection.Enumerator enumerator =
                inst_ordersMap.Keys.GetEnumerator();
            InstrList.Items.Clear();
            while(enumerator.MoveNext())
            {
                string output = "";
                string key = enumerator.Current;
                XTAPI.TTOrderSetClass set = null;
                inst_ordersMap.TryGetValue(key, out set);
                Array info = (Array)set.get_Get("BuyPos,BuyOPos,SellPos,SellOPos,NetPos,NetOPos");
                output += key + ";";
                output += " orders working: " + set.Count + ";";
                output += " BuyPos: " + info.GetValue(0) + ";";
                output += " BuyOPos: " + info.GetValue(1) + ";";
                output += " SellPos: " + info.GetValue(2) + ";";
                output += " SellOPos: " + info.GetValue(3) + ";";
                InstrList.Items.Add(output);
                output = " NetPos: " + info.GetValue(4) + ";";
                output += " NetOPos: " + info.GetValue(5) + ";";
                Console.WriteLine(output);
                InstrList.Items.Add(output);
            }

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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.InstrList = new System.Windows.Forms.ListBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // sbaStatus
            // 
            this.sbaStatus.Location = new System.Drawing.Point(0, 906);
            this.sbaStatus.Name = "sbaStatus";
            this.sbaStatus.Size = new System.Drawing.Size(978, 41);
            this.sbaStatus.SizingGrip = false;
            this.sbaStatus.TabIndex = 63;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.InstrList);
            this.groupBox1.Location = new System.Drawing.Point(24, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(942, 401);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Intruments";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.treeView1);
            this.groupBox2.Location = new System.Drawing.Point(24, 481);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(942, 401);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Groups";
            // 
            // InstrList
            // 
            this.InstrList.FormattingEnabled = true;
            this.InstrList.ItemHeight = 25;
            this.InstrList.Location = new System.Drawing.Point(148, 53);
            this.InstrList.Name = "InstrList";
            this.InstrList.Size = new System.Drawing.Size(769, 329);
            this.InstrList.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(148, 53);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(769, 319);
            this.treeView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 74);
            this.button1.TabIndex = 1;
            this.button1.Text = "Create Group";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 298);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 74);
            this.button2.TabIndex = 2;
            this.button2.Text = "Remove Group";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // frmOrderSelector
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 24);
            this.ClientSize = new System.Drawing.Size(978, 947);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sbaStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Menu = this.mainMenu1;
            this.Name = "frmOrderSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PositionGrouper";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
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
			Application.Run(new frmPositionGrouper());
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
	}
}
