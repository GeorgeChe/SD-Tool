using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class GetTagBasedOnUser
    {
        public string SearchExcelFile(string userDisplayName)
        {
            string tagNumber = " ";
            var fullFilePath = @"C:\test\cmdb_ci_computer.xls";   // Path to the file

            var application = new Microsoft.Office.Interop.Excel.Application();
            var workBook = application.Workbooks.Open(fullFilePath);        // Open the file
            var inputWorkSheet = (Worksheet)application.Worksheets[1];      // Select the 1st sheet

            var rowCnt = application.ActiveSheet.UsedRange.Rows.Count;      // Get number of rows
            var colCnt = application.ActiveSheet.UsedRange.Columns.Count;   // Get number of columns

            int maxNumRow = rowCnt;
            int maxNumCol = colCnt;


            //verifica pe ce coloana Contact Type, Opened by si Description
            for (int row = 1; row <= maxNumRow; row++)
            {
                    Range cell = (Range)inputWorkSheet.Cells[row,2];
                    if(userDisplayName == cell.Value)
                    {
                        Range cellx = (Range)inputWorkSheet.Cells[row,1];
                        tagNumber = cellx.Value;
                    }
            }
            //workBook.SaveAs(fullFilePath, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,false, false, 
            //XlSaveAsAccessMode.xlNoChange,Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            workBook.Close(); // finally close workbook
            Console.WriteLine("Workbook closed.");
            application.Quit();
            Console.WriteLine("File closed.");
            return tagNumber;
        }

        public void DownloadFile(String url,String localFilename)
        {
            int bytesProcessed = 0;
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;
            
           // try
           // {
                // Create a request for the specified remote file name
                WebRequest request = WebRequest.Create(url);
            // Create the credentials required for Basic Authentication
            //System.Net.ICredentials credentials = new System.Net.NetworkCredential("user", "pass");
            //ICredentials credentials = CredentialCache.DefaultCredentials;
            // Add the credentials to the request
            //request.Credentials = credentials;
            System.Net.ICredentials cred = new System.Net.NetworkCredential("LiviP", "pizdamati4!");
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Credentials = cred;
                if (request != null)
            {
                // Send the request to the server and retrieve the
                // WebResponse object 
                response = request.GetResponse();
                if (response != null)
                {
                    // Once the WebResponse object has been retrieved,
                    // get the stream object associated with the response's data
                    remoteStream = response.GetResponseStream();
                    // Create the local file
                    localStream = File.Create(localFilename);
                    // Allocate a 1k buffer
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    // Simple do/while loop to read from stream until
                    // no bytes are returned
                    do
                    {
                        // Read data (up to 1k) from the stream
                        bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
                        // Write the data to the local file
                        localStream.Write(buffer, 0, bytesRead);
                        // Increment total bytes processed
                        bytesProcessed += bytesRead;
                    } while (bytesRead > 0);
                }
            }
            //  }
            //  catch (Exception e)
            //  {
            //       MessageBox.Show(e.Message);
            //   }
            //    finally
            //     {
            if (response != null) response.Close();
            if (remoteStream != null) remoteStream.Close();
            if (localStream != null) localStream.Close();
      //      }
            Console.WriteLine(bytesProcessed);
        }

        public string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }
    }
}
