using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Toolkit.NexusEngine
{
    [CreateAssetMenu(fileName = "NexusTextData", menuName = "Nexus/Alphabet/NexusTextData")]
    public class NexusTextData : ScriptableObject
    {

        [System.Serializable]
        struct TextObject
        {
            public char character;
            public GameObject gameObject;
        }

        [SerializeField] TextObject[] textObjects;


        public bool TryGetCharacterObject(char character, out GameObject gameObject)
        {
            foreach (var textObject in textObjects)
            {
                if (textObject.character == character)
                {
                    gameObject = textObject.gameObject;
                    return true;
                }
            }

            gameObject = null;
            return false;
        }
#if UNITY_EDITOR

        [Button("Generate Alphabet")]
        public void GenerateAlphaBet()
        {
            textObjects = new TextObject[26];

            GameObject[] selectedPrefabs = UnityEditor.Selection.gameObjects;

            for (int i = 0; i < 26; i++)
            {
                textObjects[i].character = (char)(i + 65);

                if (selectedPrefabs.Length > i)
                    textObjects[i].gameObject = selectedPrefabs[i];
            }
        }
        #endif
    }
}
