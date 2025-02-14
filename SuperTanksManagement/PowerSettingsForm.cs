using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SuperTanksManagement
{
    public partial class PowerSettingsForm : Form
    {
        private readonly string selectedPower;
        private readonly Dictionary<string, string> powerProperties;

        public Dictionary<string, string> UpdatedProperties { get; private set; }

        public PowerSettingsForm(string powerName, Dictionary<string, string> properties)
        {
            InitializeComponent();
            selectedPower = powerName;
            powerProperties = properties;
            UpdatedProperties = new Dictionary<string, string>(powerProperties);
        }

        private void PowerSettingsForm_Load(object sender, EventArgs e)
        {
            int yOffsetForLabel = 12, yOffsetForTextBox = 7;
            labelPowerName.Text = "POWER SETTINGS OF " + selectedPower.ToUpperInvariant();

            foreach (var property in powerProperties)
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
                    Location = new Point(390, yOffsetForTextBox),
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

            MessageBox.Show("New setting(s) for " + selectedPower + " have been saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void ButtonGoBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}