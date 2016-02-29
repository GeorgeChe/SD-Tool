using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class SD : Form
    {
        static string theDirectory = AppDomain.CurrentDomain.BaseDirectory + @"MachineInfo.psm1";
        static string the2ndDir = "'" + theDirectory + "'";

        public SD()
        {
            InitializeComponent();
        }
        static string RunPowerShell(string command)
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
            string makeSpace = "***************************************************************************";
            textBox1.Text = makeSpace+"\n"+result+"\n"+makeSpace+textBox1.Text;
            pictureBox1.Visible = false; // face loading bar-ul sa dispara
            return result;
        }
        private async Task MakeTheWorldBurnAsync(int button)
        {
            pictureBox1.Visible = true;
            string computerName = textBox2.Text;
            switch (button)
            {
                case 1:
                   // fa o noua metoda care sa fie apelata doar daca butonul pe a fost apasat devinde din nou true
                   // >> metoda trebuie sa schimbe intre running si idle si cercul care se roteste
                   // also sa faca o lista cu numele comenzilor care le ruleaza
                   //
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
                    text = await Task.Run(() => RunPowerShell(String.Format("ping {0}",computerName)));
                    ReturnText(text);
                    this.button14.Enabled = true;
                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(1);
            this.button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(2);
            this.button2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(3);
            this.button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(4);
            this.button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(5);
            this.button5.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(6);
            this.button6.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(7);
            this.button7.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(8);
            this.button8.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(9);
            this.button9.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(10);
            this.button10.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(11);
            this.button11.Enabled = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(12);
            this.button12.Enabled = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(13);
            this.button13.Enabled = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MakeTheWorldBurnAsync(14);
            this.button14.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void User_Tab_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
