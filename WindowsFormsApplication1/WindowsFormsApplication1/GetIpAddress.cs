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
        string[,] DC_IPs = new string[30, 30];
        Domain domain = Domain.GetCurrentDomain();
        int i = 0;
        private void GetListOfDomainControllers()
        {
            foreach (DomainController dc in domain.FindAllDiscoverableDomainControllers())
            {
                DC_IPs[i, 0] = dc.Name;
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
                Options.DnsServers = new System.Net.IPAddress[] {
                    System.Net.IPAddress.Parse(DC_IPs[DCno, 1]),
                    };
                var IPs = JHSoftware.DnsClient.LookupHost(tagno + ".sch.com",
                                                          JHSoftware.DnsClient.IPVersion.IPv4, Options);
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
    }
}
