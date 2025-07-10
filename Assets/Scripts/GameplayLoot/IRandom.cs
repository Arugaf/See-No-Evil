using System.Collections.Generic;
namespace Gameplay.Loot
{
    // That shit is only for "guarantee" mechanics maybe to be implemented later
    public interface IRandom
    {

        public T PickRandom<T>(IReadOnlyCollection<ObjectChancePair<T>> Pairs, string metadata = "");
    }
}