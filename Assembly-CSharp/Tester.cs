// Decompiled with JetBrains decompiler
// Type: Tester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Tester : MonoBehaviour
{
  public GlowParticles smoke;
  public GlowParticles fire;
  public TestStage testStage;
  private List<TestingEvent> events;
  private List<TestingEvent> eventsCivs;
  private float timer = 1f;
  public float testStageTimer = 20f;
  public bool enableFastBuilding;
  public bool enableRandomSpawn = true;

  private void init()
  {
    this.events = new List<TestingEvent>();
    this.eventsCivs = new List<TestingEvent>();
    foreach (GodPower godPower in AssetManager.powers.list)
    {
      if (godPower.id[0] != '_')
      {
        TestingEvent pEvent = this.add(new TestingEvent()
        {
          type = TestingEventType.RandomClick,
          powerID = godPower.id
        });
        if (godPower.type == PowerActionType.PowerDrawTile)
        {
          this.add(pEvent);
          this.add(pEvent);
          this.add(pEvent);
        }
      }
    }
    this.eventsCivs.Add(new TestingEvent()
    {
      powerID = "humans",
      type = TestingEventType.RandomClick
    });
    this.eventsCivs.Add(new TestingEvent()
    {
      powerID = "orcs",
      type = TestingEventType.RandomClick
    });
    this.eventsCivs.Add(new TestingEvent()
    {
      powerID = "elves",
      type = TestingEventType.RandomClick
    });
    this.eventsCivs.Add(new TestingEvent()
    {
      powerID = "dwarfs",
      type = TestingEventType.RandomClick
    });
    this.setTestStage(TestStage.SPAWN_CIVS);
    ((Behaviour) this.smoke).enabled = false;
    ((Behaviour) this.fire).enabled = false;
  }

  private void setTestStage(TestStage pStage)
  {
    this.testStage = pStage;
    switch (this.testStage)
    {
      case TestStage.SPAWN_CIVS:
        this.testStageTimer = 10f;
        break;
      case TestStage.WAIT_CIVS:
        this.testStageTimer = 60f;
        break;
      case TestStage.SPAWN_CHAOS:
        this.testStageTimer = 30f;
        break;
      case TestStage.REGENERATE:
        this.testStageTimer = 1f;
        break;
    }
  }

  private TestingEvent add(TestingEvent pEvent)
  {
    this.events.Add(pEvent);
    return pEvent;
  }

  private void Update()
  {
    if (!Config.game_loaded)
      return;
    if (this.events == null)
    {
      this.init();
    }
    else
    {
      if (!this.enableRandomSpawn)
        return;
      if ((double) this.timer > 0.0)
        this.timer -= Time.deltaTime;
      else if ((double) this.testStageTimer > 0.0)
      {
        this.testStageTimer -= Time.deltaTime;
        TestingEvent random;
        switch (this.testStage)
        {
          case TestStage.SPAWN_CIVS:
            random = this.eventsCivs.GetRandom<TestingEvent>();
            break;
          case TestStage.SPAWN_CHAOS:
            random = this.events.GetRandom<TestingEvent>();
            break;
          default:
            return;
        }
        ScrollWindow.hideAllEvent(false);
        if (random == null)
          return;
        if (random.type == TestingEventType.RandomClick)
        {
          int num1 = Randy.randomInt(0, MapBox.width);
          int num2 = Randy.randomInt(0, MapBox.height);
          LogText.log(random.powerID, "Test Power", "st");
          if (!AssetManager.powers.dict.ContainsKey(random.powerID))
            MonoBehaviour.print((object) ("TESTER ERROR... " + random.powerID));
          GodPower pPower = AssetManager.powers.get(random.powerID);
          if (!pPower.tester_enabled)
            return;
          Config.current_brush = Brush.getRandom();
          World.world.player_control.clickedFinal(new Vector2Int(num1, num2), pPower);
          LogText.log(random.powerID, "Test Power", "en");
        }
      }
      else
      {
        switch (this.testStage)
        {
          case TestStage.SPAWN_CIVS:
            this.setTestStage(TestStage.WAIT_CIVS);
            break;
          case TestStage.WAIT_CIVS:
            this.setTestStage(TestStage.SPAWN_CHAOS);
            break;
          case TestStage.SPAWN_CHAOS:
            this.setTestStage(TestStage.REGENERATE);
            break;
          case TestStage.REGENERATE:
            Config.customZoneX = 7;
            Config.customZoneY = 7;
            World.world.generateNewMap();
            this.testStageTimer = 20f;
            this.setTestStage(TestStage.SPAWN_CIVS);
            break;
        }
      }
    }
  }
}
