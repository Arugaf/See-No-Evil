using System.Collections.Generic;
namespace Gameplay.Loot
{
    public class LootDependentRandom : IRandom
    {
        private IGameLootManager _manager;
        private BasicRandom rnd;
        public LootDependentRandom(IGameLootManager manager)
        {
            _manager = manager;
            rnd = new BasicRandom();
        }

        public T PickRandom<T>(IReadOnlyCollection<ObjectChancePair<T>> Pairs, string metadata = "")
        {
            var filtered = new List<ObjectChancePair<T>>();
            foreach (var pair in Pairs)
            {
                if (pair.Value is LootScriptableObject s)
                {
                    if (_manager.Get(s.ID).Count > 0)
                    {
                        pair.weight /= 10.0f;
                    }
                    filtered.Add(pair);
                } 
                else
                {
                    filtered.Add(pair);
                }

            }
            if (filtered.Count == 0) return rnd.PickRandom(Pairs);
            else return rnd.PickRandom(filtered);
        }
    }
}