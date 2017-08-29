using System;

namespace hzmj
{
    [Serializable]
    public class HuData
    {
        public HuData(uint[] partA, uint[] partBLow, byte[] partBHigh)
        {
            PartA = partA;
            PartBLow = partBLow;
            PartBHigh = partBHigh;
        }

        public uint[] PartA { get; }
        public uint[] PartBLow { get; }
        public byte[] PartBHigh { get; }

        public bool Contain(long value)
        {
            if (value < uint.MaxValue)
            {
                return Array.BinarySearch(PartA, (uint)value) >= 0;
            }

            int lowIndex = 0;
            int highIndex = PartBLow.Length - 1;
            while (lowIndex <= highIndex)
            {
                int middleIndex = (lowIndex + highIndex) / 2;
                long v = PartBLow[middleIndex] | ((long)PartBHigh[middleIndex] << 32);
                if (value == v)
                    return true;

                if (value > v)
                    lowIndex = middleIndex + 1;
                else
                    highIndex = middleIndex - 1;
            }

            return false;
        }
    }
}