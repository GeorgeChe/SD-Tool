using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Threading;
using System.Collections;


namespace WindowsFormsApplication1
{
    public class GetTagBasedOnUser
    {

    
    
        static string theBaseDirectory = AppDomain.CurrentDomain.BaseDirectory; // containg the base directry for the .EXE file
        static string folderName = "TagInfo";
        static string destinationPath = Path.Combine(theBaseDirectory, folderName);
        static string sourcePath = @"\\sch.com\it\Audit";
        public string[] filesArray = Directory.GetFiles(sourcePath, "*.txt");
        public string[] localFilesArray;

        private string FormatTagNumber(string tagNumber)
        {
            int tagLenght = tagNumber.Length;
            tagNumber = tagNumber.ToUpper();
            int length = tagNumber.IndexOf("TAG-");
            if (length != -1)
                {
                string temp = tagNumber.Substring(length,length);
                int length2 = temp.IndexOf(",");
                tagNumber = temp.Substring(0, length2);
                }
            return tagNumber;
        }

        public string ContineRezultat(string userName)
        {
         userName = userName.ToLower();
         string tagContainer = string.Empty;
         localFilesArray = Directory.GetFiles(destinationPath, "*.txt");

            if (localFilesArray.Length != 0)
            {
                for (int i = localFilesArray.Length - 1; i >= 0; i--)
                {
                    using (StreamReader sr = File.OpenText(localFilesArray[i]))
                    {
                        string s = string.Empty;
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (s.ToLower().Contains(userName))
                            {
                                string[] words = s.Split(' ', ',', '\t');
                                for (int j = 0; j < words.Length; j++)
                                    if (words[j].ToLower().Equals(userName))
                                        tagContainer += FormatTagNumber(s) + "\n";
                            }
                        }
                    }
                }
            }
            return tagContainer;
        }

        public void ForLoop()
        {
            for (int j = 0; j < filesArray.Length; j++)
            {
                CopyFiles(j);
            }
        }
        public void ParallelForLoop()
        {
            int i = -1;
            Parallel.For(0, filesArray.Length,
                copy =>
                {
                    int nextIndex = Interlocked.Increment(ref i);
                    CopyFiles(nextIndex);
                });
        }

        public void CopyFiles(int index)
        {
            string localFileName = filesArray[index].ToString().Substring(filesArray[index].ToString().LastIndexOf('\\'));
            if (localFilesArray.Length != 0)
            {
                Array.Sort(localFilesArray, new AlphanumComparatorFast());
                Array.Sort(filesArray, new AlphanumComparatorFast());
                FileInfo localFile = new FileInfo(localFilesArray[index]);
                FileInfo serverFile = new FileInfo(filesArray[index]);
                if (serverFile.LastWriteTime > localFile.LastWriteTime)
                    File.Copy(filesArray[index].ToString(), destinationPath + localFileName, true);
            }
            else
                File.Copy(filesArray[index].ToString(), destinationPath + localFileName, true);
        }

        public void MakeTagInfoFolder()
        {
            if ((localFilesArray = Directory.GetFiles(destinationPath, "*.txt")).Length != 0)
                localFilesArray = Directory.GetFiles(destinationPath, "*.txt");
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
                //Console.WriteLine($"File {folderName} created!");
            }
            else
            {
                //Console.WriteLine($"File {folderName} already exists!");
            }

        }
    }
    public class AlphanumComparatorFast : IComparer
    {
        public int Compare(object x, object y)
        {
            string s1 = x as string;
            if (s1 == null)
            {
                return 0;
            }
            string s2 = y as string;
            if (s2 == null)
            {
                return 0;
            }

            int len1 = s1.Length;
            int len2 = s2.Length;
            int marker1 = 0;
            int marker2 = 0;

            // Walk through two the strings with two markers.
            while (marker1 < len1 && marker2 < len2)
            {
                char ch1 = s1[marker1];
                char ch2 = s2[marker2];

                // Some buffers we can build up characters in for each chunk.
                char[] space1 = new char[len1];
                int loc1 = 0;
                char[] space2 = new char[len2];
                int loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 < len1)
                    {
                        ch1 = s1[marker1];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 < len2)
                    {
                        ch2 = s2[marker2];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                string str1 = new string(space1);
                string str2 = new string(space2);

                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                {
                    int thisNumericChunk = int.Parse(str1);
                    int thatNumericChunk = int.Parse(str2);
                    result = thisNumericChunk.CompareTo(thatNumericChunk);
                }
                else
                {
                    result = str1.CompareTo(str2);
                }

                if (result != 0)
                {
                    return result;
                }
            }
            return len1 - len2;
        }
    }
}
