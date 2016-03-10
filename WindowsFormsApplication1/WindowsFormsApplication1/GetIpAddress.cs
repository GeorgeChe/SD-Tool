using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;

namespace WindowsFormsApplication1
{
    public partial class GetIpAddress : Form
    {
        public GetIpAddress()
        {
            InitializeComponent();
            GetListOfDomainControllers();
        }
        string[,] DC_IPs = new string[30, 2];
        Domain domain = Domain.GetCurrentDomain();
        int i = 0;
        private void GetListOfDomainControllers()
        {
            foreach (DomainController dc in domain.FindAllDiscoverableDomainControllers())
            {
                string temp = dc.Name.ToUpper();
                int length = temp.IndexOf(".");
                DC_IPs[i, 0] = dc.Name.ToUpper().Substring(0, length);
                DC_IPs[i, 1] = dc.IPAddress;
                i++;
            }
        }
        private string getIPfromspecificDCs(string tagno, int DCno)
        {
            string toReturn = string.Empty;
            try
            {
                var Options = new JHSoftware.DnsClient.RequestOptions();
                Options.DnsServers = new IPAddress[] {IPAddress.Parse(DC_IPs[DCno, 1])};
                var IPs = JHSoftware.DnsClient.LookupHost(tagno + ".sch.com",JHSoftware.DnsClient.IPVersion.IPv4, Options);
                foreach (var IP in IPs)
                {
                    Ping pingSender = new Ping();
                    IPAddress address = IP;
                    PingReply reply = pingSender.Send(address, 200);
                    string pingreply = "NO";
                    if (reply.Status == IPStatus.Success)
                        pingreply = "YES";
                    toReturn += DC_IPs[DCno, 0] + "\t" + IP.ToString() + "\t" + pingreply + "\n";
                }
            }
            catch (JHSoftware.DnsClient.NoDefinitiveAnswerException exceptie)
            {
                MessageBox.Show(exceptie.Message);
                for (int i = 0; i < exceptie.ServerProblems.Count; i++)
                    MessageBox.Show(exceptie.ServerProblems[i].ProblemDescription);
            }
            catch (JHSoftware.DnsClient.NXDomainException exceptie)
            {
                MessageBox.Show(exceptie.Message);
            }

            return toReturn;
        }
        private void GO()
        {
            ResultsrichTextBox.Clear();
            ResultsrichTextBox.AppendText("Domain Controller\tIP Address\tPing Reply?\n\n");
            bool ok = true;
            string tag = string.Empty;
            int tagno;
            if (tagNOtextBox.Text == string.Empty || tagNOtextBox.Text == null)
            {
                MessageBox.Show("Tag no field is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ok = false;
            }
            else if (int.TryParse(tagNOtextBox.Text, out tagno))
                if (tagno > 1000 && tagno < 99999)
                    tag = "tag-" + tagno.ToString(); //userul a introdus doar cifre
                else
                {
                    MessageBox.Show("Tag no not in correct range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ok = false;
                }
            else if (!(tagNOtextBox.Text.Length > 7) || !(tagNOtextBox.Text.Substring(0, 4).ToLower() == "tag-"))
            {
                MessageBox.Show("Tag no is incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ok = false;
            }
            else
                tag = tagNOtextBox.Text;

            if (ok)
            {
                //testing on JH to see if we get error. if we do, most likely the machine doesn;t have an ip allocated.
                string test = getIPfromspecificDCs(tag, 0);
                if (test == string.Empty || test == null)
                    MessageBox.Show("TAG might not have an IP allocated or not on domain!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    ResultsrichTextBox.AppendText(getIPfromspecificDCs(tag, 0));
                    for (int i = 1; i < 22; i++)
                        ResultsrichTextBox.AppendText(getIPfromspecificDCs(tag, i));
                }
            }
        }

        private void tagNOtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GO();
            }
        }
    }
}
