using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoadImage
{
    public class RemoteImageBase : MonoBehaviour
    {
        public Image imgResult;

        string currentUrl;
        Request request;
        // Load and hot save
        public Request Load(string imageURL, Action<Texture2D> onCompleted = null)
        {
            if (request != null)
            {
                SimpleImageDownloader.Instance.RemoveAndStopRequest(request);
                request = null;
            }
            currentUrl = imageURL;
            ShowLoading();
            request = new Request()
            {
                url = imageURL,
                onDone = result =>
                {
                    if (currentUrl == imageURL) // this will be false if a new request was done during downloading, case in which the result will be ignored
                    {
                        Texture2D texToUse = result.CreateTextureFromReceivedData();
                        texToUse.filterMode = FilterMode.Trilinear;
                        texToUse.anisoLevel = 0;
                        ShowImage(texToUse);
                        if (onCompleted != null)
                            onCompleted(texToUse);
                    }
                    request = null;
                },
                onError = () =>
                {
                    ShowError();
                    if (onCompleted != null)
                        onCompleted(null);
                    request = null;
                }
            };
            SimpleImageDownloader.Instance.Enqueue(request);
            return request;
        }

        protected virtual void ShowImage(Texture2D texToUse)
        {
            imgResult.sprite = TextureToSprite(texToUse);
            imgResult.preserveAspect = true;
        }
        protected virtual void ShowLoading()
        {
        }
        protected virtual void ShowError()
        {
        }
        public Sprite TextureToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        private void OnDestroy()
        {
            UnLoad();
        }
        public void UnLoad()
        {
            if (request != null)
            {
                SimpleImageDownloader.Instance.RemoveAndStopRequest(request);
                request = null;
            }
        }
    }
}