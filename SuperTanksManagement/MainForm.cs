using SuperTanksManagement.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SuperTanksManagement
{
    public partial class MainForm : Form
    {
        private string filePath = string.Empty;
        private string selectedTank = string.Empty;
        private string selectedPower = string.Empty;
        private bool state = false;
        private readonly List<bool?> enableStates = new List<bool?>();
        private readonly List<string> tankDetails = new List<string>();
        private readonly List<string> powerDetails = new List<string>();
        private readonly List<string> tankCommentLines = new List<string>();
        private readonly List<string> powerCommentLines = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            LoadFilePath();
        }

        private void LoadFilePath()
        {
            if (!string.IsNullOrEmpty(Settings.Default.FilePath) && File.Exists(Settings.Default.FilePath))
            {
                filePath = Settings.Default.FilePath;
                state = true;
            }
            else
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog.SafeFileName.Equals("tank_setting.txt"))
                    {
                        filePath = openFileDialog.FileName;
                        Settings.Default.FilePath = filePath;
                        Settings.Default.Save();
                        state = true;
                    }
                    else
                    {
                        MessageBox.Show("You must select 'tank_setting.txt' in order to proceed. The application will close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("File selection was cancelled. The application will close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadTanksFromFile()
        {
            string[] lines = File.ReadAllLines(filePath);
            string currentTank = "";
            string tankDetail = "";
            bool insideBlock = false;
            bool? enableState = null;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("#"))
                {
                    tankCommentLines.Add(line);
                    continue;
                }

                if (trimmedLine.EndsWith("{"))
                {
                    currentTank = trimmedLine.Split('=')[0].Trim();
                    tankDetail = trimmedLine + "\n";
                    insideBlock = true;
                    enableState = null;
                }
                else if (insideBlock)
                {
                    tankDetail += trimmedLine + "\n";

                    if (trimmedLine.StartsWith("Enable"))
                    {
                        enableState = trimmedLine.Split('=')[1].Trim().ToLower() == "true";
                    }

                    if (trimmedLine == "}")
                    {
                        listBoxTanks.Items.Add(currentTank.Replace('_', ' '));
                        tankDetails.Add(tankDetail.Trim());
                        enableStates.Add(enableState);
                        insideBlock = false;
                    }
                }
            }
        }

        private void LoadPowersFromFile()
        {
            string[] lines = File.ReadAllLines(filePath.Replace("tank_setting", "tank_power_setting"));
            string currentPower = "";
            string powerDetail = "";
            bool insideBlock = false;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("#"))
                {
                    powerCommentLines.Add(line);
                    continue;
                }

                if (trimmedLine.EndsWith("{"))
                {
                    currentPower = trimmedLine.Split('=')[0].Trim();
                    powerDetail = trimmedLine + "\n";
                    insideBlock = true;
                }
                else if (insideBlock)
                {
                    powerDetail += trimmedLine + "\n";

                    if (trimmedLine == "}")
                    {
                        listBoxPowers.Items.Add(currentPower.Replace('_', ' '));
                        powerDetails.Add(powerDetail.Trim());
                        insideBlock = false;
                    }
                }
            }
        }

        private void SaveChangesToFile(string filePath, List<string> commentLines, List<string> details)
        {
            List<string> updatedLines = new List<string>();
            updatedLines.AddRange(commentLines);
            updatedLines.AddRange(details);
            string updatedFileContent = EditFile(string.Join("\r\n", updatedLines));
            File.WriteAllText(filePath, updatedFileContent);
        }

        private void RefreshLists()
        {
            tankDetails.Clear();
            powerDetails.Clear();
            enableStates.Clear();
            tankCommentLines.Clear();
            powerCommentLines.Clear();
            listBoxTanks.Items.Clear();
            listBoxPowers.Items.Clear();
            listBoxTanks.SelectedIndex = -1;
            listBoxPowers.SelectedIndex = -1;
        }

        private string EditFile(string input)
        {
            string[] lines = input.Split(new[] { '\n' }, StringSplitOptions.None);
            string result = string.Empty;
            bool previousLineEmpty = false;
            bool isTankBlockEnded = false;
            bool hasInsertedSpaceBeforeTheFirstOption = false;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (!previousLineEmpty && !isTankBlockEnded)
                    {
                        result += "\n";
                        previousLineEmpty = true;
                    }
                }
                else
                {
                    if ((line.StartsWith("Acid_Tank = {") || line.StartsWith("Acid = {")) && !hasInsertedSpaceBeforeTheFirstOption)
                    {
                        if (!previousLineEmpty)
                        {
                            result += "\n";
                        }
                        result += line + "\n";
                        hasInsertedSpaceBeforeTheFirstOption = true;
                        isTankBlockEnded = false;
                    }
                    else if (line.Contains('}'))
                    {
                        result += line + "\n";
                        isTankBlockEnded = true;
                    }
                    else
                    {
                        result += (line.Contains('{') || line.Contains('}') || line.Contains('#') ? "" : "    ") + line + "\n";
                        isTankBlockEnded = false;
                    }
                    previousLineEmpty = false;
                }
            }
            
            result = result.TrimEnd();
            return result;
        }

        private Dictionary<string, string> ParseProperties(string details)
        {
            var properties = new Dictionary<string, string>();
            var lines = details.Split('\n');

            foreach (var line in lines.Skip(1))
            {
                if (line.Trim() == "}") break;

                var parts = line.Split('=');

                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    properties[key] = value;
                }
            }

            return properties;
        }

        private string ConvertPropertiesToText(Dictionary<string, string> properties, string tankOrPowerName)
        {
            List<string> lines = new List<string>
            {
                $"{tankOrPowerName} = {{"
            };

            foreach (var property in properties)
            {
                lines.Add($"{property.Key} = {property.Value}");
            }

            lines.Add("}");

            return string.Join(Environment.NewLine, lines);
        }

        private void ViewTextFile(string file, string name, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (MessageBox.Show($"Would you like to view '{name}.txt' file?", "Text File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(file)
                    {
                        UseShellExecute = true
                    });
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (state)
            {
                LoadTanksFromFile();
                LoadPowersFromFile();
            }
            else
            {
                Application.Exit();
            }
        }

        private void ListBoxTanks_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            bool? isEnabled = enableStates[e.Index];

            Color backgroundColor = isEnabled.HasValue && isEnabled.Value ? Color.FromArgb(76, 231, 100) : Color.FromArgb(255, 100, 120);
            Color foregroundColor = Color.FromArgb(0, 57, 57);

            if (isSelected)
            {
                backgroundColor = Color.FromArgb(21, 76, 121);
                foregroundColor = Color.FromArgb(171, 219, 227);
            }

            e.DrawBackground();

            using (Brush brush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            string tankName = listBoxTanks.Items[e.Index].ToString();
            TextRenderer.DrawText(e.Graphics, tankName, e.Font, e.Bounds, foregroundColor, TextFormatFlags.Left);

            e.DrawFocusRectangle();
        }

        private void ListBoxPowers_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color backgroundColor = Color.FromArgb(81, 186, 234);
            Color foregroundColor = Color.FromArgb(0, 57, 57);

            if (isSelected)
            {
                backgroundColor = Color.FromArgb(21, 76, 121);
                foregroundColor = Color.FromArgb(171, 219, 227);
            }

            e.DrawBackground();

            using (Brush brush = new SolidBrush(backgroundColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            string powerName = listBoxPowers.Items[e.Index].ToString();
            TextRenderer.DrawText(e.Graphics, powerName, e.Font, e.Bounds, foregroundColor, TextFormatFlags.Left);

            e.DrawFocusRectangle();
        }

        private void ListBoxTanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTanks.SelectedIndex >= 0)
            {
                selectedTank = listBoxTanks.SelectedItem.ToString();
            }
        }

        private void ListBoxPowers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPowers.SelectedIndex >= 0)
            {
                selectedPower = listBoxPowers.SelectedItem.ToString();
            }
        }

        private void ListBoxTanks_DoubleClick(object sender, EventArgs e)
        {
            buttonGetTankInformation.PerformClick();
        }

        private void ListBoxPowers_DoubleClick(object sender, EventArgs e)
        {
            buttonGetPowerInformation.PerformClick();
        }

        private void ButtonChangeBaseSettings_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedTank))
            {
                int selectedIndex = listBoxTanks.SelectedIndex;

                if (selectedIndex >= 0)
                {
                    Dictionary<string, string> selectedTankProperties = ParseProperties(tankDetails[selectedIndex]);
                    BaseSettingsForm baseSettingsForm = new BaseSettingsForm(selectedTank, selectedTankProperties);

                    if (baseSettingsForm.ShowDialog() == DialogResult.OK)
                    {
                        string updatedDetails = ConvertPropertiesToText(baseSettingsForm.UpdatedProperties, selectedTank);
                        tankDetails[selectedIndex] = updatedDetails;
                        SaveChangesToFile(filePath, tankCommentLines, tankDetails);
                        RefreshLists();
                        LoadTanksFromFile();
                        LoadPowersFromFile();
                    }
                }
                else
                {
                    MessageBox.Show("In order to change base settings of a super tank, you must select one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("In order to change base settings of a super tank, you must select one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void ButtonChangeAllBaseSettings_Click(object sender, EventArgs e)
        {
            new AllBaseSettingsForm().ShowDialog();
            RefreshLists();
            LoadTanksFromFile();
            LoadPowersFromFile();
        }

        private void ButtonChangeGeneralSettings_Click(object sender, EventArgs e)
        {
            new GeneralSettingsForm().ShowDialog();
        }

        private void ButtonChangePowerSettings_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedPower))
            {
                int selectedIndex = listBoxPowers.SelectedIndex;

                if (selectedIndex >= 0)
                {
                    Dictionary<string, string> selectedPowerProperties = ParseProperties(powerDetails[selectedIndex]);
                    PowerSettingsForm powerSettingsForm = new PowerSettingsForm(selectedPower, selectedPowerProperties);

                    if (powerSettingsForm.ShowDialog() == DialogResult.OK)
                    {
                        string updatedDetails = ConvertPropertiesToText(powerSettingsForm.UpdatedProperties, selectedPower);
                        powerDetails[selectedIndex] = updatedDetails;
                        SaveChangesToFile(filePath.Replace("tank_setting", "tank_power_setting"), powerCommentLines, powerDetails);
                        RefreshLists();
                        LoadTanksFromFile();
                        LoadPowersFromFile();
                    }
                }
                else
                {
                    MessageBox.Show("In order to change power settings, you must select one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("In order to change power settings, you must select one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonGetTankInformation_Click(object sender, EventArgs e)
        {
            if (listBoxTanks.SelectedIndex >= 0)
            {
                string information = string.Empty;

                switch (selectedTank)
                {
                    case "Acid Tank": information = "➼ Acid Tank (Common)\r\n• Creates acid to damage nearby survivors.\r\n• Any type of attack is recommended as long as you don’t get too close.\r\n\r\n\r\n➼ Corrosive Tank (Variant)\r\n• Creates large acid pools in it's surroundings.\r\n• Survivors affected by it's acid become temporarily slowed.\r\n• Molotovs and medium-range attacks are recommended."; break;
                    case "Admin Tank": information = "➼ Admin Tank (Common)\r\n• Acts like an admin, using illegal administrator commands to harm survivors.\r\n• Any attack is recommended as long as you don’t fight alone.\r\n\r\n\r\n➼ Abusive Tank (Variant)\r\n• Uses even more dangerous admin commands than Admin Tank.\r\n• If another tank is present, it is recommended kill this tank first and never fight it alone."; break;
                    case "Alchemist Tank": information = "➼ Alchemist Tank (Common)\r\n• Can randomly apply special elemental attacks, such as fire, wind, poison, weapon disarm, or freezing effects.\r\n• Molotovs, high-ground attacks, or shotguns are recommended.\r\n\r\n\r\n➼ Elementalist Tank (Variant)\r\n• Randomly uses elemental attacks including: fire bursts, storm attacks, powerful wind gusts, water blasts that disarm weapons.\r\n• Molotovs and ranged attacks are recommended."; break;
                    case "Binder Tank": information = "➼ Binder Tank (Common)\r\n• Marks the nearest survivor within a certain radius.\r\n• While marked, the survivor takes damage whenever the tank is hit.\r\n• If a survivor is marked, the tank takes no damage.\r\n• If the marked survivor is downed or killed, the mark disappears and transfers to another nearby survivor.\r\n• Since damage to the tank is greatly reduced while someone is marked, it is recommended to attack from a distance.\r\n\r\n\r\n➼ Linker Tank (Variant)\r\n• Marks all nearby survivors.\r\n• All marked survivors can see each other.\r\n• Reduces incoming damage based on the number of marked survivors.\r\n• If a marked survivor is downed or killed, their mark disappears.\r\n• If any marked survivor takes damage, the damage is split among all marked survivor, up to 75% of the original damage.\r\n• If many survivors are marked, the tank takes significantly less damage.\r\n• It is recommended to attack from a distance."; break;
                    case "Black Hole Tank": information = "➼ Black Hole Tank (Common)\r\n• Creates a black hole on it's location at intervals, pulling everyone including special infected and itself toward the center of the black hole.\r\n• It is very dangerous depending on where it appears.\r\n• It is recommended to attack from a distance whenever possible.\r\n\r\n\r\n➼ Singularity Tank (Variant)\r\n• Pulls all nearby survivors and special infected towards itself.\r\n• Ranged attacks are recommended. If that’s not possible, it is recommended everyone should attack in melee at the same time.\r\n\r\n\r\n➼ Eventhorizon Tank (Overlord)\r\n• Rocks thrown by this tank create a temporary black hole on impact.\r\n• Has a black hole above it that pulls in survivors within range.\r\n• Getting too close can result in being launched backward or into the air.\r\n• Subtypes: Black Hole Tank (Common), Singularity Tank (Variant).\r\n• Avoid cliffs and black holes while attacking from a distance. If survivors fight it near a cliff, this tank can be extremely dangerous. So it is recommended to stay away from ledges."; break;
                    case "Bloodlust Tank": information = "➼ Bloodlust Tank (Common)\r\n• Heals a portion of it's health when dealing damage.\r\n• It passively restores a certain amount of health at intervals.\r\n• The lower it's health, the more enraged it becomes, increasing it's speed.\r\n• Molotovs and ranged attacks are recommended.\r\n\r\n\r\n➼ Bloodhunger Tank (Variant)\r\n• Regenerates 1% of it's health per second.\r\n• Absorbs health from nearby survivors and special infected, draining 1% of their health per second.\r\n• Moves noticeably faster than usual.\r\n• Molotovs, high-ground attacks, and extreme long-range attacks are recommended."; break;
                    case "Boomer Tank": information = "➼ Boomer Tank (Common)\r\n• Explodes upon taking damage, just like a Boomer. But continues to do so until it's dead.\r\n• If it damages a survivor, it covers them in vomit.\r\n• To avoid vomit effects, molotov and ranged attacks are recommended.\r\n\r\n\r\n➼ Corrupted Tank (Variant)\r\n• Vomits a long-range bile attack at intervals, making survivors and special infected a target for zombies.\r\n• Punches and rock throws also cause vomit effects.\r\n• Molotovs and high-ground attacks are recommended.\r\n\r\n\r\n➼ Cataclysm Tank (Overlord)\r\n• Creates four vomit explosion waves at different angles.\r\n• Punches and rock throws apply vomit effects.\r\n• Summons more durable Boomers at intervals.\r\n• Subtypes: Boomer Tank (Common), Corrupted Tank (Variant).\r\n• Since vomit explosions form in a cross pattern, it is recommended to avoid standing directly in front or beside the tank.\r\n• Do not attack from too far or too close. It is recommended to stay at a mid-range distance and avoid explosions."; break;
                    case "Death Tank": information = "➼ Death Tank (Common)\r\n• The slowest moving tank of all, but as it's name tells, it brings certain death.\r\n• On Easy difficulty, it incapacitates survivors in two hits.\r\n• On Normal difficulty, it incapacitates in one hit.\r\n• On Advanced and Expert difficulties, it kills in a single blow.\r\n• It is easy to kill, but depending on when or where it appears. It can be extremely dangerous.\r\n• If there is a large horde of zombies and this tank appears, it could mean the end for the entire team.\r\n• It is recommended to always attack from a distance.\r\n\r\n\r\n➼ Void Tank (Variant)\r\n• Kills instantly, regardless of difficulty level.\r\n• Slightly faster than Death Tank.\r\n• Survivors killed by this tank cannot be revived; their bodies disappear into the void.\r\n• Molotov and long-range attacks are recommended.\r\n\r\n\r\n➼ Soulrender Tank (Overlord)\r\n• Surrounded by two death orbs.\r\n• Touching an orb causes black-and-white vision and turns all health into temporary health.\r\n• Slowly pulls in nearby survivors, making escape difficult.\r\n• Attacks also inflict black-and-white vision.\r\n• Deals damage equal to 50% of a survivor’s max health, meaning it can kill in two hits without downing survivors.\r\n• Subtypes: Death Tank (Common), Void Tank (Variant).\r\n• It is recommended to attack from a very high ground or a very long-range."; break;
                    case "Earthquake Tank": information = "➼ Earthquake Tank (Common)\r\n• Causes an earthquake, dealing damage to everyone nearby.\r\n• Any attack is recommended.\r\n\r\n\r\n➼ Rupture Tank (Variant)\r\n• Constantly triggers ground-shaking attacks, causing destruction in its surroundings.\r\n• Molotovs and medium-range attacks are recommended."; break;
                    case "Enrage Tank": information = "➼ Enrage Tank (Common)\r\n• The damage it deals and it's speed increase as it's health drops in 20% intervals.\r\n• Molotov and ranged attacks are recommended.\r\n\r\n\r\n➼ Berserker Tank (Variant)\r\n• Moves extremely fast but slows down as it's health decreases.\r\n• Becomes more resistant to damage over time.\r\n• Deals more damage the longer the fight lasts.\r\n• Fast and continuous attacks are recommended."; break;
                    case "Explosive Tank": information = "➼ Explosive Tank (Common)\r\n• Causes moderate explosions in it's surroundings.\r\n• It's attacks apply a minor explosive effect to targets.\r\n• Any attack is recommended.\r\n\r\n\r\n➼ Nuke Tank (Variant)\r\n• Punches and rock throws cause explosions on impact.\r\n• Triggers massive explosions in it's surroundings, capable of instantly killing nearby survivors. So never get close to the tank.\r\n• Shooting this tank may cause it to create an enormous explosion.\r\n• Molotovs and ranged attacks are recommended.\r\n\r\n\r\n➼ Annihilator Tank (Overlord)\r\n• Has a chance to trigger a small explosion when damaged.\r\n• Periodically creates three rapid-fire explosions in the direction it is facing.\r\n• Rocks thrown by this tank cause huge explosions, potentially wiping out the entire team.\r\n• Subtypes: Explosive Tank (Common), Nuke Tank (Variant).\r\n• It is recommended to attack from very long-range or high ground."; break;
                    case "Fire Tank": information = "➼ Fire Tank (Common)\r\n• Burns everything and everyone in it's vicinity.\r\n• It is resistant to fire and takes no damage from it.\r\n• Any type of attack is recommended as long as you don’t get too close.\r\n\r\n\r\n➼ Hellfire Tank (Variant)\r\n• It is immune to fire.\r\n• Burns everything in it's surroundings.\r\n• Periodically regenerates health using fire energy.\r\n• Ranged attacks are recommended.\r\n\r\n\r\n➼ Inferno Tank (Overlord)\r\n• Absorbs fire-based attacks, regenerating 0.3% of it's max HP per second when on fire.\r\n• Subtypes: Fire Tank (Common), Hellfire Tank (Variant).\r\n• It is recommended to attack from a distance. This will be a massive fire show."; break;
                    case "Freezing Tank": information = "➼ Freezing Tank (Common)\r\n• Slows down nearby survivors.\r\n• Ranged attacks are recommended.\r\n\r\n\r\n➼ Hydrofrost Tank (Variant)\r\n• Slows down nearby survivors.\r\n• Extinguishes all fire effects instantly.\r\n• Throws a rock that creates a freezing zone on impact.\r\n• Unleashes waves of cold water, knocking survivors backward.\r\n• Survivors who stay too close are launched into the air.\r\n• Ranged and high-ground attacks are recommended.\r\n\r\n\r\n➼ Frostreaper Tank (Overlord)\r\n• Freezes all nearby survivors and infected, making them nearly immobile.\r\n• Instantly extinguishes all fire.\r\n• Upon death, releases a powerful water blast, knocking survivors away.\r\n• Subtypes: Freezing Tank (Common), Hydrofrost Tank (Variant).\r\n• If encountered up close, it is recommended for everyone to attack without stopping before it reaches you. If it gets too close, everyone should melee attack at once as a last resort."; break;
                    case "Illusion Tank": information = "➼ Illusion Tank (Common)\r\n• Has a chance to move very quickly when taking damage.\r\n• During this illusion movement, it can dodge bullets.\r\n• To minimize damage, it is recommended that everyone attack from different heights if possible. If not, it is recommended that everyone should engage in melee combat at the same time.\r\n\r\n\r\n➼ Delusion Tank (Variant)\r\n• Sends it's illusion to other survivors.\r\n• Periodically spawns multiple illusions in it's surroundings.\r\n• Moves too fast to escape.\r\n• It is recommended that everyone should engage in melee combat at the same time."; break;
                    case "Immortal Tank": information = "➼ Immortal Tank (Common)\r\n• Cannot be damaged.\r\n• It dies automatically after a set amount of time.\r\n• The time it takes to die depends on its current health capacity.\r\n• Each hit it lands extends it's lifespan by 3 seconds.\r\n• It is recommended to keep running away.\r\n\r\n\r\n➼ Samsara Tank (Variant)\r\n• Has multiple health bars (randomly between 1 and 3).\r\n• When it's current health bar reaches critical levels, it becomes temporarily invincible before switching to it's next health bar.\r\n• Each new health bar increases it's melee damage by 15%.\r\n• Any attack is recommended.\r\n\r\n\r\n➼ Eternaldread Tank (Overlord)\r\n• Each time it takes damage, it's immortality duration extends by 1 second.\r\n• Its immortality cannot exceed 60 seconds, but it resets with every attack it receives.\r\n• Subtypes: Immortal Tank (Common), Samsara Tank (Variant).\r\n• It is recommended to avoid attacking it. Just survive until it's time runs out. Or this will be an endless battle."; break;
                    case "Injury Tank": information = "➼ Injury Tank (Common)\r\n• Turns the health of everyone nearby into temporary health, spreading a disease.\r\n• When it lands a direct hit, the damage taken is also converted into temporary health.\r\n• It is recommended to attack from a certain distance using any weapon.\r\n\r\n\r\n➼ Infectious Tank (Variant)\r\n• Creates a massive infection cloud.\r\n• Damages survivors within the cloud and turns their health into temporary health, spreading disease.\r\n• Molotovs and ranged attacks are recommended."; break;
                    case "Lightning Tank": information = "➼ Lightning Tank (Common)\r\n• Can launch small lightning attacks on anyone nearby.\r\n• Also applies this effect when hitting a target.\r\n• Any type of attack is recommended, as long as you don’t get too close.\r\n\r\n\r\n➼ Thunderstorm Tank (Variant)\r\n• Launches powerful electric attacks.\r\n• Strikes a large area with lightning.\r\n• Molotovs and medium-range attacks are recommended.\r\n\r\n\r\n➼ Thunderwrath Tank (Overlord)\r\n• Unleashes powerful lightning strikes across a wide area.\r\n• Nearby survivors suffer electrical shock attacks.\r\n• Rocks thrown by this tank create an electric field on impact.\r\n• Subtypes: Lightning Tank (Common), Thunderstorm Tank (Variant).\r\n• It is recommended to attack from a distance."; break;
                    case "Mech Tank": information = "➼ Mech Tank (Common)\r\n• Uses aerial attacks.\r\n• Fires laser beams at everything nearby.\r\n• Periodically creates a laser turret that deals damage to a target within medium range.\r\n• Punches with extreme force, launching targets far away, possibly causing instant death.\r\n• Molotovs and ranged attacks are recommended.\r\n\r\n\r\n➼ Legion Tank (Variant)\r\n• Uses laser and anti-aircraft attacks.\r\n• Fires orbital strikes where it's rock lands.\r\n• Launches heavy aerial bombardments.\r\n• Extinguishes itself when burning.\r\n• It is recommended to keep moving and attack from the farthest possible distance.\r\n\r\n\r\n➼ Atherion Tank (Overlord)\r\n• Has 30% damage resistance.\r\n• It's punches send survivors flying across long distances.\r\n• Uses multiple aerial and orbital attacks.\r\n• Shoots a blue laser at a random survivor, causing a wide explosion.\r\n• Fires a purple laser that deals continuous damage on contact.\r\n• Unleashes a red laser that damages everything nearby and causes a small explosion.\r\n• Rocks thrown by this tank explode into multiple toxic missiles.\r\n• Missiles release poison clouds upon impact, causing slow health loss.\r\n• Subtypes: Mech Tank (Common), Legion Tank (Variant).\r\n• It is recommended to avoid lasers and explosions at all costs. Attack from the farthest distance possible."; break;
                    case "Meteor Tank": information = "➼ Meteor Tank (Common)\r\n• Calls down meteor showers on survivors at intervals.\r\n• It is recommended never to stop moving while attacking.\r\n\r\n\r\n➼ Doomsday Tank (Variant)\r\n• Creates meteor storms.\r\n• Covers large areas with meteor impacts.\r\n• Rocks thrown by this tank bring giant meteors from the sky.\r\n• The closer you are to the impact site, the more damage you take.\r\n• Molotovs and very long-range attacks are recommended.\r\n\r\n\r\n➼ Armageddon Tank (Overlord)\r\n• Creates a non-stop, large-scale meteor storm.\r\n• Has a chance to bring a massive meteor, which upon impact creates seven more large meteors.\r\n• Periodically targets random survivors with focused meteor showers.\r\n• Subtypes: Meteor Tank (Common), Doomsday Tank (Variant).\r\n• It is recommended to attack from the farthest distance possible."; break;
                    case "Necromancer Tank": information = "➼ Necromancer Tank (Common)\r\n• Affects all infected within a certain radius.\r\n• If the infected under it's effect are killed, they may turn into witches with half the normal health and 30% of their default damage.\r\n• If a large horde is present, killing zombies can result in a dangerous number of witches.\r\n• Special infected affected by this tank gain 50% extra health.\r\n• If you die while downed by this tank, you respawn as a random special infected.\r\n• If no horde is present, it acts like a normal tank.\r\n• If a large horde is nearby, it is recommended to throw a bile bomb far away and isolate the tank before engaging.\r\n• If nobody has a bile bomb, it is recommended to stay away from killed zombies and focus on the tank or the fight may spiral out of control.\r\n\r\n\r\n➼ Shaman Tank (Variant)\r\n• Periodically summons large hordes of zombies.\r\n• Curses nearby zombies, granting them special properties.\r\n• Different effects trigger when cursed zombies are killed, such as: explosion, fire burst, poison cloud, acid pool, transforming into a special infected.\r\n• It is recommended to use a bile bomb to redirect zombies far away before engaging.\r\n\r\n\r\n➼ Gravecaller Tank (Overlord)\r\n• Periodically summons large hordes of zombies.\r\n• Infected under its influence glow in different colors:\r\n• Red-glowing infected may turn into full-power witches upon death.\r\n• Yellow-glowing infected may turn into special infected upon death.\r\n• Special infected affected by this tank glow red and may trigger random effects upon death, such as: explosion, fire burst, acid pool.\r\n• Subtypes: Necromancer Tank (Common), Shaman Tank (Variant).\r\n• It is recommended to use a bile bomb to redirect zombies far away from the tank. If nobody has a bile bomb, one survivor should try luring the infected away while the others focus on the tank.\r\n• It is recommended to avoid fighting this tank while surrounded by infected. This will cause total chaos."; break;
                    case "Phantom Tank": information = "➼ Phantom Tank (Common)\r\n• It is hard to see this tank.\r\n• Causes permanent bleeding on targets, dealing 1 damage every 1.5 seconds.\r\n• Bleeding continues until the target has 1 HP left.\r\n• Bleeding can only be stopped with a medkit.\r\n• Molotovs and medium-range attacks are recommended.\r\n\r\n\r\n➼ Wraith Tank (Variant)\r\n• Almost invisible.\r\n• Becomes briefly visible after it lands an attack.\r\n• Immune to fire.\r\n• Causes survivors to enter black-and-white vision upon impact.\r\n• It is recommended to attack from a distance or high ground."; break;
                    case "Poison Tank": information = "➼ Poison Tank (Common)\r\n• Poisons targets upon impact, causing 1 HP loss per second for 5 seconds.\r\n• If a poisoned target is hit again, the poison’s duration increases by 5 seconds, up to a maximum of 15 seconds.\r\n• Nearby survivors also take 1 HP damage per second for 4 seconds if near a poisoned ally, but the poison does not spread further.\r\n• It is recommended to attack this tank without making physical contact.\r\n\r\n\r\n➼ Venomous Tank (Variant)\r\n• Creates two rotating poison orbs.\r\n• Contact with an orb inflicts poison, causing 2 HP loss per second for 7 seconds.\r\n• Tank attacks apply poison, causing 1 HP loss every 0.3 seconds for 4 seconds.\r\n• It is recommended to avoid touching the tank or it's orbs. Attack from a distance."; break;
                    case "Police Tank": information = "➼ Police Tank (Common)\r\n• Spawns 4 to 6 armored infected upon appearing.\r\n• At 50% health, throws a flashbang grenade, blinding survivors for 5 seconds.\r\n• Downed survivors are \"arrested\" and become trapped in place.\r\n• A downed survivor cannot be rescued until the tank or the survivor dies.\r\n• Any attack is recommended.\r\n\r\n\r\n➼ SWAT Tank (Variant)\r\n• Spawns 4 to 6 armored infected upon appearing.\r\n• Targets a random survivor with it's armored infected.\r\n• Downed survivors are \"arrested\" and trapped in place.\r\n• A downed survivor cannot be rescued until either the tank or the survivor dies.\r\n• At 75% health, spawns 4 to 6 more armored infected.\r\n• At 50% health, throws a flashbang grenade, blinding survivor for 5 seconds.\r\n• At 25% health, spawns more armored infected and throws another flashbang.\r\n• Long-range attacks are recommended."; break;
                    case "Reflector Tank": information = "➼ Reflector Tank (Common)\r\n• Uses a reflective shield at intervals that bounces back incoming damage to attackers.\r\n• Any attack is recommended when the reflective shield is inactive.\r\n\r\n\r\n➼ Disarm Tank (Variant)\r\n• Has a very high chance of knocking primary weapons out of survivors’ hands when shot.\r\n• Takes 60% less damage from primary weapons.\r\n• Takes 80% more damage from secondary weapons.\r\n• Melee or pistols are recommended for attacking.\r\n\r\n\r\n➼ Mirrorbane Tank (Overlord)\r\n• Has a reflective shield that returns damage back to the attacker, dealing 2 HP per hit.\r\n• To break its shield, use secondary weapons.\r\n• Regenerates its reflective shield 10 seconds after it is broken.\r\n• Subtypes: Reflector Tank (Common), Disarm Tank (Variant).\r\n• While the tank chases a survivor, others should break its shield using melee weapons or pistols(much better) from behind."; break;
                    case "Shield Tank": information = "➼ Shield Tank (Common)\r\n• Protects itself with a shield against incoming attacks.\r\n• Shoot the shield to break it.\r\n• Once the shield is broken, the tank becomes vulnerable to damage, but it will generate a new shield after 10 seconds.\r\n• If you attack the shield with a melee weapon, your weapon will be broken.\r\n• Close-range shotgun attacks are recommended.\r\n\r\n\r\n➼ Fortress Tank (Variant)\r\n• Reduces all damage taken by 70%, except for fire damage.\r\n• Molotovs and medium-range attacks are recommended.\r\n\r\n\r\n➼ Aegisbreaker Tank (Overlord)\r\n• Has an extremely durable shield.\r\n• Ignores all damage until the shield is broken.\r\n• Once it's broken, it cannot regenerate. But the tank still has 50% damage resistance.\r\n• Grants 90% damage resistance to all special infected around it.\r\n• Subtypes: Shield Tank (Common), Fortress Tank (Variant).\r\n• Close-range rapid fire shotgun attacks are recommended."; break;
                    case "Spawner Tank": information = "➼ Spawner Tank (Common)\r\n• Randomly spawns special infected at certain intervals.\r\n• Directs nearby zombies toward a random survivor.\r\n• Any attack is recommended as long as you don’t fight alone.\r\n\r\n\r\n➼ General Tank (Variant)\r\n• Periodically spawns large groups of special infected and directs them towards survivors.\r\n• Molotovs and long-range attacks are recommended.\r\n\r\n\r\n➼ Broodlord Tank (Overlord)\r\n• Rapidly spawns 6-12 special infected and 8-11 armored zombies at certain intervals.\r\n• Summons constant zombie hordes.\r\n• Special infected summoned by this tank are fire-resistant.\r\n• Subtypes: Spawner Tank (Common), General Tank (Variant).\r\n• It is recommended to focus on killing the tank as fast as possible. However, maintain distance.\r\n• It is also recommended to throw bile bomb if you have one. So the zombies will be redirected toward the tank."; break;
                    case "Steelweaver Tank": information = "➼ Steelweaver Tank (Common)\r\n• Surrounds itself with rotating swords, damaging anything nearby.\r\n• Periodically throws a group of blades in the direction it’s facing.\r\n• Any attack except melee is recommended.\r\n\r\n\r\n➼ Steelforge Tank (Variant)\r\n• Constantly throws swords and blades in the direction it is facing.\r\n• Where it's rock lands, a rain of blades follows.\r\n• It is recommended not to get too close and attack from a distance."; break;
                    case "Swift Tank": information = "➼ Swift Tank (Common)\r\n• Moves very quickly.\r\n• Molotov and ranged attacks are recommended.\r\n\r\n\r\n➼ Shift Tank (Variant)\r\n• Teleports to different survivors at intervals.\r\n• Rocks thrown by this tank hit it's target instantly.\r\n• Staying in constant motion while attacking is recommended.\r\n\r\n\r\n➼ Voidwalker Tank (Overlord)\r\n• Randomly teleports to different survivors at intervals.\r\n• Leaves an electric shock effect upon teleporting.\r\n• Has a 50% chance of teleporting again shortly after the first teleport.\r\n• Has a 40% chance of teleporting after landing an attack.\r\n• Does not throw rocks.\r\n• Subtypes: Swift Tank (Common), Shift Tank (Variant).\r\n• Gathering in one spot and attacking simultaneously with melee weapons is recommended."; break;
                    case "Thief Tank": information = "➼ Thief Tank (Common)\r\n• Steals a random item (except secondary weapons) when punching a target. Stolen items cannot be recovered.\r\n• Throws a rock that knocks an item out of the target’s inventory, but these items can be picked back up.\r\n• Since it can steal items by punching, it is recommended to stay at a distance when attacking.\r\n\r\n\r\n➼ Purloiner Tank (Variant)\r\n• Steals all items from a survivor except their secondary weapon upon punching them. Stolen items cannot be recovered.\r\n• Throws a rock that knocks all items out of the target’s inventory, but these items can be picked back up.\r\n• Since it steals all items on impact, it is recommended to attack from a distance."; break;
                    case "Trickster Tank": information = "➼ Trickster Tank (Common)\r\n• When taking damage, it can randomly swap the attacker's primary weapon (along with it's ammo) with another weapon.\r\n• Periodically gains temporary speed or heals itself for a random amount between 0.3% and 25% of its max health or fails to do either.\r\n• Molotovs and melee attacks are recommended.\r\n\r\n\r\n➼ Jester Tank (Variant)\r\n• Summons a group of clown zombies.\r\n• Randomly swaps the attacker’s primary weapon (along with it's ammo) with another weapon.\r\n• When it jumps high it drops all nearby survivors’ items (except secondary weapons).\r\n• Plays dead to launch surprise attacks.\r\n• Molotovs and melee attacks are recommended."; break;
                    case "Wind Tank": information = "➼ Wind Tank (Common)\r\n• Creates two wind orbs that push survivors away upon contact.\r\n• Disperses molotov flames in different directions.\r\n• Ranged attacks are recommended.\r\n\r\n\r\n➼ Tempest Tank (Variant)\r\n• Creates four wind orbs that push survivors away upon contact.\r\n• Releases four more wind orbs that inflict minor damage on contact.\r\n• Nearby survivors may be pushed away by strong winds.\r\n• Moves slightly faster than usual.\r\n• Ranged attacks are recommended."; break;
                }

                MessageBox.Show(information, $"Super Tanks Guide - {selectedTank}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("In order to view information of a super tank, you must select one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonGetPowerInformation_Click(object sender, EventArgs e)
        {
            if (listBoxPowers.SelectedIndex >= 0)
            {
                string information = $"Available Settings For {selectedPower} Power :\n\n";

                switch (selectedPower)
                {
                    case "Acid": information += "• Acid Droplet Damage\r\n• Acid Droplet Damage Interval\r\n• Acid Droplet Interval\r\n• Acid Interval\r\n• Corrosive Acid Nova Damage\r\n• Corrosive Acid Nova Damage Interval\r\n• Corrosive Acid Nova Interval"; break;
                    case "Admin": information += "• Abusive Command Interval\r\n• Admin Command Interval"; break;
                    case "Binder": information += "• Binder Effect Range\r\n• Binder Transfer Damage\r\n• Linker Effect Range"; break;
                    case "Black Hole": information += "• Black Hole AoE\r\n• Black Hole Interval\r\n• Overlord Black Hole AoE\r\n• Overlord Black Hole Damage\r\n• Overlord Black Hole Damage Interval\r\n• Singularity AoE"; break;
                    case "Bloodlust": information += "• Bloodhunger Drain Health Interval\r\n• Bloodhunger Health Drain AoE\r\n• Bloodhunger Health Regen Interval\r\n• Bloodhunger Max Health Drain Amount\r\n• Bloodhunger Recovery Max Health Percentage\r\n• Bloodlust Health Regen Cooldown\r\n• Bloodlust Health Regen Interval\r\n• Bloodlust Recovery Max Health Percentage\r\n• Bloodlust Regen Time"; break;
                    case "Boomer": information += "• Boomer Vomit Explode Chance\r\n• Corrupted Vomit Interval\r\n• Overlord Boomer Minion Health\r\n• Overlord Vomit Interval"; break;
                    case "Death": information += "• Death Move Speed"; break;
                    case "Earthquake": information += "• Earthquake Damage\r\n• Earthquake Interval\r\n• Rupture Seismic Wave Interval"; break;
                    case "Enrage": information += "• Berserker Attack Boost\r\n• Berserker Speed Reduction\r\n• Enrage Attack Boost\r\n• Enrage Speed Boost"; break;
                    case "Explosive": information += "• Explosion Damage\r\n• Explosion Interval\r\n• Nuke Explosion AoE\r\n• Nuke Explosion Damage\r\n• Nuke Explosion Interval\r\n• Nuke Rock Explosion AoE\r\n• Nuke Rock Explosion Damage\r\n• Overlord Explosion Damage\r\n• Overlord Moving Explosion Damage\r\n• Overlord Moving Explosion Interval\r\n• Overlord Rock Explosion Damage"; break;
                    case "Fire": information += "• Fire Tank Burning Ground Interval\r\n• Flame Cloack Damage\r\n• Flame Cloack Interval\r\n• Hellfire Flame Pillar Interval\r\n• Overlord Fire Rains Damage\r\n• Overlord Fire Rains Damage Interval\r\n• Overlord Fire Rains Interval\r\n• Overlord Flame Cloack Interval\r\n• Overlord Flame Spin Damage\r\n• Overlord Flame Spin Damage Interval\r\n• Rock Fire Damage\r\n• Rock Fire Damage Inerval\r\n• Rock Fire Duration"; break;
                    case "Freezing": information += "• Hydro Circular Bomb Damage\r\n• Hydro Circular Bomb Interval\r\n• Hydro Push Interval\r\n• Overlord Freezing Range\r\n• Overlord Hydro Mine Damage"; break;
                    case "Illusion": information += "• Delusion Skill Interval\r\n• Illusion Speed"; break;
                    case "Immortal": information += "• Base Health\r\n• Immortal Base Timer\r\n• Immortal Timer Difficulty Multiply\r\n• Overlord Base Timer\r\n• Samsara Lives"; break;
                    case "Injury": information += "• Black Fog Damage\r\n• Black Fog Interval\r\n• Red Fog Damage\r\n• Red Fog Damage Interval\r\n• Red Fog Infected Health Chance\r\n• Red Fog Interval"; break;
                    case "Lightning": information += "• Electric Mine Damage\r\n• Electric Mine Interval\r\n• Lightning Strike Damage\r\n• Lightning Strike Interval\r\n• Overlord Electrocute Damage\r\n• Overlord Electrocute Interval\r\n• Overlord Lightning Strike Damage\r\n• Overlord Lightning Strike Interval\r\n• Overlord Thunderstorm Damage\r\n• Overlord Thunderstorm Interval\r\n• Zap Damage\r\n• Zap Interval"; break;
                    case "Mech": information += "• Air Strike Interval\r\n• Legion Flak Tower Interval\r\n• Legion Laser Tower Interval\r\n• Mech Laser Tower Interval\r\n• Overlord Orbital Cannon AoE\r\n• Overlord Orbital Cannon Damage\r\n• Overlord Orbital Cannon Interval\r\n• Overlord Orbital Heatwave Damage\r\n• Overlord Orbital Heatwave Interval\r\n• Overlord Orbital Laser Damage\r\n• Overlord Orbital Laser Interval\r\n• Red Laser Damage\r\n• Red Laser Interval"; break;
                    case "Meteor": information += "• Doomsday Meteor Rain Interval\r\n• Huge Meteor Damage\r\n• Meteor Count\r\n• Meteor Damage\r\n• Meteor Interval\r\n• Overlord Meteor Rain Interval"; break;
                    case "Necromancer": information += "• Necro Witch Health Reduction\r\n• Necro Witch damage\r\n• Necromancer Effect Range"; break;
                    case "Phantom": information += "• Phantom Tank Bleed Interval\r\n• Wraith Tank Visible Time"; break;
                    case "Poison": information += "• Max Poison Duration\r\n• Poison Damage\r\n• Poison Damage Interval\r\n• Poison Duration\r\n• Poison Spread Range\r\n• Venom Ball Poison Damage\r\n• Venom Ball Poison Duration\r\n• Venom Tank Poison Damage\r\n• Venom Tank Poison Duration\r\n• Venom Tank Poison Interval"; break;
                    case "Police": information += "• Police Tank Flash Brightness"; break;
                    case "Reflector": information += "• Overlord Reflect Damage\r\n• Reflector Reflect Damage\r\n• Reflector Shield Interval"; break;
                    case "Shield": information += "• Fortress Damage Reduction\r\n• Overlord SI Damage Reduction\r\n• Overlord Shield Point\r\n• Shield Cooldown\r\n• Shield Point"; break;
                    case "Spawner": information += "• General Max Minions\r\n• General Spawn Minions Interval\r\n• Overlord Spawn Minions Interval\r\n• Spawner Max Minions\r\n• Spawner Spawn Minions Interval"; break;
                    case "Steelweaver": information += "• Forge Flying Weapons Damage\r\n• Forge Flying Weapons Interval\r\n• Spin Swords Damage\r\n• Weaver Flying Weapons Damage\r\n• Weaver Skill Interval"; break;
                    case "Swift": information += "• Move Speed\r\n• Overlord Attack Teleport Chance\r\n• Overlord Lightning Damage\r\n• Overlord Teleport Interval\r\n• Shift Teleport Interval"; break;
                    case "Wind": information += "• Tempest Wind Current Chain\r\n• Tempest Wind Current Damage\r\n• Tempest Wind Current Interval\r\n• Wind Tank Current Interval"; break;
                }

                MessageBox.Show(information, $"Powers Guide - {selectedPower}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("In order to view information of a power, you must select one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonClearSelections_Click(object sender, EventArgs e)
        {
            listBoxTanks.SelectedIndex = -1;
            listBoxPowers.SelectedIndex = -1;
        }

        private void ButtonBrowseModPage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would you like to browse the mod page?", "Mod Page", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=2945298750");
            }
        }

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("• Credits\r\n  • Everice (Creator of Super Tanks Mod)\r\n  • ₦Ɇ₲₳₦ (Programmer of Super Tanks Management System)\r\n\r\n═════════════════════════════════\r\n\r\n• General settings in 'general_setting.txt' can be edited.\r\n\r\n• Base settings of a specific tank in 'tank_setting.txt' can be edited after selecting a tank from the list of super tanks on the left side.\r\n\r\n• Base settings of every tank in 'tank_setting.txt' can be edited together.\r\n\r\n• Meanings of base settings and general settings can be viewed by clicking on their titles.\r\n\r\n• Power settings in 'tank_power_setting' can be edited after selecting a power from the list of powers on the right side.\r\n\r\n• Explanations for tanks can be viewed after selecting a tank from the list of super tanks on the left side. Or simply double click on tank. But it is best to read the information you need from the mod guide on steam. Because the future changes of the mod might be applied on app's content later.\r\n\r\n• Power settings of powers in 'tank_power_setting' can be viewed after selecting a power from the list of powers on the right side. Or simply double click on power.\r\n\r\n• Selected titles from both lists can be reset.\r\n\r\n• The mod configuration files can also be viewed by right clicking on one of these options: General Settings, All Base Settings, Power Settings.\r\n\r\n• Mod page can be viewed through default browser of your computer.\r\n\r\n═════════════════════════════════\r\n\r\n• If you need more details, the complete guide for this mod can be viewed through default browser of your computer by clicking the 'Yes' option below.", "Available Features", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=3426565363");
            }
        }

        private void ButtonChangeAllBaseSettings_MouseDown(object sender, MouseEventArgs e)
        {
            ViewTextFile(filePath, "tank_setting", e);
        }

        private void ButtonChangeGeneralSettings_MouseDown(object sender, MouseEventArgs e)
        {
            ViewTextFile(filePath.Replace("tank_setting", "general_setting"), "general_setting", e);
        }

        private void ButtonChangePowerSettings_MouseDown(object sender, MouseEventArgs e)
        {
            ViewTextFile(filePath.Replace("tank_setting", "tank_power_setting"), "tank_power_setting", e);
        }
    }
}