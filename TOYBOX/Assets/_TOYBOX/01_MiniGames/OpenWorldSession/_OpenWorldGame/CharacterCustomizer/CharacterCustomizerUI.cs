using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using TMPro;

public class CharacterCustomizerUI : MonoBehaviour
{
    [SerializeField] PlayerVisualizer playerVisualizer;
    [SerializeField] TMP_Dropdown genderDropdown;
    [SerializeField] TMP_Dropdown hairColorDropdown;
    [SerializeField] Slider headSizeSlider;


    [SerializeField]
    CharacterCustomizationData characterCustomizationData;

    void Start()
    {
        genderDropdown.onValueChanged.AddListener(OnGenderValueChanged);
        hairColorDropdown.onValueChanged.AddListener(OnHairColorValueChanged);
        headSizeSlider.onValueChanged.AddListener(OnHeadSizeSliderChanged);

        LoadData();
    }

    void OnGenderValueChanged(int value)
    {
        Gender gender = (Gender)value;
        characterCustomizationData.gender = gender;
        SaveData();
    }

    void OnHairColorValueChanged(int value)
    {
        Color hairColor = Color.white;


        if (value == 0)
        {
            hairColor = Color.white;
        }
        else if (value == 1)
        {
            hairColor = Color.green;
        }
        else if (value == 2)
        {
            hairColor = Color.yellow;
        }
        else if (value == 3)
        {
            hairColor = Color.black;
        }

        characterCustomizationData.hairColor = hairColor;


        SaveData();
    }

    void OnHeadSizeSliderChanged(float value)
    {
        characterCustomizationData.headSize = value;

        playerVisualizer.SetLook(characterCustomizationData);
        SaveData();
    }

    void SaveData()
    {
        playerVisualizer.SetLook(characterCustomizationData);
        string json = JsonUtility.ToJson(characterCustomizationData);
        Debug.Log(json);

        //create a file in the assets folder
        PlayerPrefs.SetString("CharacterCustomizationData", json);
    }

    void LoadData()
    {
        // string json = PlayerPrefs.GetString("CharacterCustomizationData", "");
        string json = PlayerPrefs.GetString("CharacterCustomizationData", "");

        if (json != "")
        {
            characterCustomizationData = JsonUtility.FromJson<CharacterCustomizationData>(json);
            if (playerVisualizer) playerVisualizer.SetLook(characterCustomizationData);
        }
        else
        {
            Debug.LogWarning("No data found");
        }
    }

}
