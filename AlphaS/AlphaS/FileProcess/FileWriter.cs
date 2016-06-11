using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
namespace AlphaS.FileProcess
{
    public class FileWriter: IFileWriter
    {
        private List<FileWriteOrder> OrderList = new List<FileWriteOrder>();
        public void WriteFile(string filepath, string content)
        {
            OrderList.Add(new FileWriteOrder { filepath = filepath, content = content });
            Thread t = new Thread(work);
            t.Start();
        }
        public void WriteFile(string folder, string filename, string content)
        {
            WriteFile(folder + "\\" + filename, content);
        }
        private void work()
        {
            while (OrderList.Count > 0)
            {
                using (var sw = new StreamWriter(OrderList.First().filepath, false, Encoding.Default))
                {
                    sw.Write(OrderList.First().content);
                }
                OrderList.RemoveAt(0);
            }
            OnAllWorkDone(this, null);
        }

        public event OnAllWorkDoneEventHandler OnAllWorkDone;
        public delegate void OnAllWorkDoneEventHandler(object sender, EventArgs e);
    }
}
