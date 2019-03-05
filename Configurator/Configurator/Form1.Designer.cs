namespace Configurator
{
    partial class Configurator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configurator));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.confServer = new System.Windows.Forms.Button();
            this.confScan = new System.Windows.Forms.Button();
            this.confGrid = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.servbtn = new System.Windows.Forms.Button();
            this.scannbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(561, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 47);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scanner";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(158, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 47);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server";
            // 
            // confServer
            // 
            this.confServer.Location = new System.Drawing.Point(112, 369);
            this.confServer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.confServer.Name = "confServer";
            this.confServer.Size = new System.Drawing.Size(240, 45);
            this.confServer.TabIndex = 4;
            this.confServer.Text = "Configure Server";
            this.confServer.UseVisualStyleBackColor = true;
            this.confServer.Visible = false;
            this.confServer.Click += new System.EventHandler(this.confServer_Click);
            // 
            // confScan
            // 
            this.confScan.Location = new System.Drawing.Point(534, 369);
            this.confScan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.confScan.Name = "confScan";
            this.confScan.Size = new System.Drawing.Size(240, 45);
            this.confScan.TabIndex = 5;
            this.confScan.Text = "Configure Scanner";
            this.confScan.UseVisualStyleBackColor = true;
            this.confScan.Visible = false;
            this.confScan.Click += new System.EventHandler(this.confScan_Click);
            // 
            // confGrid
            // 
            this.confGrid.Location = new System.Drawing.Point(942, 369);
            this.confGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.confGrid.Name = "confGrid";
            this.confGrid.Size = new System.Drawing.Size(240, 45);
            this.confGrid.TabIndex = 8;
            this.confGrid.Text = "Configure Chutes";
            this.confGrid.UseVisualStyleBackColor = true;
            this.confGrid.Visible = false;
            this.confGrid.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(988, 42);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 47);
            this.label3.TabIndex = 7;
            this.label3.Text = "Grid";
            // 
            // button2
            // 
            this.button2.Image = global::Configurator.Properties.Resources.grid;
            this.button2.Location = new System.Drawing.Point(942, 111);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(240, 232);
            this.button2.TabIndex = 6;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // servbtn
            // 
            this.servbtn.Image = global::Configurator.Properties.Resources.server__1_;
            this.servbtn.Location = new System.Drawing.Point(112, 111);
            this.servbtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.servbtn.Name = "servbtn";
            this.servbtn.Size = new System.Drawing.Size(240, 232);
            this.servbtn.TabIndex = 1;
            this.servbtn.UseVisualStyleBackColor = true;
            this.servbtn.Click += new System.EventHandler(this.servbtn_Click);
            // 
            // scannbtn
            // 
            this.scannbtn.Image = global::Configurator.Properties.Resources.scanner2;
            this.scannbtn.Location = new System.Drawing.Point(534, 111);
            this.scannbtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.scannbtn.Name = "scannbtn";
            this.scannbtn.Size = new System.Drawing.Size(240, 232);
            this.scannbtn.TabIndex = 0;
            this.scannbtn.UseVisualStyleBackColor = true;
            this.scannbtn.Click += new System.EventHandler(this.scannbtn_Click);
            // 
            // Configurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1319, 462);
            this.Controls.Add(this.confGrid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.confScan);
            this.Controls.Add(this.confServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.servbtn);
            this.Controls.Add(this.scannbtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Configurator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Config";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button scannbtn;
        private System.Windows.Forms.Button servbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button confServer;
        private System.Windows.Forms.Button confScan;
        private System.Windows.Forms.Button confGrid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
    }
}

