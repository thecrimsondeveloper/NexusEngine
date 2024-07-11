using UnityEngine;

namespace Toolkit.Pooling
{
    public class PoolerController : MonoBehaviour
    {
        [SerializeField] private float PerObjectLoadTime = 0.1f;

        public void AddObjects(GameObject gameObject, int amount)
        {
            // PoolDefinition def = new PoolDefinition(gameObject, amount);
            // Pooler.AddObjectsToPool(def, PerObjectLoadTime);
            Debug.LogError("AddObjects not implemented");
        }

        public GameObject Spawn(SpawnData data)
        {
            return Pooler.Spawn(data);
        }
    }
}
