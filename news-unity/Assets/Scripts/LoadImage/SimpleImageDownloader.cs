using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace LoadImage
{
    public class SimpleImageDownloader : MonoSingleton<SimpleImageDownloader>
    {
        public int MaxConcurrentRequests { get; set; }

        const int DEFAULT_MAX_CONCURRENT_REQUESTS = 2;

        List<Request> _QueuedRequests = new List<Request>();
        List<Request> _ExecutingRequests = new List<Request>();
        WaitForSeconds _Wait1Sec = new WaitForSeconds(1f);


        IEnumerator Start()
        {
            if (MaxConcurrentRequests == 0)
                MaxConcurrentRequests = DEFAULT_MAX_CONCURRENT_REQUESTS;

            while (true)
            {
                while (_ExecutingRequests.Count >= MaxConcurrentRequests)
                {
                    yield return _Wait1Sec;
                }

                if (_QueuedRequests.Count > 0)
                {
                    var first = _QueuedRequests[0];
                    _QueuedRequests.RemoveAt(0);
                    StartCoroutine(DownloadCoroutine(first));
                }
                yield return null;
            }
        }

        public void Enqueue(Request request)
        {
            _QueuedRequests.Add(request);
        }
        public void RemoveRequest(Request request)
        {
            if (_QueuedRequests.Contains(request))
            {
                _QueuedRequests.Remove(request);
            }
        }
        public void RemoveAndStopRequest(Request request)
        {
            if (_QueuedRequests.Contains(request))
            {
                _QueuedRequests.Remove(request);
            }
            else if (_ExecutingRequests.Contains(request))
            {
                StopCoroutine(DownloadCoroutine(request));
                _ExecutingRequests.Remove(request);
            }
        }
        IEnumerator DownloadCoroutine(Request request)
        {
            _ExecutingRequests.Add(request);
            var www = new WWW(request.url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                if (request != null && request.onDone != null)
                {
                    var result = new Result(www);
                    request.onDone(result);
                }
            }
            else
            {
                if (request != null && request.onError != null)
                {
#if UNITY_EDITOR
                    Debug.LogError(string.Format("DownLoad Fail: \nImage Url: {0} \n Message: {1}", request.url, www.error));
#endif
                    request.onError();
                }
            }
            www.Dispose();
            _ExecutingRequests.Remove(request);
        }
    }

    public class Request
    {
        public string url;
        public Action<Result> onDone;
        public Action onError;
    }

    public class Result
    {
        WWW _UsedWWW;
        public Result(WWW www)
        { _UsedWWW = www; }
        

        public Texture2D CreateTextureFromReceivedData()
        { 
            return _UsedWWW.texture; 
        }

        public byte[] GetByte()
        {
            return _UsedWWW.bytes;
        }

        public void LoadTextureInto(Texture2D existingTexture)
        {
            _UsedWWW.LoadImageIntoTexture(existingTexture);
        }
    }
}