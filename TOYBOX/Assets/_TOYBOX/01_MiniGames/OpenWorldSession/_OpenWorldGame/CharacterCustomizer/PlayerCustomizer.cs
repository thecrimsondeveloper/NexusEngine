using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerCustomizer : MonoBehaviour
{
    [SerializeField] CharacterCustomizationData characterCustomizationData;

    [Button]
    public void SetData(CharacterCustomizationData data)
    {
        characterCustomizationData = data;
    }
}
