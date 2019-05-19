using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoadImage;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using LitJson;

public class GSNews : MonoBehaviour
{
    public static GSNews instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        instance = null;
    }
    public TextAsset jsonFile;
    News info;
    void Start()
    {
        SetData();
    }

    public RectTransform root;
    public float width { get { return root.rect.width; }}
    public GameObject pfTitle;
    public GameObject pfImage;
    public GameObject pfParagraph;
    public GameObject btnShare;
    public GameObject objBottom;
    public ScrollRect myScrollRect;

    List<UnitNews> datas;
    List<GameObject> objDatas;

    public Text txtTitle;
    public Text txtDay;
    public Text txtMonth;
    public RemoteImageBase remote;
    public GameObject refreshSize;

    public void SetData()
    {
        JsonData jsonDataAsset = JsonMapper.ToObject(jsonFile.text);
        info = News.FromJson(jsonDataAsset);

        txtTitle.text = info.title;
        DateTime date = new DateTime();
        try
        {
            date = DateTime.ParseExact(info.datetime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        txtDay.text = date.Day.ToString();
        txtMonth.text = "" + (date.Month) + "/" + (date.Year - 2000);
        remote.Load(info.linkImg);

        datas = ContentToData(info.content);
        RemoveAllChildren();
        for (int i = 0; i < datas.Count; i++)
        {
            GameObject _pf = null;
            switch (datas[i].type)
            {
                case TypeNews.title:
                    _pf = pfTitle;
                    break;
                case TypeNews.text:
                    _pf = pfParagraph;
                    break;
                case TypeNews.image:
                    _pf = pfImage;
                    break;
                default:
                    break;
            }
            GameObject _obj = Utils.Spawn(_pf, root);
            UIUnitNews _uiNews = _obj.GetComponent<UIUnitNews>();
            _uiNews.Init(datas[i]);
            objDatas.Add(_obj);
        }

        btnShare.transform.SetAsLastSibling();
        objBottom.transform.SetAsLastSibling();
        Invoke("ScrollTopPosition", 0.2f);
    }
    public void OnRefreshSize()
    {
        refreshSize.SetActive(!refreshSize.activeSelf);
    }
    void ScrollTopPosition()
    {
        myScrollRect.normalizedPosition = new Vector2(0, 1);
    }
    void RemoveAllChildren()
    {
        if (objDatas != null)
        {
            for (int i = 0; i < objDatas.Count; i++)
            {
                GameObject.DestroyImmediate(objDatas[i]);
            }
        }
        objDatas = new List<GameObject>();
    }
    public void OnBackClick()
    {
        // back
    }
    [TextArea(3, 5)]
    public string regex = "";
    public const string srcImageTag = "src";
    public const string altImageTag = "alt";
    public const string heading1 = "heading1";
    public const string heading2 = "heading2";
    public const string paragraphTag = "paragraph";
    List<UnitNews> ContentToData(string content)
    {
        content = content.Replace("\n", "");
        content = content.Replace("\r", "");
        content = content.Replace("<br /><br />", "<br />");
        content = content.Replace("<p><br /><img", "<p><img");

        List<UnitNews> _list = new List<UnitNews>();
        MatchCollection matchColl = Regex.Matches(content, regex);
        foreach (Match match in matchColl)
        {
            //Debug.Log(" ---------------- " + "\nValue: " + match.Value + "\nIndex: " + match.Index + "\nLength: " + match.Length);
            GroupCollection group = match.Groups;
            if (string.IsNullOrEmpty(group[srcImageTag].ToString()) == false)
            {
                ImageNews _item = new ImageNews();
                _item.type = TypeNews.image;
                _item.urlImage = group[srcImageTag].ToString();
                _item.text = group[altImageTag].ToString();
                if (string.IsNullOrEmpty(_item.urlImage) == false)
                {
                    _list.Add(_item);
                }
            }
            else if ((string.IsNullOrEmpty(group[heading1].ToString()) == false))
            {
                UnitNews _item = new UnitNews();
                _item.type = TypeNews.title;
                _item.text = group[heading1].ToString();
                if (string.IsNullOrEmpty(_item.text) == false)
                {
                    _list.Add(_item);
                }
            }
            else if ((string.IsNullOrEmpty(group[heading2].ToString()) == false))
            {
                UnitNews _item = new UnitNews();
                _item.type = TypeNews.title;
                _item.text = group[heading2].ToString();
                if (string.IsNullOrEmpty(_item.text) == false)
                {
                    _list.Add(_item);
                }
            }
            else if (string.IsNullOrEmpty(group[paragraphTag].ToString()) == false)
            {
                UnitNews _item = new UnitNews();
                _item.type = TypeNews.text;
                _item.text = group[paragraphTag].ToString();
                if (string.IsNullOrEmpty(_item.text) == false)
                {
                    _list.Add(_item);
                }
            }
            else
            {

            }
        }
        return _list;
    }
}
