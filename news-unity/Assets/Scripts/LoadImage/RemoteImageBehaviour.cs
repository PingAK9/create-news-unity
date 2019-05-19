using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace LoadImage
{
    public class RemoteImageBehaviour : RemoteImageBase
    {
        public RawImage _RawImage;
        public GameObject _RawLoading;
        public GameObject _RawError;

        protected override void ShowImage(Texture2D texToUse)
        {
            if (_RawError) _RawError.gameObject.SetActive(false);
            if (_RawLoading) _RawLoading.gameObject.SetActive(false);
            if (_RawImage)
            {
                _RawImage.gameObject.SetActive(true);
                _RawImage.texture = texToUse;
            }
            if (imgResult)
            {
                imgResult.gameObject.SetActive(true);
                imgResult.sprite = TextureToSprite(texToUse);
                imgResult.preserveAspect = true;
            }

        }
        protected override void ShowLoading()
        {
            if (_RawError) _RawError.gameObject.SetActive(false);
            if (_RawLoading)
            {
                _RawLoading.gameObject.SetActive(true);
                if (_RawImage) _RawImage.gameObject.SetActive(false);
                if (imgResult) imgResult.gameObject.SetActive(false);
            }
            else
            {
                if (_RawImage) _RawImage.gameObject.SetActive(true);
                if (imgResult) imgResult.gameObject.SetActive(true);
            }
        }
        protected override void ShowError()
        {
            if (_RawLoading) _RawLoading.gameObject.SetActive(false);
            if (_RawError)
            {
                _RawError.gameObject.SetActive(true);
                if (_RawImage) _RawImage.gameObject.SetActive(false);
                if (imgResult) imgResult.gameObject.SetActive(false);
            }
            else
            {
                if (_RawImage) _RawImage.gameObject.SetActive(true);
                if (imgResult) imgResult.gameObject.SetActive(true);
            }
        }
    }

}