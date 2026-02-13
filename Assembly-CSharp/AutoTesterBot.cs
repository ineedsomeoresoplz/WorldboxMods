// Decompiled with JetBrains decompiler
// Type: AutoTesterBot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AutoTesterBot : BaseMapObject
{
  internal string debugString = "";
  internal bool active;
  internal AiSystemTester ai;
  internal float wait;
  internal int beh_year_target;
  internal WorldTile beh_tile_target;
  internal string beh_asset_target;
  internal string active_tester = "";
  private Image _icon;

  public Image icon
  {
    get
    {
      if (Object.op_Equality((Object) this._icon, (Object) null))
        this._icon = ((Component) ((Component) this).transform.Find("Icon")).GetComponent<Image>();
      return this._icon;
    }
  }

  internal void clearWorld()
  {
    this.beh_tile_target = (WorldTile) null;
    this.beh_asset_target = (string) null;
    this.beh_year_target = 0;
    this.ai?.restartJob();
  }

  internal override void create()
  {
    if (this.ai != null)
    {
      this.ai.reset();
      this.ai = (AiSystemTester) null;
    }
    base.create();
    this.ai = new AiSystemTester(this);
    this.ai.next_job_delegate = new GetNextJobID(AssetManager.tester_jobs.getNextJob);
    DebugConfig.createTool("Auto Tester", 150, 0);
    this.startAutoTester();
  }

  internal void create(string pJob)
  {
    if (this.ai != null)
    {
      this.ai.reset();
      this.ai = (AiSystemTester) null;
    }
    base.create();
    this.active_tester = pJob;
    List<string> tJobs = new List<string>() { pJob };
    this.ai = new AiSystemTester(this);
    this.ai.next_job_delegate = (GetNextJobID) (() =>
    {
      if (tJobs.Count != 0)
        return tJobs.Pop<string>();
      this.active = false;
      ((Component) this).gameObject.SetActive(false);
      return (string) null;
    });
    this.startAutoTester();
  }

  public override void update(float pElapsed)
  {
    if (!this.active)
      return;
    base.update(pElapsed);
    if ((double) this.wait > 0.0)
      this.wait -= pElapsed;
    else
      this.ai.update();
  }

  private void updateButton()
  {
    if (this.active)
    {
      this.icon.sprite = SpriteTextureLoader.getSprite("ui/Icons/iconPause");
      WorldTip.instance.showToolbarText("Auto tester running");
    }
    else
    {
      this.icon.sprite = SpriteTextureLoader.getSprite("ui/Icons/iconPlay");
      WorldTip.instance.showToolbarText("Auto tester paused");
    }
  }

  public void startAutoTester()
  {
    this.active = true;
    this.updateButton();
  }

  public void stopAutoTester()
  {
    this.active = false;
    this.updateButton();
  }

  public void toggleAutoTester()
  {
    if (this.ai == null)
      this.create();
    this.active = !this.active;
    this.updateButton();
  }
}
