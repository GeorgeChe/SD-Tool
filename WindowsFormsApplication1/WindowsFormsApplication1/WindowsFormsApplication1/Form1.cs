using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public partial class SD : Form
    {
        //Global Stuff
        int processId = 0; // counts how many processes are running
        public string selectedDomainController; // selected domain controller;
        string loggedUser = "";
        bool isUserLockedOut; // False if the account is not locked and true if the user is locked out. Global var updated from the GetUserDetails method.
        public static string userDetailsName;
        public static string SamAccountName;
        public static string userManagerName;


        static string theDirectory = AppDomain.CurrentDomain.BaseDirectory + @"MachineInfo.psm1";
        static string the2ndDir = "'" + theDirectory + "'";
        [ThreadStatic]
        string powerShellInput;

        public SD()
        {
            InitializeComponent();
            loggedUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToUpper(); // Get the user id logged on the machine
            CheckUser(loggedUser);
            getDomainControllers();
            this.Text += " " + loggedUser;
        }
        void CheckUser(string user)
        {
            user = loggedUser.Substring(4).ToUpper();
            string groupName = "UK IT Support Romania";
            List<string> usersList = new List<string>();
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);
            GroupPrincipal group = GroupPrincipal.FindByIdentity(principalContext, groupName);
            if (group != null)
            {
                foreach (Principal p in group.GetMembers(true))
                {
                    usersList.Add(p.SamAccountName);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong with the AD search!");
            }
            bool chosenOne = false;
            foreach (var users in usersList)
            {
                if (users.ToUpper() == user)
                {
                    chosenOne = true;
                }
            }
            if (!chosenOne)
            {
                MessageBox.Show("YOU ARE NOT THE CHOSEN ONE!");
                Environment.Exit(1);
            }
            else if (chosenOne)
            {
                //MessageBox.Show("YOU ARE THE CHOSEN ONE!");
            }
            group.Dispose();
            principalContext.Dispose();
        }
        void CheckStatus()
        {
            if (processId > 0)
            {
                pictureBox1.Visible = true;
                label17.Text = "Running";
            } else if (processId < 1)
            {
                pictureBox1.Visible = false;
                label17.Text = "Idle";
            }
        }
        string RunPowerShell(string command)
        {
            string comm = command;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            string doStuff = String.Format(@"Import-Module {0}; {1}", the2ndDir, comm);
            startInfo.Arguments = doStuff;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.Close();
            return output;
        }
        private string ReturnText(string result)
        {
            processId--;
            string makeSpace = "***************************************************************************";
            textBox1.Text = makeSpace + "\n" + result + "\n" + makeSpace + textBox1.Text;
            CheckStatus();
            return result;
        }
        private async void MakeTheWorldBurnAsync(int button)
        {
            processId++;
            string computerName = textBox2.Text;
            switch (button)
            {
                case 1:
                    string text = await Task.Run(() => RunPowerShell(String.Format("Get-WindowsInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button1.Enabled = true;
                    break;
                case 2:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-DriverInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button2.Enabled = true;
                    break;
                case 3:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-NICInfo -ComputerName {0}", computerName))); /// cred ca asta e pentru Used Net Adaptors
                    ReturnText(text);
                    this.button3.Enabled = true;
                    break;
                case 4:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-ModelInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button4.Enabled = true;
                    break;
                case 5:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-BiosInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button5.Enabled = true;
                    break;
                case 6:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-ProcessorInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button6.Enabled = true;
                    break;
                case 7:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-MemoryInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button7.Enabled = true;
                    break;
                case 8:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-HDDInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button8.Enabled = true;
                    break;
                case 9:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-ConnectedNic -ComputerName {0}", computerName))); // Cred ca asta e pentru installed net adaptors
                    ReturnText(text);
                    this.button9.Enabled = true;
                    break;
                case 10:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-GPReport -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button10.Enabled = true;
                    break;
                case 11:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-EventLogInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button11.Enabled = true;
                    break;
                case 12:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-ServiceInfo -ComputerName {0}", computerName)));
                    ReturnText(text);
                    this.button12.Enabled = true;
                    break;
                case 13:
                    text = await Task.Run(() => RunPowerShell(String.Format("Get-PartitionInfo -ComputerName {0}", computerName))); // cred ca asta e pentru full machie report 
                    ReturnText(text);
                    this.button13.Enabled = true;
                    break;
                case 14:
                    text = await Task.Run(() => RunPowerShell(String.Format("ping {0}", computerName)));
                    ReturnText(text);
                    this.button14.Enabled = true;
                    break;
                case 15:
                    GetTagBasedOnUser userTag = new GetTagBasedOnUser();
                    userTag.MakeTagInfoFolder();
                    await Task.Run(() => userTag.ParallelForLoop());
                    this.aButton_search_DB.Enabled = true;
                    processId--;
                    CheckStatus();
                    break;
            }
        }
        private void getDomainControllers()
        {
            List<string> DC_IPs = new List<string>();
            Domain domain = Domain.GetCurrentDomain();
            // here we get the current domain controller to update the Current DC: label.
            string currentDC = domain.FindDomainController().ToString();
            currentDC = FormatDomainControllerName(currentDC);
            current_dc_lbl.Text += currentDC;
            foreach (DomainController dc in domain.FindAllDiscoverableDomainControllers())
            {
                string temp = dc.Name;
                temp = FormatDomainControllerName(temp);
                DC_IPs.Add(temp);
            }
            DC_IPs.Sort();
            dc_comboBox.Items.Clear();
            dc_comboBox.Items.AddRange(DC_IPs.ToArray());
            dc_comboBox.SelectedIndex = 14;
        }
        private string FormatDomainControllerName(string nameOfDomainController)
        {
            nameOfDomainController = nameOfDomainController.ToUpper();
            int length = nameOfDomainController.IndexOf(".");
            nameOfDomainController = nameOfDomainController.Substring(0, length);
            return nameOfDomainController;
        }
        private void GetUserDetails()
        {
            SamAccountName = user_box.Text;
            if(user_box.Text == string.Empty)
            {
                SamAccountName = "abc";
            }
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain,selectedDomainController);
            current_dc_lbl.Text = string.Empty;
            current_dc_lbl.Text = "Current DC:" + FormatDomainControllerName(selectedDomainController);
            UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, SamAccountName);
            DirectoryEntry dEntry = new DirectoryEntry();

            if (user == null)
            {
                ClearUserTab();
                description_box.Text = "Wrong user or user doesn't exist!";
            }
            else
            {
                dEntry = (DirectoryEntry)user.GetUnderlyingObject();
            }
            if (user != null)
            {
                userDetailsName = user.DisplayName; // prepare the name of the user for the password reset email
                display_name_box.Text = user.DisplayName;
                description_box.Text = user.Description;
                if ((dEntry.Properties["physicalDeliveryOfficeName"].Value) != null)
                    office_box.Text = dEntry.Properties["physicalDeliveryOfficeName"].Value.ToString();
                else
                    office_box.Text = "N/A";
                if (dEntry.Properties["manager"].Value != null)
                {
                    string manager = dEntry.Properties["manager"].Value.ToString();
                    int Length = manager.IndexOf(',');
                    manager_box.Text = manager.Substring(3, Length - 3);
                }else
                {
                    manager_box.Text = "N/A";
                }
                // Important for the PasswordResetEmail
                userManagerName = manager_box.Text;
                //prepare the name of the user manager for the password reset email
                if (dEntry.Properties["homeDirectory"].Value != null)
                {
                    home_drive_box.Text = dEntry.Properties["homeDirectory"].Value.ToString();
                }

                last_logon_box.Text = user.LastLogon.ToString();
                isUserLockedOut = user.IsAccountLockedOut();
                if(user.PasswordNeverExpires == true)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                if (isUserLockedOut)
                {
                    unlockAccountBtn.Enabled = true;
                    DateTime lockedout = user.AccountLockoutTime.Value;
                    lockout_status_box.BackColor = Color.GhostWhite;
                    lockout_status_box.ForeColor = Color.Red;
                    lockout_status_box.Text = "Account Locked!";
                    TimeSpan temps = DateTime.Now.Subtract(lockedout);
                    lockout_time_box.Text = temps.Days.ToString() + " days, " + temps.Hours.ToString() + " hours, " + temps.Minutes.ToString() + " minutes.";
                }
                else
                {
                    unlockAccountBtn.Enabled = false;
                    lockout_status_box.BackColor = Color.GhostWhite;
                    lockout_status_box.ForeColor = Color.Black;
                    lockout_status_box.Text = "Account Not Locked!";
                    lockout_time_box.Text = string.Empty;
                }

                if (user.Enabled == true)// Check if account is enabled
                {
                    account_status_box.BackColor = Color.GhostWhite;
                    account_status_box.ForeColor = Color.Black;
                    account_status_box.Text = "Account enabled!";
                    disabel_btn.Text = "Disable Account";
                }
                else
                {
                    account_status_box.BackColor = Color.GhostWhite;
                    account_status_box.ForeColor = Color.Red;
                    account_status_box.Text = "Account disabled!";
                    disabel_btn.Text = "Enable Account";
                }
                if (user.LastPasswordSet.HasValue)
                {
                    DateTime tmp = user.LastPasswordSet.Value;
                    TimeSpan tmps = DateTime.Now.Subtract(tmp);
                    password_box.Text = tmps.Days.ToString() + " days, " + tmps.Hours.ToString() + " hours, " + tmps.Minutes.ToString() + " minutes.";
                }

                // sa-ti arate cate zile au trecut decand s-a schimbat parola!!!
                creation_date_box.Text = dEntry.Properties["whenCreated"].Value.ToString();
                last_modified_box.Text = dEntry.Properties["whenChanged"].Value.ToString();
                ad_path_box.Text = user.DistinguishedName;
                //MemberOf ComboBox
                //int groupsNo = dEntry.Properties["memberOf"].Count;
                List<string> groups = new List<string>();
                foreach (GroupPrincipal group in user.GetGroups())
                {
                    groups.Add(group.ToString());
                }
                memberof_comboBox.Items.Clear();
                memberof_comboBox.Items.Insert(0, "- Press dropdown to see - ");
                memberof_comboBox.Items.AddRange(groups.ToArray());
                memberof_comboBox.SelectedIndex = 0;
                user.Dispose();
            }
            principalContext.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(1);
            CheckStatus();
            this.button1.Enabled = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(2);
            CheckStatus();
            this.button2.Enabled = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(3);
            CheckStatus();
            this.button3.Enabled = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(4);
            CheckStatus();
            this.button4.Enabled = false;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(5);
            this.button5.Enabled = false;
            CheckStatus();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(6);
            CheckStatus();
            this.button6.Enabled = false;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(7);
            CheckStatus();
            this.button7.Enabled = false;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(8);
            CheckStatus();
            this.button8.Enabled = false;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(9);
            CheckStatus();
            this.button9.Enabled = false;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(10);
            CheckStatus();
            this.button10.Enabled = false;
        }
        private void button11_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(11);
            CheckStatus();
            this.button11.Enabled = false;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(12);
            CheckStatus();
            this.button12.Enabled = false;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(13);
            CheckStatus();
            this.button13.Enabled = false;
        }
        private void button14_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(14);
            CheckStatus();
            this.button14.Enabled = false;
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void User_Tab_Click(object sender, EventArgs e)
        {

        }
        private void button15_Click(object sender, EventArgs e)
        {
            GetUserDetails();
        }
        private void user_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button15_Click(this, new EventArgs());
                aButton_Search_TagInfo_Click(this, new EventArgs());
            }
        }
        private void dc_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedDomainController = dc_comboBox.SelectedItem.ToString();
            selectedDomainController += ".sch.com";
            selectedDomainController = selectedDomainController.ToLower();
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button14_Click(this, new EventArgs());
            }
        }
        private void unlockAccountBtn_Click(object sender, EventArgs e)
        {
            if(this.unlockAccountBtn.Enabled == true)
            {
                Process.Start("http://b2bwebtest/Scc.ITSM.Admin/Home/UnlockAccounts");
            }
        }
        private void ps_input_tb_KeyDown(object sender, KeyEventArgs e)
        {
            powerShellInput = ps_input_tb.Text;
            if (e.KeyCode == Keys.Enter)
            {
                ps_input_tb.ReadOnly = true;


                Task<string> runPS = Task.Run(() =>
                {
                    return RunPowerShell(powerShellInput);
                }).ContinueWith((r) =>
                {
                    return r.Result;
                });
                ps_output_tb.Text = runPS.Result;
                ps_input_tb.ReadOnly = false;
            }
        }
        private void getIPAddrBtn_Click(object sender, EventArgs e)
        {
            GetIpAddress form = new GetIpAddress();
            form.Show(); // or form.ShowDialog(this);
        }
        private void cmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.WorkingDirectory = @"C:\";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
        private void regeditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "regedit.exe";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
        private void powerShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "powershell.exe";
            startInfo.WorkingDirectory = @"C:\";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
        private void computerManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "compmgmt.msc";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "devmgmt.msc";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }
        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            SamAccountName = user_box.Text;
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, selectedDomainController);
            UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, SamAccountName);
            

            if (checkBox1.Checked)
            {
                user.PasswordNeverExpires = true;
                user.Save();
            }
            if (!checkBox1.Checked)
            {
                user.PasswordNeverExpires = false;
                user.Save();
            }
            user.Dispose();
            principalContext.Dispose();
        }

        private void ClearUserTab()
        {
            //user_box.Text = string.Empty;
            display_name_box.Text = string.Empty;
            description_box.Text = string.Empty;
            office_box.Text = string.Empty;
            manager_box.Text = string.Empty;
            home_drive_box.Text = string.Empty;
            last_logon_box.Text = string.Empty;
            lockout_status_box.Text = string.Empty;
            lockout_time_box.Text = string.Empty;
            account_status_box.Text = string.Empty;
            password_box.Text = string.Empty;
            creation_date_box.Text = string.Empty;
            last_modified_box.Text = string.Empty;
            ad_path_box.Text = string.Empty;
        }

        private void aClear_btn_Click(object sender, EventArgs e)
        {
            if (tabControl2.SelectedIndex == 0)
            {
                ClearUserTab();
                user_box.Text = string.Empty;
            }
            else if(tabControl2.SelectedIndex == 1)
            {
                   textBox1.Text = string.Empty;
            }
            else if (tabControl2.SelectedIndex == 2)
            {
                ps_input_tb.Text = string.Empty;
                ps_output_tb.Text = string.Empty;
            }
        }
        private void disable_btn_Click(object sender, EventArgs e)
        {
            SamAccountName = user_box.Text;
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, selectedDomainController);
            UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, SamAccountName);

            if (user.Enabled == true)
            {
                user.Enabled = false;
                user.Save();
            }
            else
            {
                user.Enabled = true;
                user.Save();
            }
            GetUserDetails();
            user.Dispose();
            principalContext.Dispose();
        }
        public string RandomPasswordGenerator()
        {
            Random randomNumberGenerator = new Random();
            string[] words = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "January",
                "February", "March", "April", "August", "September", "October", "November", "December"};
            int randomMonth = randomNumberGenerator.Next(0, words.Length);
            int randomNumber = randomNumberGenerator.Next(0, 31);
            string generatedPassword = words[randomMonth] + randomNumber.ToString();
            for (int i = generatedPassword.Length; i < 9; i++)
            {
                generatedPassword += randomNumberGenerator.Next(0, 31).ToString();
            }
            return generatedPassword;
        }
        //*******************************  PASSWORD RESTE BUTTON *****************************************
        private void button18_Click(object sender, EventArgs e)
        {
            ResetPassword form = new ResetPassword();
            form.Show();
        }
        //*******************************  PASSWORD RESTE BUTTON *****************************************
        // Update description!
        private void button16_Click(object sender, EventArgs e)
        {
            SamAccountName = user_box.Text;
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, selectedDomainController);
            UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, SamAccountName);
            user.Description = description_box.Text;
            user.Save();
            GetUserDetails();
            user.Dispose();
            principalContext.Dispose();
        }

        private void tabControl2_Selected(object sender, TabControlEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aButton_search_DB_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(15);
            CheckStatus();
            this.aButton_search_DB.Enabled = false;
        }

        private void aButton_Search_TagInfo_Click(object sender, EventArgs e)
        {
            if (user_box.Text != string.Empty)
            {
                GetTagBasedOnUser userTag = new GetTagBasedOnUser();
                
                aShowTag_tb.Text = userTag.ContineRezultat(user_box.Text);
            }
            else
                description_box.Text = "Please enter a username!";
        }
    }
}
