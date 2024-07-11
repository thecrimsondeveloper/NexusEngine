using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToyBox
{
    public class PlayerAmmoVisuals : MonoBehaviour
    {
        [SerializeField] List<SpriteRenderer> bombs = new List<SpriteRenderer>();
        [SerializeField] PlayerShoot playerShoot;

        // Start is called before the first frame update
        void Start()
        {
            foreach (SpriteRenderer bomb in bombs)
            {
                bomb.enabled = true;
            }
        }

        public void SubtractAmmo(int ammo)
        {
            foreach (SpriteRenderer bomb in bombs)
            {
                if (ammo > 0 && bomb.enabled == true)
                {
                    bomb.enabled = false;
                    ammo--;
                }
            }
        }

        public void AddAmmo(int ammo)
        {
            foreach (SpriteRenderer bomb in bombs)
            {
                if (ammo > 0 && bomb.enabled == false)
                {
                    bomb.enabled = true;
                    playerShoot.currentBullets++;
                    ammo--;
                }
            }
        }

        public void ResetAmmo()
        {
            foreach (SpriteRenderer bomb in bombs)
            {
                bomb.enabled = true;
            }
        }


    }
}
