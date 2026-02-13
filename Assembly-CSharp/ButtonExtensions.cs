// Decompiled with JetBrains decompiler
// Type: ButtonExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public static class ButtonExtensions
{
  public static void TriggerHover(this Button button)
  {
    if (!Input.mousePresent)
      return;
    EventTrigger eventTrigger = ((Component) button).gameObject.GetComponent<EventTrigger>();
    if (Object.op_Equality((Object) eventTrigger, (Object) null))
      eventTrigger = ((Component) button).gameObject.AddComponent<EventTrigger>();
    eventTrigger.OnPointerEnter(new PointerEventData(EventSystem.current));
  }

  public static void OnHover(this Button button, UnityAction call)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ButtonExtensions.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10 = new ButtonExtensions.\u003C\u003Ec__DisplayClass1_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.call = call;
    if (!Input.mousePresent)
      return;
    EventTrigger eventTrigger = ((Component) button).gameObject.GetComponent<EventTrigger>();
    if (Object.op_Equality((Object) eventTrigger, (Object) null))
      eventTrigger = ((Component) button).gameObject.AddComponent<EventTrigger>();
    EventTrigger.Entry entry = new EventTrigger.Entry();
    entry.eventID = (EventTriggerType) 0;
    // ISSUE: method pointer
    ((UnityEvent<BaseEventData>) entry.callback).AddListener(new UnityAction<BaseEventData>((object) cDisplayClass10, __methodptr(\u003COnHover\u003Eb__0)));
    eventTrigger.triggers.Add(entry);
  }

  public static void OnHoverOut(this Button button, UnityAction call)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ButtonExtensions.\u003C\u003Ec__DisplayClass2_0 cDisplayClass20 = new ButtonExtensions.\u003C\u003Ec__DisplayClass2_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass20.call = call;
    if (!Input.mousePresent)
      return;
    EventTrigger eventTrigger = ((Component) button).gameObject.GetComponent<EventTrigger>();
    if (Object.op_Equality((Object) eventTrigger, (Object) null))
      eventTrigger = ((Component) button).gameObject.AddComponent<EventTrigger>();
    EventTrigger.Entry entry = new EventTrigger.Entry();
    entry.eventID = (EventTriggerType) 1;
    // ISSUE: method pointer
    ((UnityEvent<BaseEventData>) entry.callback).AddListener(new UnityAction<BaseEventData>((object) cDisplayClass20, __methodptr(\u003COnHoverOut\u003Eb__0)));
    eventTrigger.triggers.Add(entry);
  }

  public static void OnHover(this Slider slider, UnityAction call)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ButtonExtensions.\u003C\u003Ec__DisplayClass3_0 cDisplayClass30 = new ButtonExtensions.\u003C\u003Ec__DisplayClass3_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass30.call = call;
    if (!Input.mousePresent)
      return;
    EventTrigger eventTrigger = ((Component) slider).gameObject.GetComponent<EventTrigger>();
    if (Object.op_Equality((Object) eventTrigger, (Object) null))
      eventTrigger = ((Component) slider).gameObject.AddComponent<EventTrigger>();
    EventTrigger.Entry entry = new EventTrigger.Entry();
    entry.eventID = (EventTriggerType) 0;
    // ISSUE: method pointer
    ((UnityEvent<BaseEventData>) entry.callback).AddListener(new UnityAction<BaseEventData>((object) cDisplayClass30, __methodptr(\u003COnHover\u003Eb__0)));
    eventTrigger.triggers.Add(entry);
  }

  public static void OnHoverOut(this Slider slider, UnityAction call)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ButtonExtensions.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new ButtonExtensions.\u003C\u003Ec__DisplayClass4_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.call = call;
    if (!Input.mousePresent)
      return;
    EventTrigger eventTrigger = ((Component) slider).gameObject.GetComponent<EventTrigger>();
    if (Object.op_Equality((Object) eventTrigger, (Object) null))
      eventTrigger = ((Component) slider).gameObject.AddComponent<EventTrigger>();
    EventTrigger.Entry entry = new EventTrigger.Entry();
    entry.eventID = (EventTriggerType) 1;
    // ISSUE: method pointer
    ((UnityEvent<BaseEventData>) entry.callback).AddListener(new UnityAction<BaseEventData>((object) cDisplayClass40, __methodptr(\u003COnHoverOut\u003Eb__0)));
    eventTrigger.triggers.Add(entry);
  }
}
