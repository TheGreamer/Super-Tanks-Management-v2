using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SuperTanksManagement
{
    public partial class BaseSettingsForm : Form
    {
        private readonly string selectedTank;
        private readonly Dictionary<string, string> tankProperties;

        public Dictionary<string, string> UpdatedProperties { get; private set; }

        public BaseSettingsForm(string tankName, Dictionary<string, string> properties)
        {
            InitializeComponent();
            selectedTank = tankName;
            tankProperties = properties;
            UpdatedProperties = new Dictionary<string, string>(tankProperties);
        }

        private void BaseSettingsForm_Load(object sender, EventArgs e)
        {
            int yOffsetForLabel = 12, yOffsetForTextBox = 7;
            labelTankName.Text = "BASE SETTINGS OF " + selectedTank.ToUpperInvariant();

            foreach (var property in tankProperties)
            {
                Label label = new Label
                {
                    Text = property.Key.Replace('_', ' '),
                    Location = new Point(10, yOffsetForLabel),
                    AutoSize = true,
                    ForeColor = Color.PaleTurquoise,
                    BackColor = Color.DarkSlateGray,
                    Font = new Font("Tahoma", 11.25F, FontStyle.Bold)
                };

                TextBox textBox = new TextBox
                {
                    Text = property.Value,
                    Location = new Point(250, yOffsetForTextBox),
                    Size = new Size(100, 26),
                    Tag = property.Key,
                    ForeColor = Color.DarkSlateGray,
                    BackColor = Color.PaleTurquoise,
                    Font = new Font("Tahoma", 11.25F, FontStyle.Bold)
                };

                panelContainer.Controls.Add(label);
                panelContainer.Controls.Add(textBox);

                yOffsetForLabel += 30;
                yOffsetForTextBox += 30;
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            foreach (Control control in panelContainer.Controls)
            {
                if (control is TextBox textBox)
                {
                    string propertyKey = textBox.Tag.ToString();
                    string newValue = textBox.Text;
                    UpdatedProperties[propertyKey] = newValue;
                }
            }
            
            MessageBox.Show("New setting(s) for " + selectedTank + " have been saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void ButtonGoBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}