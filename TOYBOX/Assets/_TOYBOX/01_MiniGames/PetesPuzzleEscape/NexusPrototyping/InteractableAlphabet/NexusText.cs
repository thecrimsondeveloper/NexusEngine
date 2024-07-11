using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR    
using UnityEditor;
#endif

namespace Toolkit.NexusEngine
{
    public class NexusText : MonoBehaviour
    {
        [SerializeField] NexusTextData textData;
        public enum Direction
        {
            Left,
            Right,
        }

        [SerializeField] string text;
        [SerializeField] float spacing;
        [SerializeField] Direction direction;

        string currentText = "";


        bool isRefreshing = false;

        #if UNITY_EDITOR
        public async void RefreshText(string text, float spacing)
        {
            if (isRefreshing)
            {
                return;
            }

            isRefreshing = true;
            currentText = text;

            while (transform.childCount > 0)
            {
                if (Application.isPlaying)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
                await UniTask.NextFrame();
            }


            for (int i = 0; i < text.Length; i++)
            {
                char letter = text[i];
                if (textData.TryGetCharacterObject(letter, out GameObject letterObject))
                {
                    NexusChar character = new NexusChar
                    {
                        character = letter,
                        gameObject = PrefabUtility.InstantiatePrefab(letterObject, transform) as GameObject
                    };


                    character.gameObject.transform.localPosition = new Vector3(spacing * i * (direction == Direction.Left ? 1 : -1), 0, 0);
                }
            }

            isRefreshing = false;

            if (currentText != text)
            {
                RefreshText(text, spacing);
            }
        }
        #endif


        struct NexusChar
        {
            public char character;
            public GameObject gameObject;
        }
    }
}
