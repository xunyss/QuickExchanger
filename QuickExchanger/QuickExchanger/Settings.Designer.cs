namespace QuickExchanger
{
    partial class Settings
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Sample Proxy Server");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.labelProxyListTitle = new System.Windows.Forms.Label();
            this.serverList = new System.Windows.Forms.ListView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.serverInfoGroup = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // labelProxyListTitle
            // 
            this.labelProxyListTitle.AutoSize = true;
            this.labelProxyListTitle.Location = new System.Drawing.Point(12, 9);
            this.labelProxyListTitle.Name = "labelProxyListTitle";
            this.labelProxyListTitle.Size = new System.Drawing.Size(83, 12);
            this.labelProxyListTitle.TabIndex = 0;
            this.labelProxyListTitle.Text = "proxy servers";
            // 
            // serverList
            // 
            this.serverList.GridLines = true;
            this.serverList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.serverList.Location = new System.Drawing.Point(12, 52);
            this.serverList.MultiSelect = false;
            this.serverList.Name = "serverList";
            this.serverList.Size = new System.Drawing.Size(180, 171);
            this.serverList.TabIndex = 1;
            this.serverList.UseCompatibleStateImageBehavior = false;
            this.serverList.View = System.Windows.Forms.View.List;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(232, 197);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(69, 26);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(307, 197);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(106, 24);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(86, 22);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(14, 24);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(86, 22);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // serverInfoGroup
            // 
            this.serverInfoGroup.Location = new System.Drawing.Point(198, 9);
            this.serverInfoGroup.Name = "serverInfoGroup";
            this.serverInfoGroup.Size = new System.Drawing.Size(176, 182);
            this.serverInfoGroup.TabIndex = 6;
            this.serverInfoGroup.TabStop = false;
            this.serverInfoGroup.Text = "server information";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 233);
            this.Controls.Add(this.serverInfoGroup);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.serverList);
            this.Controls.Add(this.labelProxyListTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings - QuickExchanger";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProxyListTitle;
        private System.Windows.Forms.ListView serverList;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox serverInfoGroup;
    }
}