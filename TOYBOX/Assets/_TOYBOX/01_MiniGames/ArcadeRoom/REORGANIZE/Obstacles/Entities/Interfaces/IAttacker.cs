using UnityEngine;

namespace ToyBox.Minigames.BeatEmUp
{
    public interface IAttacker : IEntity
    {
        public int strength { get; }
    }

    public interface ICollidable : IEntity
    {
        void OnCollisionEnter(Collision other);
    }
}
