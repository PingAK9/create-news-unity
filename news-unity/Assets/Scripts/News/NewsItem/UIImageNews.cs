using System.Collections;
using LoadImage;
using UnityEngine;
using UnityEngine.UI;

public class UIImageNews : UIUnitNews
{

    public Text txtCaption;
    public RemoteImageBase imgPanel;
    public int sizeCaption = 68;
    public ImageNews imgData;
    public override void Init(UnitNews _data)
    {
        imgData = _data as ImageNews;
        StartCoroutine(LoadImage());
    }
    IEnumerator LoadImage()
    {
        yield return new WaitForEndOfFrame();
        txtCaption.text = HtmlTextToUnityText.Convert(imgData.text);
        imgPanel.Load(imgData.urlImage, LoadImageFinish);
    }

    void LoadImageFinish(Texture texture)
    {
        float width = GSNews.instance.width;
        RectTransform rect = transform as RectTransform;
        RectTransform rectImage = imgPanel.transform as RectTransform;
        float heightImage = width * texture.height / texture.width;
        rectImage.sizeDelta = new Vector2(width, heightImage);
        if (string.IsNullOrEmpty(imgData.text) == false)
        {
            rect.sizeDelta = new Vector2(width, heightImage);
        }
        else
        {
            rect.sizeDelta = new Vector2(width, heightImage + sizeCaption);
        }
        GSNews.instance.OnRefreshSize();
    }
}
