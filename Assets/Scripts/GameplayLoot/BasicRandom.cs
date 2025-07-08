using System.Collections.Generic;
namespace Gameplay.Loot
{
    public class BasicRandom : IRandom
    {
        public T PickRandom<T>(IReadOnlyCollection<ObjectChancePair<T>> Pairs, string metadata = "")
        {
            float sum = 0;
            foreach (var pair in Pairs)
            {
                sum += pair.weight;
            }
            float rnd = UnityEngine.Random.Range(0, sum);
            T last = default;
            foreach (var pair in Pairs)
            {
                rnd -= pair.weight;
                last = pair.Value;
                if (rnd <= 0) break;
            }
            return last;
        }
    }
}