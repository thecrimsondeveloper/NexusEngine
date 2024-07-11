using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ToyBox
{
    public class ImageLoader : MonoBehaviour
    {
        bool bothPopulated => image != null && spriteRenderer != null;
        bool shouldShowImage => image != null || bothPopulated;
        bool shouldShowSpriteRenderer => spriteRenderer != null || bothPopulated;

        public string URL = "https://picsum.photos/200/300";

        [SerializeField, ShowIf("shouldShowImage")] Image image;
        [SerializeField, ShowIf("shouldShowSpriteRenderer")] SpriteRenderer spriteRenderer;

        [Title("Editor Only")]
        [SerializeField] bool billboardToSceneView = true;
        [SerializeField, ShowIf(nameof(billboardToSceneView))] Vector3 rotationOffset = Vector3.zero;
        [SerializeField] bool followCameraAsIfChild = true;

        [SerializeField] Vector3 savedDirectionFromForwardToImage;
        void OnDrawGizmos()
        {


            if (billboardToSceneView)
            {
                transform.LookAt(Camera.current.transform);
                transform.Rotate(rotationOffset);
            }

            if (followCameraAsIfChild)
            {
                transform.position = Camera.current.transform.position +
                                    Camera.current.transform.forward * savedDirectionFromForwardToImage.z +
                                    Camera.current.transform.right * savedDirectionFromForwardToImage.x +
                                    Camera.current.transform.up * savedDirectionFromForwardToImage.y;
            }
            else
            {
                Vector3 direction = (transform.position - Camera.current.transform.position);
                Vector3 forward = Camera.current.transform.forward;

                Vector3 GetSaveDirection()
                {
                    return new Vector3(Vector3.Dot(direction, Camera.current.transform.right),
                                    Vector3.Dot(direction, Camera.current.transform.up),
                                    Vector3.Dot(direction, forward));
                }
                Debug.Log("Direction: " + direction + " Forward: " + forward + " SaveDirection: " + GetSaveDirection());

                savedDirectionFromForwardToImage = GetSaveDirection();
            }
        }

        void OnValidate()
        {
            CheckForRenderComponent();

        }

        void CheckForRenderComponent()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        bool isLoading = false;
        [Button("Load Image")]
        async void LoadInEditor()
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;
            Sprite sprite = await DownloadSprite(URL);

            if (image != null)
            {
                image.sprite = sprite;
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;
            }

            isLoading = false;
        }

        public static async UniTask<Sprite> DownloadSprite(string url)
        {
            Debug.Log("Downloading sprite from " + url);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            request.timeout = 10; // Set a timeout value for the request (in seconds)

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading sprite: " + request.error);
                return null;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            Debug.Log("Downloaded sprite from " + url + " with size " + texture.width + "x" + texture.height);
            return sprite;
        }

    }
}
