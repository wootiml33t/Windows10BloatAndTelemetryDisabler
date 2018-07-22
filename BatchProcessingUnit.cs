using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class BatchProcessingUnit
    {
        public BatchProcessingUnit()
        {
            //
        }
        private String filePath = String.Empty;
        public String FilePath()
        {
            return filePath;
        }
        public String BatchBuilder(String[] batchSource, String fileName)
        {
            //Generate the batch file from string array
            StreamWriter w = new StreamWriter(fileName);
            for (int i = 0; i < batchSource.Length; i++)
            {
                w.WriteLine(batchSource[i]);
            }
            w.Close();
            return fileName;
        }
        public void BatchExecuter(String fileName)
        {
            //Execute the batch file
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = fileName;
            //Forces batch to run as admin
            if (System.Environment.OSVersion.Version.Major >= 6)
            {
                proc.StartInfo.Verb = "runas";
            }
            try
            {
                proc.Start();
                proc.WaitForExit();
            }
            catch { }
            //Deletes the temp batch file
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
