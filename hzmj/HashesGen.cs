using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace hzmj
{
    public class HashesGen
    {
        private static int _maxHzCount;
        private static int _length;
        private static readonly Dictionary<string, bool> _parsedGroupDic = new Dictionary<string, bool>();
        private static readonly Dictionary<int, HashData> _hashDic = new Dictionary<int, HashData>();

        public static void Generate(int maxHzCount, CardType cardType, string prefix)
        {
            _parsedGroupDic.Clear();
            _hashDic.Clear();
            _maxHzCount = maxHzCount;
            _length = (int)cardType;
            var array = Enumerable.Repeat(0, _length).ToArray();
            var genGroup = cardType == CardType.Num ? new Action<int[], bool>(GenNumGroup) : GenFengGroup;

            genGroup(array, false);
            for (int i = 0; i < _length; i++)
            {
                array[i] = 2;
                ParseGroup(array, true);
                genGroup(array, true);
                array[i] = 0;
            }

            var groups = _hashDic.Values.GroupBy(x => x.IsJiang).ToList();
            foreach (var groupData in groups)
            {
                var hashDatas = groupData.OrderBy(x => x.Key).ToList();
                string fileName = $"{prefix}{((bool)groupData.Key ? "" : "_no")}_jiang_map.lua";
                var sb = new StringBuilder();
                sb.Append("return{\n");
                foreach (var hashData in hashDatas)
                    sb.AppendLine($"[{hashData.Key}]={ hashData.HzCount},");
                sb.Append("}");
                File.WriteAllText(fileName, sb.ToString());
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
            string key = string.Join(",", array);

            if (_parsedGroupDic.ContainsKey(key))
                return;

            _parsedGroupDic[key] = true;
            if (!array.Any(x => x > 4))
                Add(HuTools.CalcKey(array), 0, isJiang);

            GenHash(array, 1, isJiang);
        }

        private static void GenHash(int[] array, int hzCount, bool isJiang)
        {
            for (int i = 0; i < _length; i++)
            {
                if (array[i] == 0)
                    continue;

                array[i] -= 1;

                if (!array.Any(x => x > 4))
                {
                    int key = HuTools.CalcKey(array);
                    Add(key, hzCount, isJiang);
                }

                if (hzCount < _maxHzCount)
                    GenHash(array, hzCount + 1, isJiang);

                array[i] += 1;
            }
        }

        private static void GenNumGroup(int[] array, bool isJiang)
        {
            for (var i = 0; i < 16; i++)
            {
                if (array.Sum() > 11)
                    break;

                if (i < 9)
                {
                    if (array[i] > _maxHzCount + 1)
                        continue;
                    array[i] += 3;
                }
                else
                {
                    var idx = i - 9;
                    if (array[idx] > _maxHzCount + 3 || array[idx + 1] > _maxHzCount + 3
                        || array[idx + 2] > _maxHzCount + 3)
                        continue;
                    array[idx] += 1;
                    array[idx + 1] += 1;
                    array[idx + 2] += 1;
                }

                ParseGroup(array, isJiang);
                GenNumGroup(array, isJiang);

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

        private static void GenFengGroup(int[] array, bool isJiang)
        {
            for (var i = 0; i < 7; i++)
            {
                if (array.Sum() > 11)
                    break;

                if (array[i] > _maxHzCount + 1)
                    continue;
                array[i] += 3;

                ParseGroup(array, isJiang);
                GenFengGroup(array, isJiang);

                array[i] -= 3;
            }
        }
    }
}