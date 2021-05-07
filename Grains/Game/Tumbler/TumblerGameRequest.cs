using GrainInterfaces.Game;

namespace Grains.Game.Tumbler
{
    public class TumblerGameRequest
    {
        public TumblerGameRequest(Dice dice1, Dice dice2)
        {
            Dice1 = dice1;
            Dice2 = dice2;
        }

        public Dice Dice1 { get; }
        public Dice Dice2 { get; }
    }
}