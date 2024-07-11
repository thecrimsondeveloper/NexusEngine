using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Toolkit.NexusEngine
{
    public class NexusItemShop : MonoBehaviour
    {
        [SerializeField] TMP_Text itemNameText;
        [SerializeField] TMP_Text costText;
        [SerializeField] SpriteRenderer itemSprite;
        [SerializeField] Transform shopItemSpawnParent;
        [SerializeField] NexusShopItemDefinition shopItem;
        [SerializeField] NexusScoreHandler defaultScoreHandler;

        private void Start()
        {
            itemNameText.text = shopItem.itemName;
            costText.text = "Cost: " + shopItem.cost.ToString();
            itemSprite.sprite = shopItem.itemIcon;
        }


        public void PurchaseItem(NexusScoreHandler scoreHandler)
        {
            // Purchase logic
            if (defaultScoreHandler.Score >= shopItem.cost)
            {
                defaultScoreHandler.RemoveScore(shopItem.cost);
                SpawnItem();
            }
            else
            {

            }
        }

        [Button]
        public void PurchaseItem()
        {
            // Purchase logic
            PurchaseItem(defaultScoreHandler);
        }

        [Button]
        public async void SpawnItem()
        {
            if (shopItemSpawnParent.childCount > 0)
            {
                foreach (Transform child in shopItemSpawnParent)
                {
                    Destroy(child.gameObject);
                }
            }

            GameObject obj = Instantiate(shopItem.prefab, shopItemSpawnParent);

            obj.transform.localPosition = shopItem.spawnOffsetPosition;
            obj.transform.localEulerAngles = shopItem.spawnOffsetRotation;
        }
    }



}
