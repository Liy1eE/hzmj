using System;
using System.IO;
using System.Text;

namespace hzmj
{
    public static class Util
    {
        public static void Read(string path, Action<StreamReader> handle)
        {
            using (var sr = new StreamReader(path, Encoding.Default))
            {
                handle(sr);
            }
        }

        public static void Write(string path, Action<StreamWriter> handle)
        {
            var fs = new FileStream(path, FileMode.Create);
            var sw = new StreamWriter(fs);
            handle(sw);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}