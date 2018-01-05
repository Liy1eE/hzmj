namespace hzmj
{
    public static class HuTools
    {
        public static int CalcKey(int[] array)
        {
            int ret = 0;
            int zeroCount = 0;

            foreach (int count in array)
            {
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
    }
}