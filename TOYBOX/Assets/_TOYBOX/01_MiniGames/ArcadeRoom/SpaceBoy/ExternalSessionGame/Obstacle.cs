using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace ToyBox
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;

        [SerializeField] float horizontalMoveSpeed = 5f;

        [SerializeField] GameObject explosionVFX;

        [SerializeField] BombCollectable bombCollectablePrefab;

        [SerializeField] Animator animator;

        [SerializeField][Range(0, 1)] float chanceToSpawnBomb = 0.1f;

        bool isDestroyed = false;

        private void Awake()
        {
            explosionVFX.SetActive(false);
            isDestroyed = false;
        }

        private void Start()
        {
            Destroy(gameObject, 10f);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (isDestroyed)
            {
                return;
            }
            rb.velocity = new Vector2(-horizontalMoveSpeed, 0);
        }

        public async UniTask DestroySelf(float time)
        {
            explosionVFX.SetActive(true);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            isDestroyed = true;
            animator.Play("AsteroidBlink");
            if (Random.value < chanceToSpawnBomb)
            {
                Instantiate(bombCollectablePrefab, transform.position, Quaternion.identity);
            }
            await UniTask.Delay((int)((time) * 1000));
            Destroy(gameObject);
        }

    }
}
