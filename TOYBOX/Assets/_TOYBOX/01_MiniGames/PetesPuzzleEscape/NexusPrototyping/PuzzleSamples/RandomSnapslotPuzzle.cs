using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Toolkit.Sequences;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToyBox
{
    public class RandomSnapslotPuzzle : MonoSequence
    {
        [SerializeField] SnapSlot snapSlotPrefab = null;
        [SerializeField] List<SnapSlotData> snapPairs = new List<SnapSlotData>();
        private Dictionary<SnapSlot, Snapper> poppedSnappers = new Dictionary<SnapSlot, Snapper>();

        private void Start()
        {
            // Any additional start logic if needed
        }

        protected override UniTask Finish()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask WhenLoad()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask Unload()
        {
            return UniTask.CompletedTask;
        }

        protected override void AfterLoad()
        {
            foreach (SnapSlotData data in snapPairs)
            {
                if (data.snapper.HasSnapPoint)
                {
                    data.snapper.Activate();
                }
            }

            UniTask.Delay(1000).ContinueWith(() => PopSnappers(6)).Forget();
        }

        protected override void OnStart()
        {
            Debug.Log("Starting Random Snapslot Puzzle");
        }

        protected override void OnFinished()
        {
            // Logic to execute when the sequence is finished
        }

        protected override void OnUnload()
        {
            // Logic to execute on sequence unload
        }

        public void PopSnappers(int count)
        {
            if (snapPairs.Count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, snapPairs.Count);
                Snapper snapper = snapPairs[randomIndex].snapper;
                snapper.EnablePhysics(true);
                if (snapper.TryGetComponent(out Rigidbody rb))
                {
                    Vector3 randomDirection = Random.insideUnitSphere;
                    Vector3 snapperForward = snapper.transform.forward;
                    rb.AddForce(Vector3.up * 2 + randomDirection + snapperForward, ForceMode.Impulse);
                }

                SnapSlot snapSlot = Instantiate(snapSlotPrefab, snapper.transform.position, Quaternion.identity, transform);
                snapSlot.SetSnapper(snapper);
                snapSlot.OnSnap.AddListener(OnSnap);

                poppedSnappers.Add(snapSlot, snapper);
                snapper.gameObject.SetActive(true);
            }
        }

        public void OnSnap(SnapSlot snapSlot)
        {
            if (poppedSnappers.ContainsKey(snapSlot))
            {
                snapSlot.OnSnap.RemoveListener(OnSnap);
                poppedSnappers.Remove(snapSlot);
                Destroy(snapSlot.gameObject);
            }
        }

        bool SnapPairsContains(Snapper snapper)
        {
            foreach (SnapSlotData data in snapPairs)
            {
                if (data.snapper == snapper)
                {
                    return true;
                }
            }
            return false;
        }

#if UNITY_EDITOR
        [Button]
        private void FindAllSnapSlots()
        {
            List<Snapper> snappers = new List<Snapper>(GetComponentsInChildren<Snapper>());

            snapPairs.Clear();
            foreach (Snapper snapper in snappers)
            {
                snapPairs.Add(new SnapSlotData
                {
                    snapSlot = null,
                    snapper = snapper
                });
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [Button]
        private void GenerateSnapSlots()
        {
            for (int i = 0; i < snapPairs.Count; i++)
            {
                SnapSlotData data = snapPairs[i];
                Snapper snapper = data.snapper;
                if (snapper.HasSnapPoint)
                {
                    continue;
                }

                SnapSlot snapSlot = PrefabUtility.InstantiatePrefab(snapSlotPrefab, snapper.transform.parent) as SnapSlot;
                snapSlot.transform.position = snapper.transform.position;
                snapSlot.transform.rotation = snapper.transform.rotation;
                snapSlot.transform.localScale = Vector3.one;
                snapSlot.gameObject.name = snapper.gameObject.name + "_SnapSlot";

                snapSlot.SetSnapper(snapper);
                snapper.SetSnapSlot(snapSlot);

                if (data.snapSlot != null)
                {
                    DestroyImmediate(data.snapSlot.gameObject);
                }
                data.snapSlot = snapSlot;

                snapPairs[i] = data;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [Button]
        void DestroyAllSnapSlots()
        {
            SnapSlot[] snapSlots = GetComponentsInChildren<SnapSlot>();
            foreach (SnapSlot snapSlot in snapSlots)
            {
                DestroyImmediate(snapSlot.gameObject);
            }
        }

        [Button]
        void AddMeshCollidersToAllMeshFilters()
        {
            MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter filter in filters)
            {
                if (filter.TryGetComponent(out MeshCollider collider))
                {
                    collider.sharedMesh = filter.sharedMesh;
                    collider.convex = true;
                }
                else
                {
                    collider = filter.gameObject.AddComponent<MeshCollider>();
                    collider.sharedMesh = filter.sharedMesh;
                    collider.convex = true;
                }
            }
        }
#endif

        [System.Serializable]
        struct SnapSlotData
        {
            public SnapSlot snapSlot;
            public Snapper snapper;
        }
    }
}
