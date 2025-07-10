namespace Gameplay.Loot
{
    public interface IRandomPickable<T>
    {
        public T Pick(IRandom rnd);
    }
}