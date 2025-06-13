using System;

namespace Actors
{
    [Serializable]
    public struct DamageParameters
    {
        public int Damage;
        public float StunTime;
        public float StunCoeff;
        public DamageParameters(int damage)
        {
            Damage = damage;
            StunTime = 0;
            StunCoeff = 0;
        }
    }
}