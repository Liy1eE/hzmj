using System;
using System.Collections.Generic;
using System.Linq;

namespace hzmj
{
    public class HashesGen
    {
        private static readonly Dictionary<int, bool> _parsedGroupDic = new Dictionary<int, bool>();
        private static readonly Dictionary<int, HashData> _hashDic = new Dictionary<int, HashData>();

        public static void Generate()
        {
            var array = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            GenGroup(array, false);
            for (int i = 0; i < 9; i++)
            {
                array[i] = 2;
                ParseGroup(array, true);
                GenGroup(array, true);
                array[i] = 0;
            }

            var groups = _hashDic.Values.GroupBy(x => x.IsJiang).ToList();
            foreach (var groupData in groups)
            {
                var hashDatas = groupData.OrderBy(x => x.Key).ToList();

                string fileName = "hz" + ((bool)groupData.Key ? "" : "_no") + "_jiang_map.lua";
                Util.Write(fileName, sw =>
                {
                    sw.Write("return{\n");
                    foreach (var hashData in hashDatas)
                        sw.WriteLine("[{0}]={1},", hashData.Key, hashData.HzCount);
                    sw.Write("}");
                });
            }

            Console.WriteLine("Done!");
        }

        private static void Add(int key, int hzCount, bool isJiang)
        {
            if (key == 0)
                return;

            if (_hashDic.TryGetValue(key, out var hashData))
            {
                hashData.SetState(hzCount, isJiang);
            }
            else
            {
                hashData = new HashData(key);
                hashData.SetState(hzCount, isJiang);
                _hashDic[key] = hashData;
            }
        }

        private static void ParseGroup(int[] array, bool isJiang)
        {
            int key = HuTools.CalcKey(array);
            if (_parsedGroupDic.ContainsKey(key))
                return;

            _parsedGroupDic[key] = true;
            if (array.All(x => x <= 4))
                Add(key, 0, isJiang);

            GenHash(array, 1, isJiang);
        }

        private static void GenHash(int[] array, int hzCount, bool isJiang)
        {
            for (int i = 0; i < 9; i++)
            {
                if (array[i] == 0)
                    continue;

                array[i] -= 1;

                if (array.All(x => x <= 4))
                {
                    int key = HuTools.CalcKey(array);
                    Add(key, hzCount, isJiang);
                }

                if (hzCount < 4)
                    GenHash(array, hzCount + 1, isJiang);

                array[i] += 1;
            }
        }

        private static void GenGroup(int[] array, bool isJiang)
        {
            for (var i = 0; i < 16; i++)
            {
                if (array.Sum() > 11)
                    break;

                if (i < 9)
                {
                    if (array[i] > 5)
                        continue;
                    array[i] += 3;
                }
                else
                {
                    var idx = i - 9;
                    if (array[idx] > 7 || array[idx + 1] > 7 || array[idx + 2] > 7)
                        continue;
                    array[idx] += 1;
                    array[idx + 1] += 1;
                    array[idx + 2] += 1;
                }

                ParseGroup(array, isJiang);
                GenGroup(array, isJiang);

                if (i < 9)
                {
                    array[i] -= 3;
                }
                else
                {
                    var idx = i - 9;
                    array[idx] -= 1;
                    array[idx + 1] -= 1;
                    array[idx + 2] -= 1;
                }
            }
        }
    }
}