namespace RecursionTracker.Plugins.VoicepackCombiner.GUI
{
    partial class VoicepackCombinerForm
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
            this.addVoicepackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new System.Windows.Forms.Button();
            this.listBoxVoicepacks = new System.Windows.Forms.ListBox();
            this.checkBoxEnableCombinedVoicepack = new System.Windows.Forms.CheckBox();
            this.btnExportCombinedVoicepack = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnAddVoicepack = new System.Windows.Forms.Button();
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
            this.addVoicepackToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addVoicepackToolStripMenuItem
            // 
            this.addVoicepackToolStripMenuItem.ForeColor = System.Drawing.Color.LightGray;
            this.addVoicepackToolStripMenuItem.Image = global::RecursionTracker.Plugins.VoicepackCombiner.Properties.Resources._077_AddFile_16x16_72;
            this.addVoicepackToolStripMenuItem.Name = "addVoicepackToolStripMenuItem";
            this.addVoicepackToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.addVoicepackToolStripMenuItem.Text = "Add Voicepacks...";
            this.addVoicepackToolStripMenuItem.Click += new System.EventHandler(this.btnAddVoicepack_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.LightGray;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItem1.Text = "Export Combined Voicepack...";
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
            // listBoxVoicepacks
            // 
            this.listBoxVoicepacks.AllowDrop = true;
            this.listBoxVoicepacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxVoicepacks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listBoxVoicepacks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxVoicepacks.ForeColor = System.Drawing.Color.White;
            this.listBoxVoicepacks.FormattingEnabled = true;
            this.listBoxVoicepacks.IntegralHeight = false;
            this.listBoxVoicepacks.ItemHeight = 15;
            this.listBoxVoicepacks.Location = new System.Drawing.Point(14, 75);
            this.listBoxVoicepacks.Name = "listBoxVoicepacks";
            this.listBoxVoicepacks.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxVoicepacks.Size = new System.Drawing.Size(613, 386);
            this.listBoxVoicepacks.TabIndex = 3;
            // 
            // checkBoxEnableCombinedVoicepack
            // 
            this.checkBoxEnableCombinedVoicepack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxEnableCombinedVoicepack.AutoSize = true;
            this.checkBoxEnableCombinedVoicepack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnableCombinedVoicepack.ForeColor = System.Drawing.Color.White;
            this.checkBoxEnableCombinedVoicepack.Location = new System.Drawing.Point(14, 476);
            this.checkBoxEnableCombinedVoicepack.Name = "checkBoxEnableCombinedVoicepack";
            this.checkBoxEnableCombinedVoicepack.Size = new System.Drawing.Size(158, 19);
            this.checkBoxEnableCombinedVoicepack.TabIndex = 6;
            this.checkBoxEnableCombinedVoicepack.Text = "Use combined Voicepack";
            this.checkBoxEnableCombinedVoicepack.UseVisualStyleBackColor = true;
            this.checkBoxEnableCombinedVoicepack.CheckedChanged += new System.EventHandler(this.checkBoxEnableCombinedVoicepack_CheckedChanged);
            // 
            // btnExportCombinedVoicepack
            // 
            this.btnExportCombinedVoicepack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportCombinedVoicepack.AutoSize = true;
            this.btnExportCombinedVoicepack.BackColor = System.Drawing.Color.Gray;
            this.btnExportCombinedVoicepack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCombinedVoicepack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportCombinedVoicepack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportCombinedVoicepack.Location = new System.Drawing.Point(360, 468);
            this.btnExportCombinedVoicepack.Name = "btnExportCombinedVoicepack";
            this.btnExportCombinedVoicepack.Size = new System.Drawing.Size(196, 31);
            this.btnExportCombinedVoicepack.TabIndex = 7;
            this.btnExportCombinedVoicepack.Text = "Export Combined Voicepack...";
            this.btnExportCombinedVoicepack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExportCombinedVoicepack.UseVisualStyleBackColor = false;
            this.btnExportCombinedVoicepack.Click += new System.EventHandler(this.btnExportCombinedVoicepack_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.AutoSize = true;
            this.btnRemoveSelected.BackColor = System.Drawing.Color.Gray;
            this.btnRemoveSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSelected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveSelected.Image = global::RecursionTracker.Plugins.VoicepackCombiner.Properties.Resources.action_Cancel_16xMD;
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
            // btnAddVoicepack
            // 
            this.btnAddVoicepack.AutoSize = true;
            this.btnAddVoicepack.BackColor = System.Drawing.Color.Gray;
            this.btnAddVoicepack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddVoicepack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddVoicepack.Image = global::RecursionTracker.Plugins.VoicepackCombiner.Properties.Resources._077_AddFile_16x16_72;
            this.btnAddVoicepack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddVoicepack.Location = new System.Drawing.Point(14, 37);
            this.btnAddVoicepack.Name = "btnAddVoicepack";
            this.btnAddVoicepack.Size = new System.Drawing.Size(131, 31);
            this.btnAddVoicepack.TabIndex = 1;
            this.btnAddVoicepack.Text = "Add Voicepacks...";
            this.btnAddVoicepack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddVoicepack.UseVisualStyleBackColor = false;
            this.btnAddVoicepack.Click += new System.EventHandler(this.btnAddVoicepack_Click);
            // 
            // VoicepackCombinerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 509);
            this.Controls.Add(this.btnExportCombinedVoicepack);
            this.Controls.Add(this.checkBoxEnableCombinedVoicepack);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.listBoxVoicepacks);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddVoicepack);
            this.Controls.Add(this.menuStrip1);
            this.DropShadow = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VoicepackCombinerForm";
            this.Text = "Voicepack Combiner";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addVoicepackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tutorialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button btnAddVoicepack;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox listBoxVoicepacks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.CheckBox checkBoxEnableCombinedVoicepack;
        private System.Windows.Forms.Button btnExportCombinedVoicepack;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;

    }
}