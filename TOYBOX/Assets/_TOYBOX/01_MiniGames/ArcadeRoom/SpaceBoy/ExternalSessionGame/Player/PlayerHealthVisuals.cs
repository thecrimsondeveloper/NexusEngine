using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class PlayerHealthVisuals : MonoBehaviour
    {
        [SerializeField] List<SpriteRenderer> hearts = new List<SpriteRenderer>();

        [SerializeField] Sprite fullHeart;
        [SerializeField] Sprite halfHeart;
        [SerializeField] Sprite emptyHeart;




        // Start is called before the first frame update
        void Start()
        {
            foreach (SpriteRenderer heart in hearts)
            {
                heart.sprite = fullHeart;
            }
        }

        public void TakeDamage(int damage)
        {
            for (int i = 0; i < damage; i++)
            {
                if (hearts[i].sprite == fullHeart)
                {
                    hearts[i].sprite = halfHeart;
                }
                else if (hearts[i].sprite == halfHeart)
                {
                    hearts[i].sprite = emptyHeart;
                    //remove the heart from the list
                    hearts.RemoveAt(i);
                }
            }
        }

        public void RestoreHealth(int health)
        {
            for (int i = 0; i < health; i++)
            {
                if (hearts[i].sprite == emptyHeart)
                {
                    hearts[i].sprite = halfHeart;
                }
                else if (hearts[i].sprite == halfHeart)
                {
                    hearts[i].sprite = fullHeart;
                    //add a heart to the list
                    hearts.Add(hearts[i]);
                }
            }
        }
    }
}
