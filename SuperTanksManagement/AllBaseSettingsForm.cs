using SuperTanksManagement.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SuperTanksManagement
{
    public partial class AllBaseSettingsForm : Form
    {
        public AllBaseSettingsForm()
        {
            InitializeComponent();
            
            foreach (Control label in panelContainer.Controls.OfType<Label>())
            {
                if (!label.Name.Equals("labelTitle"))
                {
                    label.Click += new EventHandler(Labels_Click);
                    label.MouseEnter += new EventHandler(Labels_MouseEnter);
                    label.MouseLeave += new EventHandler(Labels_MouseLeave);
                }
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            TextBox[] textBoxes = { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10 };
            string[] propertyTitles = { "Base_Damage", "Base_Health", "Difficulty_Appearance", "Enable", "Finale_Only", "Fire_Immune", "Health_Multiply", "Overlord_Chance", "Variant_Chance", "Variant_Fire_Immune" };

            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(textBoxes[i].Text))
                {
                    properties[propertyTitles[i]] = textBoxes[i].Text.Trim();
                }
            }

            if (properties.Count == 0)
            {
                MessageBox.Show("You have not entered any new values to change the settings.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string filePath = Settings.Default.FilePath;
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                foreach (var property in properties)
                {
                    lines[i] = Regex.Replace(lines[i], $@"^    {property.Key}\s*=\s*.*", $"    {property.Key} = {property.Value}");
                }
            }

            string content = string.Join("\r\n", lines);
            File.WriteAllText(filePath, content);

            MessageBox.Show("New base settings for all super tanks have been saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void ButtonGoBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Labels_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            string information = string.Empty;
            string title = label.Text.Replace(" ?", "");

            switch (title)
            {
                case "Base Damage": information = "Tank's damage in percent.\r\n\r\nUsage : Base_Damage = 40\r\n\r\nThis means tank will do 40% of the original damage.\r\n\r\nRecommended Value : 100"; break;
                case "Base Health": information = "Tank's health. \r\n\r\nUsage : Base_Health = 4000\r\n\r\nSet it 4000 to use the original health.\r\n\r\nRecommended Value : 4000"; break;
                case "Difficulty Appearance": information = "Allow tank to be spawned on specific difficulty.\r\n\r\nUsage 1 : Difficulty_Appearance = 5\r\nUsage 2 : Difficulty_Appearance = 1|2|3\r\n\r\nSetting it 5 means tank can be spawned on all difficulties. You can put | sign to allow more than one difficulty.\r\n5 -> All Difficulties\r\n4 -> Expert\r\n3 -> Advanced\r\n2 -> Normal\r\n1 -> Easy\r\n1|2|3 -> Easy, Normal, Advanced\r\n\r\nRecommended Value : 5"; break;
                case "Enable": information = "Enable or disable tank from spawning.\r\n\r\nUsage : Enable = true\r\n\r\nSet it 'true' and 'false' only."; break;
                case "Finale Only": information = "Allow tank only to be spawned on a campaign's finale.\r\n\r\nUsage 1 : Finale_Only = false\r\n\r\nSet it 'true' and 'false' only.\r\n\r\nRecommended Value : false"; break;
                case "Fire Immune": information = "Enable or disable fire immunity on tank.\r\n\r\nUsage : Fire_Immune = false\r\n\r\nSet it 'true' and 'false' only."; break;
                case "Health Multiply": information = "Multiplies tank's health.\r\n\r\nUsage : Health_Multiply = 1.0\r\n\r\nSetting it 1.0 means tank will have 100% more health.\r\n\r\nRecommended Value : 0"; break;
                case "Overlord Chance": information = "Chance to spawn overlord class of tank in percent.\r\n\r\nUsage : Overlord_Chance = 15\r\n\r\nSetting it 15 means the spawn chance for overlord is %15.\r\n\r\nRecommended Value : 5"; break;
                case "Variant Chance": information = "Chance to spawn variant class of tank in percent.\r\n\r\nUsage : Variant_Chance = 50\r\n\r\nSetting it 15 means the spawn chance for variant is %15.\r\n\r\nRecommended Value : 30"; break;
                case "Variant Fire Immune": information = "Enable or disable fire immunity on tank's variant.\r\n\r\nUsage : Variant_Fire_Immune = true\r\n\r\nSet it 'true' and 'false' only."; break;
            }

            if (label != null)
            {
                MessageBox.Show(information, $"Settings Guide - {title}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Labels_MouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            label.ForeColor = Color.Aqua;
            label.Text += " ?";
            Cursor = Cursors.Hand;
        }

        private void Labels_MouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            label.ForeColor = Color.PaleTurquoise;
            label.Text = label.Text.Replace(" ?", "");
            Cursor = Cursors.Default;
        }
    }
}