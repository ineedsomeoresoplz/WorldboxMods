// Decompiled with JetBrains decompiler
// Type: MapBorder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MapBorder : BaseEffect
{
  private int currentState;
  private WorldTimer updateTimer;
  private WorldTimer alphaTimer;
  private int curWidth;
  private int curHeight;

  internal override void create()
  {
    base.create();
    this.updateTimer = new WorldTimer(0.12f, new Action(this.updateEffect));
    this.alphaTimer = new WorldTimer(0.02f, new Action(this.updateAlpha));
  }

  internal void generateTexture()
  {
    if (this.curWidth == MapBox.width && this.curHeight == MapBox.height)
      return;
    this.curWidth = MapBox.width;
    this.curHeight = MapBox.height;
    SpriteRenderer component = ((Component) this).gameObject.GetComponent<SpriteRenderer>();
    Texture2D texture2D = new Texture2D(this.curWidth, this.curHeight, (TextureFormat) 4, false);
    ((Texture) texture2D).filterMode = (FilterMode) 0;
    ((Object) texture2D).name = $"MapBorder_{this.curWidth.ToString()}x{this.curHeight.ToString()}";
    int length = ((Texture) texture2D).height * ((Texture) texture2D).width;
    Color32[] color32Array = new Color32[length];
    List<int> intList = new List<int>();
    List<int> collection = new List<int>();
    collection.Clear();
    int num1 = 0;
    int num2 = 0;
    for (int index = 0; index < length; ++index)
    {
      if (num2 == 0 && !intList.Contains(index))
        collection.Add(index);
      ++num1;
      if (num1 >= this.curWidth)
      {
        num1 = 0;
        ++num2;
      }
    }
    intList.AddRange((IEnumerable<int>) collection);
    collection.Clear();
    int num3 = 0;
    int num4 = 0;
    for (int index = 0; index < length; ++index)
    {
      if (num3 == this.curWidth - 1 && !intList.Contains(index))
        collection.Add(index);
      ++num3;
      if (num3 >= this.curWidth)
      {
        num3 = 0;
        ++num4;
      }
    }
    intList.AddRange((IEnumerable<int>) collection);
    collection.Clear();
    int num5 = 0;
    int num6 = 0;
    for (int index = 0; index < length; ++index)
    {
      if (num6 == this.curHeight - 1 && !intList.Contains(index))
        collection.Add(index);
      ++num5;
      if (num5 >= this.curWidth)
      {
        num5 = 0;
        ++num6;
      }
    }
    intList.AddRange((IEnumerable<int>) collection);
    collection.Clear();
    int num7 = 0;
    int num8 = 0;
    for (int index = 0; index < length; ++index)
    {
      if (num7 == 0 && !intList.Contains(index))
        collection.Add(index);
      ++num7;
      if (num7 >= this.curWidth)
      {
        num7 = 0;
        ++num8;
      }
    }
    collection.Reverse();
    intList.AddRange((IEnumerable<int>) collection);
    int num9 = 0;
    for (int index1 = 0; index1 < intList.Count; ++index1)
    {
      int index2 = intList[index1];
      if (num9 == 0 || num9 == 1 || num9 == 2)
      {
        color32Array[index2] = Color32.op_Implicit(Color.white);
        ++num9;
      }
      else
        num9 = 0;
    }
    component.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).width, (float) ((Texture) texture2D).height), new Vector2(0.5f, 0.5f), 1f);
    texture2D.SetPixels32(color32Array);
    texture2D.Apply();
    ((Component) this).gameObject.transform.localPosition = new Vector3((float) (this.curWidth / 2), (float) (this.curHeight / 2));
  }

  private void Update()
  {
    this.updateTimer.update();
    this.alphaTimer.update();
  }

  private void updateAlpha()
  {
    if (Object.op_Equality((Object) World.world.selected_buttons.selectedButton, (Object) null))
    {
      this.alpha -= 0.02f;
      if ((double) this.alpha < 0.0)
        this.alpha = 0.0f;
    }
    else
    {
      this.alpha += 0.02f;
      if ((double) this.alpha > 0.41999998688697815)
        this.alpha = 0.42f;
    }
    if ((double) this.sprite_renderer.color.a == (double) this.alpha)
      return;
    this.setAlpha(this.alpha);
  }

  private void updateEffect()
  {
    if ((double) this.alpha == 0.0)
      return;
    ++this.currentState;
    if (this.currentState > 3)
      this.currentState = 0;
    switch (this.currentState)
    {
      case 0:
        this.sprite_renderer.flipX = false;
        this.sprite_renderer.flipY = false;
        break;
      case 1:
        this.sprite_renderer.flipX = true;
        this.sprite_renderer.flipY = false;
        break;
      case 2:
        this.sprite_renderer.flipX = true;
        this.sprite_renderer.flipY = true;
        break;
      case 3:
        this.sprite_renderer.flipX = false;
        this.sprite_renderer.flipY = true;
        break;
    }
  }
}
