using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace WindowsFormsApplication1
{
    public partial class ResetPassword : Form
    {
        string userNewPassword;
        bool userMustChangePasswordAtNextLogin;
        SD mainClass = new SD();

        public ResetPassword()
        {
            InitializeComponent();
        }
        public void sendEmailWithTheNewPassword()
        {
            SD mainClass = new SD();

            Outlook.Application outlookApp = new Outlook.Application();
            Outlook._MailItem oMailItem = (Outlook._MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
            string manager = SD.userManagerName;
            if (manager != string.Empty)
            {
                int Length = manager.IndexOf(' ');
                manager = manager.Substring(0, Length);
            }
            oMailItem.To = SD.userManagerName;
            oMailItem.Subject = "New network account password for " + SD.userDetailsName;
            oMailItem.HTMLBody = $"<html><title></title><body style='font-family:Calibri;font size=11;font-style:italic;'><p> Dear {manager},</p><p> We received a request for a Password Reset from {SD.userDetailsName}.</p></p>Please find below the requested details:<p><p> Username:<span style = 'color: red;'>{SD.SamAccountName} </span ></p><p> Password: <span style = 'color: red;'>{userNewPassword} </span ></p>We kindly advise to forward these details to {SD.userDetailsName} in order for the request to be complete.</p></p>Should you have any questions, please contact the Service Desk on 0121 281 8600 or It.Support@scc.com<p></p><p></p></body></html>" + "\n \n" + ReadSignature();
            oMailItem.Display(true);
        }
        private string ReadSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);

            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");

                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                    }
                }
            }
            return signature;
        }

        private void aCheckPasswordMustChange_MouseClick(object sender, MouseEventArgs e)
        {
            if (aCheckPasswordMustChange.Checked)
            {
                userMustChangePasswordAtNextLogin = true;
            }
            if (!aCheckPasswordMustChange.Checked)
            {
                userMustChangePasswordAtNextLogin = false;
            }
        }

        private void aPasswordResetButton_Click(object sender, EventArgs e)
        {
            string SamAccountName = SD.SamAccountName;
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, mainClass.selectedDomainController);
            UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, SamAccountName);
            userNewPassword = mainClass.RandomPasswordGenerator();
            user.SetPassword(userNewPassword);
            if (userMustChangePasswordAtNextLogin)
            {
                user.ExpirePasswordNow();
            }
            //GetUserDetails();
            user.Dispose();
            principalContext.Dispose();
            sendEmailWithTheNewPassword();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
