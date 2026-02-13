// Decompiled with JetBrains decompiler
// Type: ArmyMemberIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ArmyMemberIcons : ArmyElement
{
  [SerializeField]
  private UnitAvatarLoader _top;
  [SerializeField]
  private UnitAvatarLoader _top_left;
  [SerializeField]
  private UnitAvatarLoader _top_right;
  [SerializeField]
  private UnitAvatarLoader _left;
  [SerializeField]
  private UnitAvatarLoader _right;
  [SerializeField]
  private UnitAvatarLoader _bottom;
  [SerializeField]
  private UnitAvatarLoader _bottom_left;
  [SerializeField]
  private UnitAvatarLoader _bottom_right;
  [SerializeField]
  private ArmyBanner _banner;
  private UnitAvatarLoader[] _list_warrior_avatars;

  protected override void Awake()
  {
    this._list_warrior_avatars = new UnitAvatarLoader[8]
    {
      this._top,
      this._top_left,
      this._right,
      this._bottom_right,
      this._bottom,
      this._bottom_left,
      this._left,
      this._top_right
    };
    base.Awake();
  }

  protected override void clear()
  {
    foreach (Component listWarriorAvatar in this._list_warrior_avatars)
      listWarriorAvatar.gameObject.SetActive(false);
    ((Component) this._banner).gameObject.SetActive(false);
  }

  protected override IEnumerator showContent()
  {
    ArmyMemberIcons armyMemberIcons = this;
    ((Component) armyMemberIcons._banner).gameObject.SetActive(true);
    armyMemberIcons._banner.load((NanoObject) armyMemberIcons.army);
    bool flag;
    using (ListPool<Actor> tUnits = new ListPool<Actor>(armyMemberIcons.army.getUnits()))
    {
      if (tUnits.Count == 0)
      {
        flag = false;
      }
      else
      {
        tUnits.Shuffle<Actor>();
        for (int i = 0; i < armyMemberIcons._list_warrior_avatars.Length; ++i)
        {
          yield return (object) new WaitForEndOfFrame();
          if (tUnits.Count != 0)
          {
            UnitAvatarLoader listWarriorAvatar = armyMemberIcons._list_warrior_avatars[i];
            Actor pActor = tUnits.Pop<Actor>();
            ((Component) listWarriorAvatar).gameObject.SetActive(true);
            listWarriorAvatar.load(pActor);
          }
          else
            break;
        }
        flag = false;
      }
    }
    return flag;
  }
}
