// Decompiled with JetBrains decompiler
// Type: SelectedObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class SelectedObjects
{
  private static NanoObject _selected_nano_object;

  public static void unselectNanoObject()
  {
    SelectedObjects._selected_nano_object = (NanoObject) null;
    PowerTabController.prev_selected_meta_id = (string) null;
  }

  public static bool isNanoObjectSelected(NanoObject pNanoObject)
  {
    return SelectedObjects.isNanoObjectSet() && pNanoObject == SelectedObjects._selected_nano_object;
  }

  public static bool isNanoObjectSet() => !SelectedObjects._selected_nano_object.isRekt();

  public static void setNanoObject(NanoObject pNanoObject)
  {
    SelectedObjects._selected_nano_object = pNanoObject;
    SoundBox.click();
  }

  public static NanoObject getSelectedNanoObject() => SelectedObjects._selected_nano_object;
}
