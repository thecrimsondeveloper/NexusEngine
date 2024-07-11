using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField] Vector3 minimumHeadSize = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] CharacterCustomizationData characterCustomizationData;
    [SerializeField] Transform playerHeadTransform;
    [SerializeField] MeshRenderer playerHead;
    [SerializeField] MeshRenderer playerBody;

    private void Start()
    {
        string json = PlayerPrefs.GetString("CharacterCustomizationData", "");
        if (json == "") return;

        characterCustomizationData = JsonUtility.FromJson<CharacterCustomizationData>(json);
        RefreshLook();
    }
    public void SetLook(CharacterCustomizationData data)
    {
        characterCustomizationData = data;
        RefreshLook();
    }

    void RefreshLook()
    {
        playerHead.material.color = characterCustomizationData.hairColor;
        playerBody.material.color = characterCustomizationData.skinColor;

        playerHeadTransform.localScale = minimumHeadSize + (Vector3.one * characterCustomizationData.headSize);
    }

}
