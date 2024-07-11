using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class GameObjectController : MonoBehaviour
    {
        public GameObject[] gameObjects;
        public void SetActive(bool active)
        {
            foreach (var go in gameObjects)
            {
                go.SetActive(active);
            }
        }

        public void SetActive(int index, bool active)
        {
            if (index < 0 || index >= gameObjects.Length)
                return;

            gameObjects[index].SetActive(active);
        }

        public void SetActive(string name, bool active)
        {
            foreach (var go in gameObjects)
            {
                if (go.name == name)
                {
                    go.SetActive(active);
                    return;
                }
            }
        }

        public void SetActive(GameObject go, bool active)
        {
            go.SetActive(active);
        }

        public void SetInactive()
        {
            foreach (var go in gameObjects)
            {
                go.SetActive(false);
            }
        }

        public void SetInactive(int index)
        {
            if (index < 0 || index >= gameObjects.Length)
                return;

            gameObjects[index].SetActive(false);
        }

        public void DestroyAll()
        {
            foreach (var go in gameObjects)
            {
                Destroy(go);
            }

            gameObjects = new GameObject[0];
        }
    }
}
