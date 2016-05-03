namespace TTAPI_Samples
{
    partial class frmPriceUpdateManual
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusBar1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblNotProduction = new System.Windows.Forms.Label();
            this.lblAbout = new System.Windows.Forms.Label();
            this.gboInstrumentMarketData = new System.Windows.Forms.GroupBox();
            this.lblAskPrice = new System.Windows.Forms.Label();
            this.txtAskPrice = new System.Windows.Forms.TextBox();
            this.txtBidPrice = new System.Windows.Forms.TextBox();
            this.lblLastQty = new System.Windows.Forms.Label();
            this.txtLastQty = new System.Windows.Forms.TextBox();
            this.lblBidPrice = new System.Windows.Forms.Label();
            this.lblAskQty = new System.Windows.Forms.Label();
            this.txtAskQty = new System.Windows.Forms.TextBox();
            this.lblLastPrice = new System.Windows.Forms.Label();
            this.lblBidQty = new System.Windows.Forms.Label();
            this.txtBidQty = new System.Windows.Forms.TextBox();
            this.txtLastPrice = new System.Windows.Forms.TextBox();
            this.gboInstrumentInfo = new System.Windows.Forms.GroupBox();
            this.cboProductType = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblProductType = new System.Windows.Forms.Label();
            this.txtProduct = new System.Windows.Forms.TextBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblExchange = new System.Windows.Forms.Label();
            this.txtContract = new System.Windows.Forms.TextBox();
            this.lblContract = new System.Windows.Forms.Label();
            this.txtExchange = new System.Windows.Forms.TextBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAskPrice2 = new System.Windows.Forms.TextBox();
            this.txtBidPrice2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastQty2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAskQty2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBidQty2 = new System.Windows.Forms.TextBox();
            this.txtLastPrice2 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboProductType2 = new System.Windows.Forms.ComboBox();
            this.btnConnect2 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProduct2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtContract2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtExchange2 = new System.Windows.Forms.TextBox();
            this.statusBar1.SuspendLayout();
            this.gboInstrumentMarketData.SuspendLayout();
            this.gboInstrumentInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar1
            // 
            this.statusBar1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusBar1.Location = new System.Drawing.Point(0, 621);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusBar1.Size = new System.Drawing.Size(637, 37);
            this.statusBar1.TabIndex = 43;
            this.statusBar1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(738, 32);
            this.toolStripStatusLabel1.Text = "Complete the Instrument Information and click the Connect button.";
            // 
            // lblNotProduction
            // 
            this.lblNotProduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotProduction.Location = new System.Drawing.Point(16, 42);
            this.lblNotProduction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNotProduction.Name = "lblNotProduction";
            this.lblNotProduction.Size = new System.Drawing.Size(565, 17);
            this.lblNotProduction.TabIndex = 82;
            this.lblNotProduction.Text = "This sample is NOT to be used in production or during conformance testing.";
            this.lblNotProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAbout
            // 
            this.lblAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbout.Location = new System.Drawing.Point(16, 11);
            this.lblAbout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(565, 28);
            this.lblAbout.TabIndex = 81;
            this.lblAbout.Text = "WARNING!";
            this.lblAbout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gboInstrumentMarketData
            // 
            this.gboInstrumentMarketData.Controls.Add(this.lblAskPrice);
            this.gboInstrumentMarketData.Controls.Add(this.txtAskPrice);
            this.gboInstrumentMarketData.Controls.Add(this.txtBidPrice);
            this.gboInstrumentMarketData.Controls.Add(this.lblLastQty);
            this.gboInstrumentMarketData.Controls.Add(this.txtLastQty);
            this.gboInstrumentMarketData.Controls.Add(this.lblBidPrice);
            this.gboInstrumentMarketData.Controls.Add(this.lblAskQty);
            this.gboInstrumentMarketData.Controls.Add(this.txtAskQty);
            this.gboInstrumentMarketData.Controls.Add(this.lblLastPrice);
            this.gboInstrumentMarketData.Controls.Add(this.lblBidQty);
            this.gboInstrumentMarketData.Controls.Add(this.txtBidQty);
            this.gboInstrumentMarketData.Controls.Add(this.txtLastPrice);
            this.gboInstrumentMarketData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gboInstrumentMarketData.Location = new System.Drawing.Point(307, 68);
            this.gboInstrumentMarketData.Margin = new System.Windows.Forms.Padding(4);
            this.gboInstrumentMarketData.Name = "gboInstrumentMarketData";
            this.gboInstrumentMarketData.Padding = new System.Windows.Forms.Padding(4);
            this.gboInstrumentMarketData.Size = new System.Drawing.Size(277, 220);
            this.gboInstrumentMarketData.TabIndex = 80;
            this.gboInstrumentMarketData.TabStop = false;
            this.gboInstrumentMarketData.Text = "Instrument Market Data";
            // 
            // lblAskPrice
            // 
            this.lblAskPrice.Location = new System.Drawing.Point(21, 89);
            this.lblAskPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAskPrice.Name = "lblAskPrice";
            this.lblAskPrice.Size = new System.Drawing.Size(108, 20);
            this.lblAskPrice.TabIndex = 46;
            this.lblAskPrice.Text = "Ask Price:";
            this.lblAskPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAskPrice
            // 
            this.txtAskPrice.Location = new System.Drawing.Point(137, 89);
            this.txtAskPrice.Margin = new System.Windows.Forms.Padding(4);
            this.txtAskPrice.Name = "txtAskPrice";
            this.txtAskPrice.Size = new System.Drawing.Size(117, 22);
            this.txtAskPrice.TabIndex = 45;
            // 
            // txtBidPrice
            // 
            this.txtBidPrice.Location = new System.Drawing.Point(137, 30);
            this.txtBidPrice.Margin = new System.Windows.Forms.Padding(4);
            this.txtBidPrice.Name = "txtBidPrice";
            this.txtBidPrice.Size = new System.Drawing.Size(117, 22);
            this.txtBidPrice.TabIndex = 41;
            // 
            // lblLastQty
            // 
            this.lblLastQty.Location = new System.Drawing.Point(21, 177);
            this.lblLastQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastQty.Name = "lblLastQty";
            this.lblLastQty.Size = new System.Drawing.Size(108, 20);
            this.lblLastQty.TabIndex = 52;
            this.lblLastQty.Text = "Last Qty:";
            this.lblLastQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastQty
            // 
            this.txtLastQty.Location = new System.Drawing.Point(137, 177);
            this.txtLastQty.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastQty.Name = "txtLastQty";
            this.txtLastQty.Size = new System.Drawing.Size(117, 22);
            this.txtLastQty.TabIndex = 51;
            // 
            // lblBidPrice
            // 
            this.lblBidPrice.Location = new System.Drawing.Point(21, 30);
            this.lblBidPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBidPrice.Name = "lblBidPrice";
            this.lblBidPrice.Size = new System.Drawing.Size(108, 20);
            this.lblBidPrice.TabIndex = 42;
            this.lblBidPrice.Text = "Bid Price:";
            this.lblBidPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAskQty
            // 
            this.lblAskQty.Location = new System.Drawing.Point(21, 118);
            this.lblAskQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAskQty.Name = "lblAskQty";
            this.lblAskQty.Size = new System.Drawing.Size(108, 20);
            this.lblAskQty.TabIndex = 48;
            this.lblAskQty.Text = "Ask Qty:";
            this.lblAskQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAskQty
            // 
            this.txtAskQty.Location = new System.Drawing.Point(137, 118);
            this.txtAskQty.Margin = new System.Windows.Forms.Padding(4);
            this.txtAskQty.Name = "txtAskQty";
            this.txtAskQty.Size = new System.Drawing.Size(117, 22);
            this.txtAskQty.TabIndex = 47;
            // 
            // lblLastPrice
            // 
            this.lblLastPrice.Location = new System.Drawing.Point(21, 148);
            this.lblLastPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastPrice.Name = "lblLastPrice";
            this.lblLastPrice.Size = new System.Drawing.Size(108, 20);
            this.lblLastPrice.TabIndex = 50;
            this.lblLastPrice.Text = "Last Price:";
            this.lblLastPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBidQty
            // 
            this.lblBidQty.Location = new System.Drawing.Point(21, 59);
            this.lblBidQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBidQty.Name = "lblBidQty";
            this.lblBidQty.Size = new System.Drawing.Size(108, 20);
            this.lblBidQty.TabIndex = 44;
            this.lblBidQty.Text = "Bid Qty:";
            this.lblBidQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBidQty
            // 
            this.txtBidQty.Location = new System.Drawing.Point(137, 59);
            this.txtBidQty.Margin = new System.Windows.Forms.Padding(4);
            this.txtBidQty.Name = "txtBidQty";
            this.txtBidQty.Size = new System.Drawing.Size(117, 22);
            this.txtBidQty.TabIndex = 43;
            // 
            // txtLastPrice
            // 
            this.txtLastPrice.Location = new System.Drawing.Point(137, 148);
            this.txtLastPrice.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastPrice.Name = "txtLastPrice";
            this.txtLastPrice.Size = new System.Drawing.Size(117, 22);
            this.txtLastPrice.TabIndex = 49;
            // 
            // gboInstrumentInfo
            // 
            this.gboInstrumentInfo.Controls.Add(this.cboProductType);
            this.gboInstrumentInfo.Controls.Add(this.btnConnect);
            this.gboInstrumentInfo.Controls.Add(this.lblProductType);
            this.gboInstrumentInfo.Controls.Add(this.txtProduct);
            this.gboInstrumentInfo.Controls.Add(this.lblProduct);
            this.gboInstrumentInfo.Controls.Add(this.lblExchange);
            this.gboInstrumentInfo.Controls.Add(this.txtContract);
            this.gboInstrumentInfo.Controls.Add(this.lblContract);
            this.gboInstrumentInfo.Controls.Add(this.txtExchange);
            this.gboInstrumentInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gboInstrumentInfo.Location = new System.Drawing.Point(11, 68);
            this.gboInstrumentInfo.Margin = new System.Windows.Forms.Padding(4);
            this.gboInstrumentInfo.Name = "gboInstrumentInfo";
            this.gboInstrumentInfo.Padding = new System.Windows.Forms.Padding(4);
            this.gboInstrumentInfo.Size = new System.Drawing.Size(288, 220);
            this.gboInstrumentInfo.TabIndex = 79;
            this.gboInstrumentInfo.TabStop = false;
            this.gboInstrumentInfo.Text = "Instrument Information";
            // 
            // cboProductType
            // 
            this.cboProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProductType.FormattingEnabled = true;
            this.cboProductType.Location = new System.Drawing.Point(128, 87);
            this.cboProductType.Margin = new System.Windows.Forms.Padding(4);
            this.cboProductType.Name = "cboProductType";
            this.cboProductType.Size = new System.Drawing.Size(151, 34);
            this.cboProductType.TabIndex = 42;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.SystemColors.Control;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConnect.Location = new System.Drawing.Point(180, 169);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 28);
            this.btnConnect.TabIndex = 41;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btn1Connect_Click);
            // 
            // lblProductType
            // 
            this.lblProductType.Location = new System.Drawing.Point(11, 89);
            this.lblProductType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(107, 20);
            this.lblProductType.TabIndex = 38;
            this.lblProductType.Text = "Product Type:";
            this.lblProductType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(128, 59);
            this.txtProduct.Margin = new System.Windows.Forms.Padding(4);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Size = new System.Drawing.Size(151, 22);
            this.txtProduct.TabIndex = 35;
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(53, 59);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(64, 20);
            this.lblProduct.TabIndex = 36;
            this.lblProduct.Text = "Product:";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblExchange
            // 
            this.lblExchange.Location = new System.Drawing.Point(32, 30);
            this.lblExchange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExchange.Name = "lblExchange";
            this.lblExchange.Size = new System.Drawing.Size(85, 20);
            this.lblExchange.TabIndex = 34;
            this.lblExchange.Text = "Exchange:";
            this.lblExchange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContract
            // 
            this.txtContract.Location = new System.Drawing.Point(128, 118);
            this.txtContract.Margin = new System.Windows.Forms.Padding(4);
            this.txtContract.Name = "txtContract";
            this.txtContract.Size = new System.Drawing.Size(151, 22);
            this.txtContract.TabIndex = 39;
            // 
            // lblContract
            // 
            this.lblContract.Location = new System.Drawing.Point(43, 118);
            this.lblContract.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContract.Name = "lblContract";
            this.lblContract.Size = new System.Drawing.Size(75, 20);
            this.lblContract.TabIndex = 40;
            this.lblContract.Text = "Contract:";
            this.lblContract.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExchange
            // 
            this.txtExchange.Location = new System.Drawing.Point(128, 30);
            this.txtExchange.Margin = new System.Windows.Forms.Padding(4);
            this.txtExchange.Name = "txtExchange";
            this.txtExchange.Size = new System.Drawing.Size(151, 22);
            this.txtExchange.TabIndex = 33;
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
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtAskPrice2);
            this.groupBox1.Controls.Add(this.txtBidPrice2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtLastQty2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtAskQty2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtBidQty2);
            this.groupBox1.Controls.Add(this.txtLastPrice2);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(307, 296);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(277, 220);
            this.groupBox1.TabIndex = 81;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Instrument Market Data";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 20);
            this.label1.TabIndex = 46;
            this.label1.Text = "Ask Price:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAskPrice2
            // 
            this.txtAskPrice2.Location = new System.Drawing.Point(137, 89);
            this.txtAskPrice2.Margin = new System.Windows.Forms.Padding(4);
            this.txtAskPrice2.Name = "txtAskPrice2";
            this.txtAskPrice2.Size = new System.Drawing.Size(117, 22);
            this.txtAskPrice2.TabIndex = 45;
            // 
            // txtBidPrice2
            // 
            this.txtBidPrice2.Location = new System.Drawing.Point(137, 30);
            this.txtBidPrice2.Margin = new System.Windows.Forms.Padding(4);
            this.txtBidPrice2.Name = "txtBidPrice2";
            this.txtBidPrice2.Size = new System.Drawing.Size(117, 22);
            this.txtBidPrice2.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 177);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 20);
            this.label2.TabIndex = 52;
            this.label2.Text = "Last Qty:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastQty2
            // 
            this.txtLastQty2.Location = new System.Drawing.Point(137, 177);
            this.txtLastQty2.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastQty2.Name = "txtLastQty2";
            this.txtLastQty2.Size = new System.Drawing.Size(117, 22);
            this.txtLastQty2.TabIndex = 51;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(21, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 20);
            this.label3.TabIndex = 42;
            this.label3.Text = "Bid Price:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(21, 118);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 20);
            this.label4.TabIndex = 48;
            this.label4.Text = "Ask Qty:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAskQty2
            // 
            this.txtAskQty2.Location = new System.Drawing.Point(137, 118);
            this.txtAskQty2.Margin = new System.Windows.Forms.Padding(4);
            this.txtAskQty2.Name = "txtAskQty2";
            this.txtAskQty2.Size = new System.Drawing.Size(117, 22);
            this.txtAskQty2.TabIndex = 47;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(21, 148);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 20);
            this.label5.TabIndex = 50;
            this.label5.Text = "Last Price:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(21, 59);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 20);
            this.label6.TabIndex = 44;
            this.label6.Text = "Bid Qty:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBidQty2
            // 
            this.txtBidQty2.Location = new System.Drawing.Point(137, 59);
            this.txtBidQty2.Margin = new System.Windows.Forms.Padding(4);
            this.txtBidQty2.Name = "txtBidQty2";
            this.txtBidQty2.Size = new System.Drawing.Size(117, 22);
            this.txtBidQty2.TabIndex = 43;
            // 
            // txtLastPrice2
            // 
            this.txtLastPrice2.Location = new System.Drawing.Point(137, 148);
            this.txtLastPrice2.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastPrice2.Name = "txtLastPrice2";
            this.txtLastPrice2.Size = new System.Drawing.Size(117, 22);
            this.txtLastPrice2.TabIndex = 49;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboProductType2);
            this.groupBox2.Controls.Add(this.btnConnect2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtProduct2);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtContract2);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtExchange2);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(13, 296);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(288, 220);
            this.groupBox2.TabIndex = 80;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Instrument Information";
            // 
            // cboProductType2
            // 
            this.cboProductType2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductType2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProductType2.FormattingEnabled = true;
            this.cboProductType2.Location = new System.Drawing.Point(128, 87);
            this.cboProductType2.Margin = new System.Windows.Forms.Padding(4);
            this.cboProductType2.Name = "cboProductType2";
            this.cboProductType2.Size = new System.Drawing.Size(151, 34);
            this.cboProductType2.TabIndex = 42;
            // 
            // btnConnect2
            // 
            this.btnConnect2.BackColor = System.Drawing.SystemColors.Control;
            this.btnConnect2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConnect2.Location = new System.Drawing.Point(180, 169);
            this.btnConnect2.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect2.Name = "btnConnect2";
            this.btnConnect2.Size = new System.Drawing.Size(100, 28);
            this.btnConnect2.TabIndex = 41;
            this.btnConnect2.Text = "Connect";
            this.btnConnect2.UseVisualStyleBackColor = false;
            this.btnConnect2.Click += new System.EventHandler(this.btn2Connect_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(11, 89);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 20);
            this.label7.TabIndex = 38;
            this.label7.Text = "Product Type:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProduct2
            // 
            this.txtProduct2.Location = new System.Drawing.Point(128, 59);
            this.txtProduct2.Margin = new System.Windows.Forms.Padding(4);
            this.txtProduct2.Name = "txtProduct2";
            this.txtProduct2.Size = new System.Drawing.Size(151, 22);
            this.txtProduct2.TabIndex = 35;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(53, 59);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 20);
            this.label8.TabIndex = 36;
            this.label8.Text = "Product:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(32, 30);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 20);
            this.label9.TabIndex = 34;
            this.label9.Text = "Exchange:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContract2
            // 
            this.txtContract2.Location = new System.Drawing.Point(128, 118);
            this.txtContract2.Margin = new System.Windows.Forms.Padding(4);
            this.txtContract2.Name = "txtContract2";
            this.txtContract2.Size = new System.Drawing.Size(151, 22);
            this.txtContract2.TabIndex = 39;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(43, 118);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 20);
            this.label10.TabIndex = 40;
            this.label10.Text = "Contract:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExchange2
            // 
            this.txtExchange2.Location = new System.Drawing.Point(128, 30);
            this.txtExchange2.Margin = new System.Windows.Forms.Padding(4);
            this.txtExchange2.Name = "txtExchange2";
            this.txtExchange2.Size = new System.Drawing.Size(151, 22);
            this.txtExchange2.TabIndex = 33;
            // 
            // frmPriceUpdateManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 658);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblNotProduction);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.gboInstrumentMarketData);
            this.Controls.Add(this.gboInstrumentInfo);
            this.Controls.Add(this.statusBar1);
            this.Enabled = false;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Menu = this.mainMenu1;
            this.Name = "frmPriceUpdateManual";
            this.Text = "PriceUpdateManual";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusBar1.ResumeLayout(false);
            this.statusBar1.PerformLayout();
            this.gboInstrumentMarketData.ResumeLayout(false);
            this.gboInstrumentMarketData.PerformLayout();
            this.gboInstrumentInfo.ResumeLayout(false);
            this.gboInstrumentInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblNotProduction;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.GroupBox gboInstrumentMarketData;
        private System.Windows.Forms.Label lblAskPrice;
        private System.Windows.Forms.TextBox txtAskPrice;
        private System.Windows.Forms.TextBox txtBidPrice;
        private System.Windows.Forms.Label lblLastQty;
        private System.Windows.Forms.TextBox txtLastQty;
        private System.Windows.Forms.Label lblBidPrice;
        private System.Windows.Forms.Label lblAskQty;
        private System.Windows.Forms.TextBox txtAskQty;
        private System.Windows.Forms.Label lblLastPrice;
        private System.Windows.Forms.Label lblBidQty;
        private System.Windows.Forms.TextBox txtBidQty;
        private System.Windows.Forms.TextBox txtLastPrice;
        private System.Windows.Forms.GroupBox gboInstrumentInfo;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.TextBox txtProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblExchange;
        private System.Windows.Forms.TextBox txtContract;
        private System.Windows.Forms.Label lblContract;
        private System.Windows.Forms.TextBox txtExchange;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.ComboBox cboProductType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAskPrice2;
        private System.Windows.Forms.TextBox txtBidPrice2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLastQty2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAskQty2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBidQty2;
        private System.Windows.Forms.TextBox txtLastPrice2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboProductType2;
        private System.Windows.Forms.Button btnConnect2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProduct2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtContract2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtExchange2;
    }
}

