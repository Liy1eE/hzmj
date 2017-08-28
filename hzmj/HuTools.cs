namespace hzmj
{
    public static class HuTools
    {
        public static int CalcKey(int[] newGroup, int color)
        {
            int ret = 0;
            int zeroCount = -9;

            for (int j = color; j < 9; j++)
            {
                int count = newGroup[color * 9 + j];
                if (count == 0)
                    zeroCount++;
                else
                {
                    if (zeroCount > 0)
                        ret = ret * (zeroCount > 1 ? 100 : 10);

                    zeroCount = 0;
                    ret = ret * 10 + count;
                }
            }

            return ret;
        }

        // traditional group [0,0,1,1,2,2,...]
        // new group [1,0,0,3,...]
        public static int[] GetRemovedNewGroup(int[] g, int a = -1, int b = -1, int c = -1, int d = -1)
        {
            var newGroup = new int[27];
            for (var i = 0; i < g.Length; i++)
            {
                if (i == a || i == b || i == c || i == d)
                    continue;

                newGroup[g[i]]++;
            }
            return newGroup;
        }
    }
}