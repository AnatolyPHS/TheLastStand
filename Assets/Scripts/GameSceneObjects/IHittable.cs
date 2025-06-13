using UnityEngine;

namespace GameSceneObjects
{
    public interface IHittable
    {
        void GetDamage(float dmg);
        bool IsAlive();
        Vector3 GetPosition();
        void Heal(float healEffect);
        bool CanBeAttacked();
    }
}
