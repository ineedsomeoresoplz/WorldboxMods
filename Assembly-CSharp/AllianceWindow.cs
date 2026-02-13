// Decompiled with JetBrains decompiler
// Type: AllianceWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class AllianceWindow : WindowMetaGeneric<Alliance, AllianceData>
{
  public NameInput mottoInput;
  public StatBar bar_experience;

  public override MetaType meta_type => MetaType.Alliance;

  protected override Alliance meta_object => SelectedMetas.selected_alliance;

  protected override void initNameInput()
  {
    base.initNameInput();
    // ISSUE: method pointer
    this.mottoInput.addListener(new UnityAction<string>((object) this, __methodptr(applyInputMotto)));
  }

  private void applyInputMotto(string pInput)
  {
    if (pInput == null || this.meta_object == null)
      return;
    this.meta_object.data.motto = pInput;
  }

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    Alliance metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this.mottoInput.setText(metaObject.getMotto());
    ((Graphic) this.mottoInput.textField).color = metaObject.getColor().getColorText();
  }

  internal override void showStatsRows()
  {
    Alliance metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.tryToShowActor("alliance_founder", metaObject.data.founder_actor_id, metaObject.data.founder_actor_name, pIconPath: "actor_traits/iconStupid");
    this.tryToShowMetaKingdom("alliance_founder_kingdom", metaObject.data.founder_kingdom_id, metaObject.data.founder_kingdom_name);
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.mottoInput.inputField.DeactivateInputField();
  }
}
