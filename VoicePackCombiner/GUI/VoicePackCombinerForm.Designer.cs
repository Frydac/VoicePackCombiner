namespace RecursionTracker.Plugins.VoicePackCombiner.GUI
{
    partial class VoicePackCombinerForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addVoicePackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new System.Windows.Forms.Button();
            this.listBoxVoicePacks = new System.Windows.Forms.ListBox();
            this.checkBoxEnableCombinedVoicePack = new System.Windows.Forms.CheckBox();
            this.btnExportCombinedVoicePack = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnAddVoicePack = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(642, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addVoicePackToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addVoicePackToolStripMenuItem
            // 
            this.addVoicePackToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.addVoicePackToolStripMenuItem.Image = global::RecursionTracker.Plugins.VoicePackCombiner.Properties.Resources._077_AddFile_16x16_72;
            this.addVoicePackToolStripMenuItem.Name = "addVoicePackToolStripMenuItem";
            this.addVoicePackToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.addVoicePackToolStripMenuItem.Text = "Add Voice Packs...";
            this.addVoicePackToolStripMenuItem.Click += new System.EventHandler(this.btnAddVoicePack_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.LightGray;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItem1.Text = "Export Combined Voice Pack...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tutorialToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tutorialToolStripMenuItem
            // 
            this.tutorialToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.tutorialToolStripMenuItem.Name = "tutorialToolStripMenuItem";
            this.tutorialToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.tutorialToolStripMenuItem.Text = "Tutorial";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Gray;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(562, 468);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(65, 31);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // listBoxVoicePacks
            // 
            this.listBoxVoicePacks.AllowDrop = true;
            this.listBoxVoicePacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxVoicePacks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxVoicePacks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxVoicePacks.ForeColor = System.Drawing.Color.White;
            this.listBoxVoicePacks.FormattingEnabled = true;
            this.listBoxVoicePacks.IntegralHeight = false;
            this.listBoxVoicePacks.ItemHeight = 15;
            this.listBoxVoicePacks.Location = new System.Drawing.Point(14, 75);
            this.listBoxVoicePacks.Name = "listBoxVoicePacks";
            this.listBoxVoicePacks.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxVoicePacks.Size = new System.Drawing.Size(613, 386);
            this.listBoxVoicePacks.TabIndex = 3;
            // 
            // checkBoxEnableCombinedVoicePack
            // 
            this.checkBoxEnableCombinedVoicePack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEnableCombinedVoicePack.AutoSize = true;
            this.checkBoxEnableCombinedVoicePack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableCombinedVoicePack.ForeColor = System.Drawing.Color.White;
            this.checkBoxEnableCombinedVoicePack.Location = new System.Drawing.Point(14, 476);
            this.checkBoxEnableCombinedVoicePack.Name = "checkBoxEnableCombinedVoicePack";
            this.checkBoxEnableCombinedVoicePack.Size = new System.Drawing.Size(158, 19);
            this.checkBoxEnableCombinedVoicePack.TabIndex = 6;
            this.checkBoxEnableCombinedVoicePack.Text = "Use combined VoicePack";
            this.checkBoxEnableCombinedVoicePack.UseVisualStyleBackColor = true;
            this.checkBoxEnableCombinedVoicePack.CheckedChanged += new System.EventHandler(this.checkBoxEnableCombinedVoicePack_CheckedChanged);
            // 
            // btnExportCombinedVoicePack
            // 
            this.btnExportCombinedVoicePack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportCombinedVoicePack.AutoSize = true;
            this.btnExportCombinedVoicePack.BackColor = System.Drawing.Color.Gray;
            this.btnExportCombinedVoicePack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCombinedVoicePack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportCombinedVoicePack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportCombinedVoicePack.Location = new System.Drawing.Point(360, 468);
            this.btnExportCombinedVoicePack.Name = "btnExportCombinedVoicePack";
            this.btnExportCombinedVoicePack.Size = new System.Drawing.Size(196, 31);
            this.btnExportCombinedVoicePack.TabIndex = 7;
            this.btnExportCombinedVoicePack.Text = "Export Combined Voice Pack...";
            this.btnExportCombinedVoicePack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExportCombinedVoicePack.UseVisualStyleBackColor = false;
            this.btnExportCombinedVoicePack.Click += new System.EventHandler(this.btnExportCombinedVoicePack_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.AutoSize = true;
            this.btnRemoveSelected.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSelected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveSelected.Image = global::RecursionTracker.Plugins.VoicePackCombiner.Properties.Resources.action_Cancel_16xMD;
            this.btnRemoveSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveSelected.Location = new System.Drawing.Point(151, 37);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(125, 31);
            this.btnRemoveSelected.TabIndex = 4;
            this.btnRemoveSelected.Text = "Remove Selected";
            this.btnRemoveSelected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveSelected.UseVisualStyleBackColor = false;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnAddVoicePack
            // 
            this.btnAddVoicePack.AutoSize = true;
            this.btnAddVoicePack.BackColor = System.Drawing.Color.Gray;
            this.btnAddVoicePack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddVoicePack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddVoicePack.Image = global::RecursionTracker.Plugins.VoicePackCombiner.Properties.Resources._077_AddFile_16x16_72;
            this.btnAddVoicePack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddVoicePack.Location = new System.Drawing.Point(14, 37);
            this.btnAddVoicePack.Name = "btnAddVoicePack";
            this.btnAddVoicePack.Size = new System.Drawing.Size(131, 31);
            this.btnAddVoicePack.TabIndex = 1;
            this.btnAddVoicePack.Text = "Add Voice Packs...";
            this.btnAddVoicePack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddVoicePack.UseVisualStyleBackColor = false;
            this.btnAddVoicePack.Click += new System.EventHandler(this.btnAddVoicePack_Click);
            // 
            // VoicePackCombinerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 509);
            this.Controls.Add(this.btnExportCombinedVoicePack);
            this.Controls.Add(this.checkBoxEnableCombinedVoicePack);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.listBoxVoicePacks);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddVoicePack);
            this.Controls.Add(this.menuStrip1);
            this.DropShadow = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VoicePackCombinerForm";
            this.Text = "Voice Pack Combiner";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addVoicePackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tutorialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btnAddVoicePack;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox listBoxVoicePacks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.CheckBox checkBoxEnableCombinedVoicePack;
        private System.Windows.Forms.Button btnExportCombinedVoicePack;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

    }
}