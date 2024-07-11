using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ToyBox
{
    public class MediaDownloader : MonoBehaviour
    {
        [SerializeField] string url = "";
        [SerializeField] AudioSource source = null;

        [SerializeField] RawImage image = null;
        [SerializeField] VideoPlayer videoPlayer = null;

        [Button]
        async void SetImage()
        {
            //download the image
            image.texture = await DownloadImage(url);
        }

        [Button]
        async void SetAudio()
        {
            //download the audio
            source.clip = await DownloadAudio(url);
            source.Play();
        }

        [Button]
        async void SetVideo()
        {
            //download the video
            videoPlayer.url = url;
            videoPlayer.Prepare();
            await UniTask.WaitUntil(() => videoPlayer.isPrepared);
            videoPlayer.Play();
        }


        //downloads an image from the web and returns the path to the image
        async UniTask<Texture2D> DownloadImage(string url)
        {
            //create a new web request
            var request = new UnityWebRequest()
            {
                //set the request method to GET
                method = UnityWebRequest.kHttpVerbGET,
                //set the download handler to a texture download handler
                downloadHandler = new DownloadHandlerTexture(),
                //set the url to the url passed in
                url = url
            };
            //wait for the request to finish
            await request.SendWebRequest();

            //get the texture from the download handler
            var texture = (request.downloadHandler as DownloadHandlerTexture).texture;

            return texture;
        }

        public async UniTask<AudioClip> DownloadAudio(string url)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
            request.timeout = 10; // Set a timeout value for the request (in seconds)

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error downloading audio: " + request.error);
                return null;
            }

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
            return audioClip;
        }






    }

}
