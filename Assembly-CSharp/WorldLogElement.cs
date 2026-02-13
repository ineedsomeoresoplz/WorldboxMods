// Decompiled with JetBrains decompiler
// Type: WorldLogElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldLogElement : MonoBehaviour
{
  public Text date;
  public Text description;
  public Image icon;
  public GameObject locate;
  public GameObject follow;
  public WorldLogMessage message;

  public void showMessage(WorldLogMessage pMessage)
  {
    this.message = pMessage;
    Text date = this.date;
    int num = Date.getYear((double) this.message.timestamp);
    string str1 = num.ToString();
    num = Date.getMonth((double) this.message.timestamp);
    string str2 = num.ToString();
    string str3 = $"y:{str1}, m:{str2}";
    date.text = str3;
    string formatedText = this.message.getFormatedText(this.description);
    bool flag = this.message.hasLocation();
    if (this.message.hasFollowLocation())
    {
      this.follow.SetActive(true);
      this.locate.SetActive(false);
    }
    else
    {
      this.follow.SetActive(false);
      this.locate.SetActive(flag);
    }
    this.description.text = formatedText ?? "";
    ((Component) this.description).GetComponent<LocalizedText>().checkTextFont();
    string pathIcon = this.message.getAsset().path_icon;
    if (!string.IsNullOrEmpty(pathIcon))
      this.icon.sprite = SpriteTextureLoader.getSprite(pathIcon);
    else
      ((Component) this.icon).gameObject.SetActive(false);
    ((Component) this.description).GetComponent<LocalizedText>().checkSpecialLanguages();
  }

  public void clickLocate() => this.message.jumpToLocation();
}
