using System;

namespace GrainInterfaces.Game
{
    [Serializable]
    public class Dice
    {
        public Dice(int value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public int Value1 { get; }
        public int Value2 { get; }

        public int Sum => Value1 + Value2;

        public override string ToString()
        {
            return $"({Value1},{Value2})";
        }
    }
}