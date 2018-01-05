namespace hzmj
{
    public class HashData
    {
        public int Key { get; }
        public int? HzCount { get; private set; }
        public bool? IsJiang { get; private set; }

        public HashData(int key)
        {
            Key = key;
        }

        public void SetState(int hzCount, bool isJiang)
        {
            if (HzCount == null && IsJiang == null)
            {
                HzCount = hzCount;
                IsJiang = isJiang;
            }
            else
            {
                if (hzCount < HzCount)
                {
                    HzCount = hzCount;
                    IsJiang = isJiang;
                }
            }
        }

        public override string ToString()
        {
            return "[" + Key + "]";
        }
    }
}