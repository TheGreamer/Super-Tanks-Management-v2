using SuperTanksManagement.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperTanksManagement
{
    public partial class GeneralSettingsForm : Form
    {
        private readonly string filePath = string.Empty;
        private readonly Dictionary<string, TextBox> textBoxMapping;
        private readonly List<string> commentLines = new List<string>();

        public GeneralSettingsForm()
        {
            InitializeComponent();

            textBoxMapping = new Dictionary<string, TextBox>()
            {
                { "Disable_All_Powers", textBox1 },
                { "Godmode", textBox2 },
                { "Godmode_Bots", textBox3 },
                { "Hidden_Tank_Chance", textBox4 },
                { "Hidden_Tank_Enable", textBox5 },
                { "Hidden_Tank_Limit", textBox6 },
                { "Hidden_Tank_Variant_Chance", textBox7 },
                { "No_Hidden_Tank_After_Restart_Times", textBox8 },
                { "Overlord_Tank_Limit", textBox9 },
                { "Tank_Health_Bar", textBox10 },
                { "Tank_Rewards_Type", textBox11 },
                { "Tank_Spawn_Notify", textBox12 },
                { "Tank_Spawner", textBox13 }
            };

            filePath = LoadSettings("general_setting.txt", textBoxMapping);

            foreach (Control label in Controls.OfType<Label>())
            {
                if (!label.Name.Equals("labelTitle"))
                {
                    label.Click += new EventHandler(Labels_Click);
                    label.MouseEnter += new EventHandler(Labels_MouseEnter);
                    label.MouseLeave += new EventHandler(Labels_MouseLeave);
                }
            }
        }

        private string LoadSettings(string fileName, Dictionary<string, TextBox> textBoxMapping)
        {
            string filePath = Settings.Default.FilePath.Replace("tank_setting.txt", fileName);
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("#"))
                {
                    commentLines.Add(line);
                    continue;
                }

                string[] parts = line.Split('=').Select(p => p.Trim()).ToArray();

                if (parts.Length == 2)
                {
                    string key = parts[0];
                    string value = parts[1];

                    if (textBoxMapping.ContainsKey(key))
                    {
                        textBoxMapping[key].Text = value;
                    }
                }
            }

            return filePath;
        }

        private void SaveSettings(string filePath, Dictionary<string, TextBox> textBoxMapping)
        {
            string[] lines = textBoxMapping.Select(kvp => $"{kvp.Key} = {kvp.Value.Text}").ToArray();
            File.WriteAllLines(filePath, commentLines);
            File.AppendAllLines(filePath, lines);
            MessageBox.Show("General settings have been saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveSettings(filePath, textBoxMapping);
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
                case "Disable All Powers": information = "Set it 1 to disable tank powers. Set it 0 to enable.\r\n\r\nUsage : Disable_All_Powers = 0\r\n\r\nRecommended Value : 0"; break;
                case "God Mode": information = "Set it 1 to immune to all damage from most of damage sources. Set it 0 to disable this feature.\r\n\r\nUsage : Godmode = 1\r\n\r\nRecommended Value : 0"; break;
                case "God Mode Bots": information = "Set it 1 to immune to all damage from most of damage sources. Set it 0 to disable this feature.\r\n\r\nUsage : Godmode_Bots = 0\r\n\r\nRecommended Value : 0"; break;
                case "Hidden Tank Chance": information = "Chance to spawn hidden class tank in percent.\r\n\r\nUsage : Hidden_Tank_Chance = 10\r\n\r\nThis means hidden class tank's spawn chance is %10.\r\n\r\nRecommended Value : 1"; break;
                case "Hidden Tank Enable": information = "Set it 1 to enable hidden class tank. Set it 0 to disable.\r\n\r\nUsage : Hidden_Tank_Enable = 1\r\n\r\nRecommended Value : 1"; break;
                case "Hidden Tank Limit": information = "Value of how many hidden class tanks can be spawned at a time.\r\n\r\nUsage : Hidden_Tank_Limit = 2\r\n\r\nKeep in mind that this class is the hardest class. Also spawning too many could crash the game.\r\n\r\nRecommended Value : 1"; break;
                case "Hidden Tank Variant Chance": information = "Variant chance for hidden tank class in percent.\r\n\r\nUsage : Hidden_Tank_Variant_Chance = 50\r\n\r\nThis means hidden variant class tank's spawn chance is %50.\r\n\r\nRecommended Value : 30"; break;
                case "No Hidden Tank After Restart Times": information = "Prevent hidden class tank from spawning after restarting a chapter.\r\n\r\nUsage : No_Hidden_Tank_After_Restart_Times = 1\r\n\r\nRecommended Value : 2"; break;
                case "Overlord Tank Limit": information = "Value of how many overlord class tanks can be spawned at a time.\r\n\r\nUsage : Overlord_Tank_Limit = 2\r\n\r\nKeep in mind that spawning too many overlord class tanks could crash the game.\r\n\r\nRecommended Value : 1"; break;
                case "Tank Health Bar": information = "Set it 1 to show tank's health bar when attacking it. Set it 0 to hide the health bar.\r\n\r\nUsage : Tank_Health_Bar = 1\r\n\r\nRecommended Value : 0"; break;
                case "Tank Rewards Type": information = "Value of which health item pack will be droped when tank is dead.\r\n\r\nUsage : Tank_Rewards_Type = 1\r\n\r\n0 -> Disable\r\n1 -> Med Kits/Defibs\r\n2 -> Pills/Adrenalines/Defibs\r\n3 -> Random Set\r\n\r\nRecommended Value : 0"; break;
                case "Tank Spawn Notify": information = "Set it 1 to show notification when tank spawned with special powers. Set it 0 to disable notifications.\r\n\r\nUsage : Tank_Spawn_Notify = 0\r\n\r\nRecommended Value : 1"; break;
                case "Tank Spawner": information = "Set it 1 to enable tank spawn around the saferoom if haven't encountered any tank. Set to 0 to disable.\r\n\r\nUsage : Tank_Spawner = 0\r\n\r\nRecommended Value : 0"; break;
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