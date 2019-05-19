using UnityEngine;

public static class Utils
{
    public const string dateFormat = "yyyy-MM-dd hh:mm:ss";
    /// FPS
    public const int _fps = 60;
    public static void SetFPS()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _fps;
    }

    public static TextAsset loadResourcesText(string param)
    {
        return Resources.Load<TextAsset>(param);
    }
    public static Sprite loadResourcesSprite(string param)
    {
        return Resources.Load<Sprite>("" + param);
    }

    public static GameObject Spawn(GameObject paramPrefab, Transform paramParent = null)
    {
        GameObject newObject = UnityEngine.Object.Instantiate(paramPrefab) as GameObject;
        newObject.transform.SetParent(paramParent);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = paramPrefab.transform.localScale;
        newObject.SetActive(true);
        return newObject;
    }
    public static void setActive(GameObject paramObject, bool paramValue)
    {
        if (paramObject != null)
            paramObject.SetActive(paramValue);
    }
    public static void RemoveAllChildren(Transform paramParent, bool paramInstant = true)
    {
        if (paramParent == null)
            return;
        for (int i = paramParent.childCount - 1; i >= 0; i--)
        {
            if (paramInstant)
            {
                GameObject.DestroyImmediate(paramParent.GetChild(i).gameObject);
            }
            else
            {
                paramParent.GetChild(i).gameObject.SetActive(false);
                GameObject.Destroy(paramParent.GetChild(i).gameObject);
            }
        }
    }
   
    public static string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }
}