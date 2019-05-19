using UnityEngine;
using UnityEngine.UI;

public class UITextNews : UIUnitNews {

    public Text txt;
    public override void Init(UnitNews _data)
    {
        txt.text = HtmlTextToUnityText.Convert(_data.text);
    }
}
