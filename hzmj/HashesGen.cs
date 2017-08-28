using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace hzmj
{
    public static class HashesGen
    {
        public static void Generate()
        {
            Console.WriteLine("Start!");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var groups = LoadGroups("groups.txt");

            stopWatch.Stop();
            Console.WriteLine("serialize time:" + stopWatch.ElapsedMilliseconds / 1000f);

            stopWatch.Restart();

            groups = groups.Select(x => x.Where(y => y < 9).ToArray()).Where(x => x.Length != 0)
                .GroupBy(x => string.Join(",", x)).Select(x => x.First()).OrderBy(x => x.Length).ToList();

            Console.WriteLine("sort groups time:" + stopWatch.ElapsedMilliseconds / 1000f);

            var hashDic = new Dictionary<int, int>();

            void Check(int originalLength, int hzCount, int key)
            {
                int value = 1 << hzCount;

                // it is jiang part?
                if (originalLength % 3 != 0)
                    value <<= 5;

                if (!hashDic.ContainsKey(key))
                {
                    hashDic[key] = value;
                }
                else
                {
                    int v = hashDic[key];

                    if ((v & value) == 0)
                        hashDic[key] = v | value;
                }
            }

            for (var i = 0; i < groups.Count; i++)
            {
                if (i % 1000 == 0)
                    Console.WriteLine("index:{0},length:{1},time:{2}", i, hashDic.Count,
                        stopWatch.ElapsedMilliseconds / 1000f);

                var g = groups[i];
                int length = g.Length;

                var dict1 = new Dictionary<int, bool>();
                var dict2 = new Dictionary<string, bool>();
                var dict3 = new Dictionary<string, bool>();
                var dict4 = new Dictionary<string, bool>();

                var _newGroup = HuTools.GetRemovedNewGroup(g);
                int _hash = HuTools.CalcKey(_newGroup, 0);

                Check(length, 0, _hash);

                for (int a = 0; a < length; a++)
                {
                    if (!dict1.ContainsKey(a))
                    {
                        dict1[a] = false;
                        var newGroup = HuTools.GetRemovedNewGroup(g, a);
                        int hash = HuTools.CalcKey(newGroup, 0);
                        Check(length, 1, hash);
                    }
                    for (int b = a + 1; b < length; b++)
                    {
                        string key2 = a + " " + b;
                        if (!dict2.ContainsKey(key2))
                        {
                            dict2[key2] = false;
                            var newGroup = HuTools.GetRemovedNewGroup(g, a, b);
                            int hash = HuTools.CalcKey(newGroup, 0);
                            Check(length, 2, hash);
                        }
                        for (int c = b + 1; c < length; c++)
                        {
                            string key3 = a + " " + b + " " + c;
                            if (!dict3.ContainsKey(key3))
                            {
                                dict3[key3] = false;
                                var newGroup = HuTools.GetRemovedNewGroup(g, a, b, c);
                                int hash = HuTools.CalcKey(newGroup, 0);
                                Check(length, 3, hash);
                            }
                            for (int d = c + 1; d < length; d++)
                            {
                                string key4 = a + " " + b + " " + c + " " + d;
                                if (!dict4.ContainsKey(key4))
                                {
                                    dict4[key4] = false;
                                    var newGroup = HuTools.GetRemovedNewGroup(g, a, b, c, d);
                                    int hash = HuTools.CalcKey(newGroup, 0);
                                    Check(length, 4, hash);
                                }
                            }
                        }
                    }
                }
            }

            SaveLuaCode(hashDic);
        }

        private static List<int[]> LoadGroups(string path)
        {
            var groups = new List<int[]>();
            Util.Read(path, sr =>
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var array = line.Split(',').Select(int.Parse).ToArray();
                    groups.Add(array);
                }
            });

            return groups;
        }

        private static void SaveLuaCode(Dictionary<int, int> hashDic)
        {
            var dic = new Dictionary<int, bool>();
            var hashes = hashDic.Keys.Where(x =>
            {
                int reversal = Convert.ToInt32(new string(Convert.ToString(x).Reverse().ToArray()));

                if (dic.ContainsKey(x) || dic.ContainsKey(reversal))
                    return false;

                dic[x] = true;
                return true;
            }).ToList();

            hashes.Sort();

            Console.WriteLine("all count:" + hashes.Count);

            Util.Write("hz_hu_map.lua", sw =>
            {
                sw.Write("return{\n");
                foreach (int hash in hashes)
                    sw.WriteLine("[{0}]={1},", hash, hashDic[hash]);
                sw.Write("}");
            });

            Console.WriteLine("Done!");
        }
    }
}