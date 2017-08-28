using System;
using System.Collections.Generic;

namespace hzmj
{
    public static class GroupGen
    {
        public static void Generate()
        {
            //SaveGroups("groups_5.txt", 1);
            //SaveGroups("groups_8.txt", 2);
            //SaveGroups("groups_11.txt", 3);
            SaveGroups("groups_14.txt", 4);
        }

        private static void SaveGroups(string path, int mianziCount)
        {
            var groups = GetGroups(mianziCount);

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

        private static List<List<int>> GetGroups(int mianziCount)
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
                    Console.WriteLine("{0},{1},{2}", i, m1, groups.Count);
                    if (i == m1 && m1 < 27)
                        continue;

                    var mianzi1 = mianzis[m1];
                    if (mianziCount <= 1)
                    {
                        AddGroup(dic, groups, jiang, mianzi1);
                        continue;
                    }

                    for (int m2 = 0; m2 < mianzis.Count; m2++)
                    {
                        if ((i == m2 || m1 == m2) && m2 < 27)
                            continue;
                        var mianzi2 = mianzis[m2];
                        if (mianziCount == 2)
                        {
                            AddGroup(dic, groups, jiang, mianzi1, mianzi2);
                            continue;
                        }

                        for (int m3 = 0; m3 < mianzis.Count; m3++)
                        {
                            if ((i == m3 || m1 == m3 || m2 == m3) && m3 < 27)
                                continue;
                            var mianzi3 = mianzis[m3];
                            if (mianziCount == 3)
                            {
                                AddGroup(dic, groups, jiang, mianzi1, mianzi2, mianzi3);
                                continue;
                            }

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