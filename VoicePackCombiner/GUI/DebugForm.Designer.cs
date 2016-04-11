namespace RecursionTracker.Plugins.VoicepackCombiner.GUI
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
            this.btnPrintGlobalVoicepack = new System.Windows.Forms.Button();
            this.btnPrintCombinedVoicepack = new System.Windows.Forms.Button();
            this.btnPrintBackupVoicepack = new System.Windows.Forms.Button();
            this.txtBoxDebugInfo = new System.Windows.Forms.RichTextBox();
            this.btnClearTxtBox = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPrintGlobalVoicepack
            // 
            this.btnPrintGlobalVoicepack.Location = new System.Drawing.Point(12, 12);
            this.btnPrintGlobalVoicepack.Name = "btnPrintGlobalVoicepack";
            this.btnPrintGlobalVoicepack.Size = new System.Drawing.Size(166, 23);
            this.btnPrintGlobalVoicepack.TabIndex = 0;
            this.btnPrintGlobalVoicepack.Text = "Print Global Voicepack";
            this.btnPrintGlobalVoicepack.UseVisualStyleBackColor = true;
            this.btnPrintGlobalVoicepack.Click += new System.EventHandler(this.btnPrintGlobalVoicepack_Click);
            // 
            // btnPrintCombinedVoicepack
            // 
            this.btnPrintCombinedVoicepack.Location = new System.Drawing.Point(12, 41);
            this.btnPrintCombinedVoicepack.Name = "btnPrintCombinedVoicepack";
            this.btnPrintCombinedVoicepack.Size = new System.Drawing.Size(166, 23);
            this.btnPrintCombinedVoicepack.TabIndex = 0;
            this.btnPrintCombinedVoicepack.Text = "Print Combined Voicepack";
            this.btnPrintCombinedVoicepack.UseVisualStyleBackColor = true;
            this.btnPrintCombinedVoicepack.Click += new System.EventHandler(this.btnPrintCombinedVoicepack_Click);
            // 
            // btnPrintBackupVoicepack
            // 
            this.btnPrintBackupVoicepack.Location = new System.Drawing.Point(12, 70);
            this.btnPrintBackupVoicepack.Name = "btnPrintBackupVoicepack";
            this.btnPrintBackupVoicepack.Size = new System.Drawing.Size(166, 23);
            this.btnPrintBackupVoicepack.TabIndex = 0;
            this.btnPrintBackupVoicepack.Text = "Print Backup Voicepack";
            this.btnPrintBackupVoicepack.UseVisualStyleBackColor = true;
            this.btnPrintBackupVoicepack.Click += new System.EventHandler(this.btnPrintBackupVoicepack_Click);
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
            this.Controls.Add(this.btnPrintBackupVoicepack);
            this.Controls.Add(this.btnPrintCombinedVoicepack);
            this.Controls.Add(this.btnPrintGlobalVoicepack);
            this.Name = "DebugForm";
            this.Text = "DebugForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrintGlobalVoicepack;
        private System.Windows.Forms.Button btnPrintCombinedVoicepack;
        private System.Windows.Forms.Button btnPrintBackupVoicepack;
        private System.Windows.Forms.RichTextBox txtBoxDebugInfo;
        private System.Windows.Forms.Button btnClearTxtBox;
    }
}