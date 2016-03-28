namespace RecursionTracker.Plugins.VoicePackCombiner
{
    partial class DebugForm
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
            this.btnPrintGlobalVoicePack = new System.Windows.Forms.Button();
            this.btnPrintCombinedVoicePack = new System.Windows.Forms.Button();
            this.btnPrintBackupVoicePack = new System.Windows.Forms.Button();
            this.txtBoxDebugInfo = new System.Windows.Forms.RichTextBox();
            this.btnClearTxtBox = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPrintGlobalVoicePack
            // 
            this.btnPrintGlobalVoicePack.Location = new System.Drawing.Point(12, 12);
            this.btnPrintGlobalVoicePack.Name = "btnPrintGlobalVoicePack";
            this.btnPrintGlobalVoicePack.Size = new System.Drawing.Size(166, 23);
            this.btnPrintGlobalVoicePack.TabIndex = 0;
            this.btnPrintGlobalVoicePack.Text = "Print Global Voicepack";
            this.btnPrintGlobalVoicePack.UseVisualStyleBackColor = true;
            this.btnPrintGlobalVoicePack.Click += new System.EventHandler(this.btnPrintGlobalVoicePack_Click);
            // 
            // btnPrintCombinedVoicePack
            // 
            this.btnPrintCombinedVoicePack.Location = new System.Drawing.Point(12, 41);
            this.btnPrintCombinedVoicePack.Name = "btnPrintCombinedVoicePack";
            this.btnPrintCombinedVoicePack.Size = new System.Drawing.Size(166, 23);
            this.btnPrintCombinedVoicePack.TabIndex = 0;
            this.btnPrintCombinedVoicePack.Text = "Print Combined Voicepack";
            this.btnPrintCombinedVoicePack.UseVisualStyleBackColor = true;
            this.btnPrintCombinedVoicePack.Click += new System.EventHandler(this.btnPrintCombinedVoicePack_Click);
            // 
            // btnPrintBackupVoicePack
            // 
            this.btnPrintBackupVoicePack.Location = new System.Drawing.Point(12, 70);
            this.btnPrintBackupVoicePack.Name = "btnPrintBackupVoicePack";
            this.btnPrintBackupVoicePack.Size = new System.Drawing.Size(166, 23);
            this.btnPrintBackupVoicePack.TabIndex = 0;
            this.btnPrintBackupVoicePack.Text = "Print Backup VoicePack";
            this.btnPrintBackupVoicePack.UseVisualStyleBackColor = true;
            this.btnPrintBackupVoicePack.Click += new System.EventHandler(this.btnPrintBackupVoicePack_Click);
            // 
            // txtBoxDebugInfo
            // 
            this.txtBoxDebugInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxDebugInfo.BackColor = System.Drawing.Color.LightGray;
            this.txtBoxDebugInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBoxDebugInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDebugInfo.Location = new System.Drawing.Point(184, 12);
            this.txtBoxDebugInfo.Name = "txtBoxDebugInfo";
            this.txtBoxDebugInfo.ReadOnly = true;
            this.txtBoxDebugInfo.Size = new System.Drawing.Size(835, 566);
            this.txtBoxDebugInfo.TabIndex = 2;
            this.txtBoxDebugInfo.Text = "";
            // 
            // btnClearTxtBox
            // 
            this.btnClearTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearTxtBox.Location = new System.Drawing.Point(12, 555);
            this.btnClearTxtBox.Name = "btnClearTxtBox";
            this.btnClearTxtBox.Size = new System.Drawing.Size(165, 23);
            this.btnClearTxtBox.TabIndex = 3;
            this.btnClearTxtBox.Text = "Clear TextBox";
            this.btnClearTxtBox.UseVisualStyleBackColor = true;
            this.btnClearTxtBox.Click += new System.EventHandler(this.btnClearTxtBox_Click);
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 590);
            this.Controls.Add(this.btnClearTxtBox);
            this.Controls.Add(this.txtBoxDebugInfo);
            this.Controls.Add(this.btnPrintBackupVoicePack);
            this.Controls.Add(this.btnPrintCombinedVoicePack);
            this.Controls.Add(this.btnPrintGlobalVoicePack);
            this.Name = "DebugForm";
            this.Text = "DebugForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrintGlobalVoicePack;
        private System.Windows.Forms.Button btnPrintCombinedVoicePack;
        private System.Windows.Forms.Button btnPrintBackupVoicePack;
        private System.Windows.Forms.RichTextBox txtBoxDebugInfo;
        private System.Windows.Forms.Button btnClearTxtBox;
    }
}