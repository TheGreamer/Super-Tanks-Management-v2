namespace SuperTanksManagement
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxTanks;
        private System.Windows.Forms.Button buttonChangeBaseSettings;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listBoxTanks = new System.Windows.Forms.ListBox();
            this.buttonChangeBaseSettings = new System.Windows.Forms.Button();
            this.buttonChangeAllBaseSettings = new System.Windows.Forms.Button();
            this.buttonChangePowerSettings = new System.Windows.Forms.Button();
            this.buttonChangeGeneralSettings = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.labelSuperTanks = new System.Windows.Forms.Label();
            this.labelOptions = new System.Windows.Forms.Label();
            this.buttonGetTankInformation = new System.Windows.Forms.Button();
            this.labelPowers = new System.Windows.Forms.Label();
            this.listBoxPowers = new System.Windows.Forms.ListBox();
            this.buttonGetPowerInformation = new System.Windows.Forms.Button();
            this.buttonClearSelections = new System.Windows.Forms.Button();
            this.buttonBrowseModPage = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxTanks
            // 
            this.listBoxTanks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxTanks.BackColor = System.Drawing.Color.PaleTurquoise;
            this.listBoxTanks.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxTanks.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.listBoxTanks.FormattingEnabled = true;
            this.listBoxTanks.ItemHeight = 20;
            this.listBoxTanks.Location = new System.Drawing.Point(9, 49);
            this.listBoxTanks.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxTanks.Name = "listBoxTanks";
            this.listBoxTanks.Size = new System.Drawing.Size(278, 364);
            this.listBoxTanks.TabIndex = 0;
            this.listBoxTanks.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxTanks_DrawItem);
            this.listBoxTanks.SelectedIndexChanged += new System.EventHandler(this.ListBoxTanks_SelectedIndexChanged);
            this.listBoxTanks.DoubleClick += new System.EventHandler(this.ListBoxTanks_DoubleClick);
            // 
            // buttonChangeBaseSettings
            // 
            this.buttonChangeBaseSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeBaseSettings.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonChangeBaseSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonChangeBaseSettings.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonChangeBaseSettings.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonChangeBaseSettings.Location = new System.Drawing.Point(303, 90);
            this.buttonChangeBaseSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChangeBaseSettings.Name = "buttonChangeBaseSettings";
            this.buttonChangeBaseSettings.Size = new System.Drawing.Size(237, 30);
            this.buttonChangeBaseSettings.TabIndex = 1;
            this.buttonChangeBaseSettings.Text = "Base Settings";
            this.buttonChangeBaseSettings.UseVisualStyleBackColor = false;
            this.buttonChangeBaseSettings.Click += new System.EventHandler(this.ButtonChangeBaseSettings_Click);
            // 
            // buttonChangeAllBaseSettings
            // 
            this.buttonChangeAllBaseSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeAllBaseSettings.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonChangeAllBaseSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonChangeAllBaseSettings.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonChangeAllBaseSettings.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonChangeAllBaseSettings.Location = new System.Drawing.Point(303, 131);
            this.buttonChangeAllBaseSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChangeAllBaseSettings.Name = "buttonChangeAllBaseSettings";
            this.buttonChangeAllBaseSettings.Size = new System.Drawing.Size(237, 30);
            this.buttonChangeAllBaseSettings.TabIndex = 2;
            this.buttonChangeAllBaseSettings.Text = "All Base Settings";
            this.buttonChangeAllBaseSettings.UseVisualStyleBackColor = false;
            this.buttonChangeAllBaseSettings.Click += new System.EventHandler(this.ButtonChangeAllBaseSettings_Click);
            this.buttonChangeAllBaseSettings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonChangeAllBaseSettings_MouseDown);
            // 
            // buttonChangePowerSettings
            // 
            this.buttonChangePowerSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangePowerSettings.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonChangePowerSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonChangePowerSettings.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonChangePowerSettings.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonChangePowerSettings.Location = new System.Drawing.Point(303, 172);
            this.buttonChangePowerSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChangePowerSettings.Name = "buttonChangePowerSettings";
            this.buttonChangePowerSettings.Size = new System.Drawing.Size(237, 30);
            this.buttonChangePowerSettings.TabIndex = 3;
            this.buttonChangePowerSettings.Text = "Power Settings";
            this.buttonChangePowerSettings.UseVisualStyleBackColor = false;
            this.buttonChangePowerSettings.Click += new System.EventHandler(this.ButtonChangePowerSettings_Click);
            this.buttonChangePowerSettings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonChangePowerSettings_MouseDown);
            // 
            // buttonChangeGeneralSettings
            // 
            this.buttonChangeGeneralSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeGeneralSettings.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonChangeGeneralSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonChangeGeneralSettings.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonChangeGeneralSettings.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonChangeGeneralSettings.Location = new System.Drawing.Point(303, 49);
            this.buttonChangeGeneralSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChangeGeneralSettings.Name = "buttonChangeGeneralSettings";
            this.buttonChangeGeneralSettings.Size = new System.Drawing.Size(237, 30);
            this.buttonChangeGeneralSettings.TabIndex = 4;
            this.buttonChangeGeneralSettings.Text = "General Settings";
            this.buttonChangeGeneralSettings.UseVisualStyleBackColor = false;
            this.buttonChangeGeneralSettings.Click += new System.EventHandler(this.ButtonChangeGeneralSettings_Click);
            this.buttonChangeGeneralSettings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonChangeGeneralSettings_MouseDown);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Text files (*.txt)|*.txt";
            this.openFileDialog.Title = "Select (tank_setting.txt) file";
            // 
            // labelSuperTanks
            // 
            this.labelSuperTanks.AutoSize = true;
            this.labelSuperTanks.Font = new System.Drawing.Font("Tahoma", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelSuperTanks.ForeColor = System.Drawing.Color.Aquamarine;
            this.labelSuperTanks.Location = new System.Drawing.Point(92, 18);
            this.labelSuperTanks.Name = "labelSuperTanks";
            this.labelSuperTanks.Size = new System.Drawing.Size(114, 18);
            this.labelSuperTanks.TabIndex = 9;
            this.labelSuperTanks.Text = "SUPER TANKS";
            // 
            // labelOptions
            // 
            this.labelOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOptions.AutoSize = true;
            this.labelOptions.Font = new System.Drawing.Font("Tahoma", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelOptions.ForeColor = System.Drawing.Color.Aquamarine;
            this.labelOptions.Location = new System.Drawing.Point(381, 18);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(80, 18);
            this.labelOptions.TabIndex = 10;
            this.labelOptions.Text = "OPTIONS";
            // 
            // buttonGetTankInformation
            // 
            this.buttonGetTankInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetTankInformation.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonGetTankInformation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGetTankInformation.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonGetTankInformation.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonGetTankInformation.Location = new System.Drawing.Point(303, 213);
            this.buttonGetTankInformation.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGetTankInformation.Name = "buttonGetTankInformation";
            this.buttonGetTankInformation.Size = new System.Drawing.Size(237, 30);
            this.buttonGetTankInformation.TabIndex = 11;
            this.buttonGetTankInformation.Text = "Get Tank Information";
            this.buttonGetTankInformation.UseVisualStyleBackColor = false;
            this.buttonGetTankInformation.Click += new System.EventHandler(this.ButtonGetTankInformation_Click);
            // 
            // labelPowers
            // 
            this.labelPowers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPowers.AutoSize = true;
            this.labelPowers.Font = new System.Drawing.Font("Tahoma", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.labelPowers.ForeColor = System.Drawing.Color.Aquamarine;
            this.labelPowers.Location = new System.Drawing.Point(656, 18);
            this.labelPowers.Name = "labelPowers";
            this.labelPowers.Size = new System.Drawing.Size(75, 18);
            this.labelPowers.TabIndex = 13;
            this.labelPowers.Text = "POWERS";
            // 
            // listBoxPowers
            // 
            this.listBoxPowers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPowers.BackColor = System.Drawing.Color.PaleTurquoise;
            this.listBoxPowers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxPowers.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.listBoxPowers.FormattingEnabled = true;
            this.listBoxPowers.ItemHeight = 20;
            this.listBoxPowers.Location = new System.Drawing.Point(555, 49);
            this.listBoxPowers.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxPowers.Name = "listBoxPowers";
            this.listBoxPowers.Size = new System.Drawing.Size(278, 364);
            this.listBoxPowers.TabIndex = 12;
            this.listBoxPowers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxPowers_DrawItem);
            this.listBoxPowers.SelectedIndexChanged += new System.EventHandler(this.ListBoxPowers_SelectedIndexChanged);
            this.listBoxPowers.DoubleClick += new System.EventHandler(this.ListBoxPowers_DoubleClick);
            // 
            // buttonGetPowerInformation
            // 
            this.buttonGetPowerInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetPowerInformation.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonGetPowerInformation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGetPowerInformation.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonGetPowerInformation.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonGetPowerInformation.Location = new System.Drawing.Point(303, 254);
            this.buttonGetPowerInformation.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGetPowerInformation.Name = "buttonGetPowerInformation";
            this.buttonGetPowerInformation.Size = new System.Drawing.Size(237, 30);
            this.buttonGetPowerInformation.TabIndex = 14;
            this.buttonGetPowerInformation.Text = "Get Power Information";
            this.buttonGetPowerInformation.UseVisualStyleBackColor = false;
            this.buttonGetPowerInformation.Click += new System.EventHandler(this.ButtonGetPowerInformation_Click);
            // 
            // buttonClearSelections
            // 
            this.buttonClearSelections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearSelections.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonClearSelections.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClearSelections.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonClearSelections.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonClearSelections.Location = new System.Drawing.Point(303, 295);
            this.buttonClearSelections.Margin = new System.Windows.Forms.Padding(2);
            this.buttonClearSelections.Name = "buttonClearSelections";
            this.buttonClearSelections.Size = new System.Drawing.Size(237, 30);
            this.buttonClearSelections.TabIndex = 15;
            this.buttonClearSelections.Text = "Clear Selections";
            this.buttonClearSelections.UseVisualStyleBackColor = false;
            this.buttonClearSelections.Click += new System.EventHandler(this.ButtonClearSelections_Click);
            // 
            // buttonBrowseModPage
            // 
            this.buttonBrowseModPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseModPage.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonBrowseModPage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBrowseModPage.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonBrowseModPage.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonBrowseModPage.Location = new System.Drawing.Point(303, 336);
            this.buttonBrowseModPage.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBrowseModPage.Name = "buttonBrowseModPage";
            this.buttonBrowseModPage.Size = new System.Drawing.Size(237, 30);
            this.buttonBrowseModPage.TabIndex = 16;
            this.buttonBrowseModPage.Text = "Browse Mod Page";
            this.buttonBrowseModPage.UseVisualStyleBackColor = false;
            this.buttonBrowseModPage.Click += new System.EventHandler(this.ButtonBrowseModPage_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.BackColor = System.Drawing.Color.PaleTurquoise;
            this.buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonHelp.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.buttonHelp.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.buttonHelp.Location = new System.Drawing.Point(303, 377);
            this.buttonHelp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(237, 30);
            this.buttonHelp.TabIndex = 17;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = false;
            this.buttonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(842, 424);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonBrowseModPage);
            this.Controls.Add(this.buttonClearSelections);
            this.Controls.Add(this.buttonGetPowerInformation);
            this.Controls.Add(this.labelPowers);
            this.Controls.Add(this.listBoxPowers);
            this.Controls.Add(this.buttonGetTankInformation);
            this.Controls.Add(this.labelOptions);
            this.Controls.Add(this.labelSuperTanks);
            this.Controls.Add(this.buttonChangeGeneralSettings);
            this.Controls.Add(this.buttonChangePowerSettings);
            this.Controls.Add(this.buttonChangeAllBaseSettings);
            this.Controls.Add(this.buttonChangeBaseSettings);
            this.Controls.Add(this.listBoxTanks);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Super Tanks Management System";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button buttonChangeAllBaseSettings;
        private System.Windows.Forms.Button buttonChangePowerSettings;
        private System.Windows.Forms.Button buttonChangeGeneralSettings;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelSuperTanks;
        private System.Windows.Forms.Label labelOptions;
        private System.Windows.Forms.Button buttonGetTankInformation;
        private System.Windows.Forms.Label labelPowers;
        private System.Windows.Forms.ListBox listBoxPowers;
        private System.Windows.Forms.Button buttonGetPowerInformation;
        private System.Windows.Forms.Button buttonClearSelections;
        private System.Windows.Forms.Button buttonBrowseModPage;
        private System.Windows.Forms.Button buttonHelp;
    }
}