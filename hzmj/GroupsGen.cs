using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace hzmj
{
    public static class GroupsGen
    {
        public static Stopwatch _stopwatch;

        public static void Generate()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            SaveGroups("groups.txt");
        }

        private static void SaveGroups(string path)
        {
            var groups = GetGroups();

            Util.Write(path, sw =>
            {
                foreach (var group in groups)
                    sw.Write(string.Join(",", group) + "\n");
            });
        }

        private static List<List<int>> GetJiangs()
        {
            var jiangs = new List<List<int>>();
            for (int i = 0; i < 27; i++)
            {
                jiangs.Add(new List<int> { i, i });
            }
            return jiangs;
        }

        private static List<List<int>> GetMianzis()
        {
            var mianzis = new List<List<int>>();
            for (int i = 0; i < 27; i++)
            {
                mianzis.Add(new List<int> { i, i, i });
            }
            for (int i = 0; i < 27; i++)
            {
                if (i % 9 >= 7)
                    continue;
                mianzis.Add(new List<int> { i, i + 1, i + 2 });
            }
            return mianzis;
        }

        private static List<List<int>> GetGroups()
        {
            var jiangs = GetJiangs();
            var mianzis = GetMianzis();
            var groups = new List<List<int>>();
            var dic = new Dictionary<string, bool>();

            for (var i = 0; i < jiangs.Count; i++)
            {
                var jiang = jiangs[i];
                for (var m1 = 0; m1 < mianzis.Count; m1++)
                {
                    Console.WriteLine("{0},{1},{2},time:{3}", i, m1, groups.Count, _stopwatch.ElapsedMilliseconds / 1000f);
                    if (i == m1 && m1 < 27)
                        continue;
                    var mianzi1 = mianzis[m1];

                    for (int m2 = 0; m2 < mianzis.Count; m2++)
                    {
                        if ((i == m2 || m1 == m2) && m2 < 27)
                            continue;
                        var mianzi2 = mianzis[m2];

                        for (int m3 = 0; m3 < mianzis.Count; m3++)
                        {
                            if ((i == m3 || m1 == m3 || m2 == m3) && m3 < 27)
                                continue;
                            var mianzi3 = mianzis[m3];

                            for (var m4 = 0; m4 < mianzis.Count; m4++)
                            {
                                if ((i == m4 || m1 == m4 || m2 == m4 || m3 == m4) && m4 < 27)
                                    continue;
                                var mianzi4 = mianzis[m4];
                                AddGroup(dic, groups, jiang, mianzi1, mianzi2, mianzi3, mianzi4);
                            }
                        }
                    }
                }
            }
            return groups;
        }

        private static void AddGroup(Dictionary<string, bool> dic, List<List<int>> groups,
            params List<int>[] arg)
        {
            var group = new List<int>();
            foreach (var part in arg)
                group.AddRange(part);

            group.Sort();
            bool invalid = false;

            for (int j = 0; j < group.Count - 4; j++)
            {
                if (group[j] == group[j + 4])
                {
                    invalid = true;
                    break;
                }
            }

            if (invalid)
                return;

            string key = string.Join(",", group);

            if (!dic.ContainsKey(key))
            {
                dic[key] = true;
                groups.Add(group);
            }
        }
    }
}