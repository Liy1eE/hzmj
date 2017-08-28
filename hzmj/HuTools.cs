namespace hzmj
{
    public static class HuTools
    {
        // 1 -> 0     【0】
        // 10 -> 10
        // 100 ->110
        // 2 -> 1110     【3】
        // 20 -> 11110
        // 200 -> 111110
        // 3 -> 1111110     【6】
        // 30 -> 11111110
        // 300 -> 111111110
        // 4 -> 1111111110     【9】
        // 40 -> 11111111110
        // 400 -> 111111111110
        public static long CalcKey(int[] newGroup)
        {
            long ret = 0;
            int length = -1;
            bool b = false;
            int interval = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int count = newGroup[i * 9 + j];
                    if (count == 0)
                    {
                        if (b)
                        {
                            ret |= 1L << length;
                            length++;
                            interval++;
                            if (interval >= 2)
                                b = false;
                        }
                    }
                    else
                    {
                        length++;
                        b = true;
                        interval = 0;

                        switch (count)
                        {
                            case 2:
                                ret |= 7L << length;
                                length += 3;
                                break;

                            case 3:
                                ret |= 63L << length;
                                length += 6;
                                break;

                            case 4:
                                ret |= 511L << length;
                                length += 9;
                                break;
                        }
                    }
                }
                if (b)
                {
                    b = false;
                    if (interval == 1)
                    {
                        ret |= 1L << length;
                        length += 1;
                        interval++;
                    }
                    else
                    {
                        ret |= 3L << length;
                        length += 2;
                    }
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