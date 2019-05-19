using LitJson;

public class News
{
    public int id;
    public string title;
    public string shortDescription;
    public string content;
    //2018-05-30
    public string datetime;
    public string linkImg;

    public static News FromJson(JsonData jsonData)
    {
        News news = new News();
        news.id = int.Parse(jsonData["id"].ToString());
        news.title = jsonData["title"].ToString();
        news.shortDescription = jsonData["shortDescription"].ToString();
        news.content = jsonData["content"].ToString();
        news.datetime = jsonData["datetime"].ToString();
        news.linkImg = jsonData["linkImg"].ToString();
        return news;
    }
}

public enum TypeNews
{
    title = 0,
    text,
    image
}
public class UnitNews
{
    public TypeNews type;
    public string text;
}
[System.Serializable]
public class ImageNews : UnitNews
{
    public string urlImage;
}