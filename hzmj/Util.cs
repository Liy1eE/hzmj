using System;
using System.IO;

namespace hzmj
{
    public static class Util
    {
        public static void Read()
        {
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