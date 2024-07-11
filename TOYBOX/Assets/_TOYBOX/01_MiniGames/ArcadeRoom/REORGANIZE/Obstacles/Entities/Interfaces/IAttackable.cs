using System.Diagnostics;

namespace ToyBox.Minigames.BeatEmUp
{
    public interface IAttackable : IEntity
    {
        public void Attack(IAttacker attacker)
        {
            UnityEngine.Debug.Log("Attacked");
            OnAttacked(attacker);
        }

        void OnAttacked(IAttacker attacker);
    }

}




