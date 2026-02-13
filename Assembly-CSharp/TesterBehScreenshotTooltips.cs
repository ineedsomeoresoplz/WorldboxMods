// Decompiled with JetBrains decompiler
// Type: TesterBehScreenshotTooltips
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class TesterBehScreenshotTooltips : BehaviourActionTester
{
  private int screenshots;
  private TooltipScreenshotState state;
  private List<ButtonTrigger> triggers = new List<ButtonTrigger>();
  private ButtonTrigger activeTrigger;
  private bool _screenshot;

  public TesterBehScreenshotTooltips(bool pScreenshot = true) => this._screenshot = pScreenshot;

  public override BehResult execute(AutoTesterBot pObject)
  {
    string screenshotFolder = TesterBehScreenshotFolder.getScreenshotFolder(LocalizedTextManager.instance.language);
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    string screenId = currentWindow.screen_id;
    RectTransform component1 = ((Component) ((Component) currentWindow).transform.FindRecursive("Viewport")).gameObject.GetComponent<RectTransform>();
    string str1 = ((int) ((Transform) ((Component) ((Component) currentWindow).transform.FindRecursive("Content")).gameObject.GetComponent<RectTransform>()).localPosition.y).ToString("D4");
    switch (this.state)
    {
      case TooltipScreenshotState.Load:
        foreach (Button componentsInChild in ((Component) currentWindow).gameObject.GetComponentsInChildren<Button>())
        {
          if (((Behaviour) componentsInChild).isActiveAndEnabled && ((Component) componentsInChild).gameObject.activeInHierarchy)
          {
            EventTrigger component2 = ((Component) componentsInChild).gameObject.GetComponent<EventTrigger>();
            if (!Object.op_Equality((Object) component2, (Object) null) && !(((Object) componentsInChild).name == "Close"))
            {
              if (Object.op_Inequality((Object) ((Component) ((Component) componentsInChild).transform).GetComponentInParent<ScrollWindow>(), (Object) null))
              {
                Rect worldRect = ((Component) componentsInChild).gameObject.GetComponent<RectTransform>().GetWorldRect();
                if (!((Rect) ref worldRect).Overlaps(component1.GetWorldRect()))
                  continue;
              }
              int num = 0;
              foreach (EventTrigger.Entry trigger in component2.triggers)
              {
                if (trigger.eventID == null)
                  this.triggers.Add(new ButtonTrigger(componentsInChild, trigger, ++num));
              }
            }
          }
        }
        this.state = TooltipScreenshotState.NextTrigger;
        return BehResult.RepeatStep;
      case TooltipScreenshotState.Screenshot:
        if (!Tooltip.anyActive())
        {
          this.state = TooltipScreenshotState.NextTrigger;
          return BehResult.RepeatStep;
        }
        if (this._screenshot)
        {
          ++this.screenshots;
          string str2 = "";
          if (this.activeTrigger.index > 1)
            str2 = "_" + this.activeTrigger.index.ToString();
          string str3 = $"{screenId}_{str1}_{this.screenshots.ToString("D3")}_{((Object) this.activeTrigger.button).name}{str2}_5";
          ScreenCapture.CaptureScreenshot($"{screenshotFolder}/{str3}.png");
        }
        this.state = TooltipScreenshotState.Cleanup;
        return BehResult.RepeatStep;
      case TooltipScreenshotState.Cleanup:
        Tooltip.hideTooltipNow();
        this.state = TooltipScreenshotState.NextTrigger;
        return BehResult.RepeatStep;
      case TooltipScreenshotState.NextTrigger:
        if (this.triggers.Count == 0)
        {
          this.state = TooltipScreenshotState.Finish;
          return BehResult.RepeatStep;
        }
        this.activeTrigger = this.triggers.Shift<ButtonTrigger>();
        if (!((Behaviour) this.activeTrigger.button).isActiveAndEnabled)
        {
          Debug.LogWarning((object) ("button was already disabled: " + ((Object) this.activeTrigger.button).name), (Object) this.activeTrigger.button);
          return BehResult.RepeatStep;
        }
        ((UnityEvent<BaseEventData>) this.activeTrigger.entry.callback).Invoke(new BaseEventData(EventSystem.current));
        this.state = TooltipScreenshotState.Screenshot;
        pObject.wait = 0.01f;
        return BehResult.RepeatStep;
      case TooltipScreenshotState.Finish:
        this.state = TooltipScreenshotState.Load;
        this.screenshots = 0;
        this.activeTrigger = new ButtonTrigger();
        this.triggers.Clear();
        return BehResult.Continue;
      default:
        Debug.LogError((object) ("TesterBehScreenshotTooltips: Unknown state: " + this.state.ToString()));
        return BehResult.Stop;
    }
  }
}
