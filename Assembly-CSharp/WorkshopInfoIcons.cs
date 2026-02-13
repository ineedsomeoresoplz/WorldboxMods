// Decompiled with JetBrains decompiler
// Type: WorkshopInfoIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks.Ugc;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorkshopInfoIcons : MonoBehaviour
{
  public Text favorites;
  public Text upvotes;
  public Text comments;
  public Text subscription;

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    WorkshopMapData currentWorkshopMapData = SaveManager.currentWorkshopMapData;
    if (currentWorkshopMapData == null)
      return;
    Text favorites = this.favorites;
    ulong num = ((Item) ref currentWorkshopMapData.workshop_item).NumFavorites;
    string str1 = num.ToString();
    favorites.text = str1;
    this.upvotes.text = ((Item) ref currentWorkshopMapData.workshop_item).VotesUp.ToString();
    Text comments = this.comments;
    num = ((Item) ref currentWorkshopMapData.workshop_item).NumComments;
    string str2 = num.ToString();
    comments.text = str2;
    Text subscription = this.subscription;
    num = ((Item) ref currentWorkshopMapData.workshop_item).NumSubscriptions;
    string str3 = num.ToString();
    subscription.text = str3;
  }
}
