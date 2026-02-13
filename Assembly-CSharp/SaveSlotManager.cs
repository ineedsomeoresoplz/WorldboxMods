// Decompiled with JetBrains decompiler
// Type: SaveSlotManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SaveSlotManager : MonoBehaviour
{
  public GameObject buttonsContainer;
  private List<LevelPreviewButton> previews = new List<LevelPreviewButton>();
  private List<GameObject> containers = new List<GameObject>();
  public GameObject slotButtonPrefab;
  public RectTransform content;
  private Vector3 originalPos;
  public bool loaded;
  public bool worldNetUpload;

  private void Init()
  {
    SaveManager.clearCurrentSelectedWorld();
    int num1 = 65;
    int num2 = 65;
    int num3 = 0;
    int num4 = 1;
    int num5 = 10;
    for (int index = 0; index < num5; ++index)
    {
      GameObject gameObject1 = Object.Instantiate<GameObject>(this.slotButtonPrefab, this.buttonsContainer.transform);
      gameObject1.transform.localPosition = new Vector3((float) -num1, (float) (-num3 * num2));
      GameObject pContainer1 = gameObject1;
      int pID1 = num4;
      int num6 = pID1 + 1;
      this.setID(pContainer1, pID1);
      GameObject gameObject2 = Object.Instantiate<GameObject>(this.slotButtonPrefab, this.buttonsContainer.transform);
      gameObject2.transform.localPosition = new Vector3(0.0f, (float) (-num3 * num2));
      GameObject pContainer2 = gameObject2;
      int pID2 = num6;
      int num7 = pID2 + 1;
      this.setID(pContainer2, pID2);
      GameObject gameObject3 = Object.Instantiate<GameObject>(this.slotButtonPrefab, this.buttonsContainer.transform);
      gameObject3.transform.localPosition = new Vector3((float) num1, (float) (-num3 * num2));
      GameObject pContainer3 = gameObject3;
      int pID3 = num7;
      num4 = pID3 + 1;
      this.setID(pContainer3, pID3);
      ++num3;
    }
    this.content.sizeDelta = new Vector2(0.0f, (float) (num5 * num2));
  }

  private void OnEnable()
  {
    this.loaded = false;
    this.Init();
  }

  private void Update()
  {
    foreach (LevelPreviewButton preview in this.previews)
    {
      if (!preview.loaded && !preview.loading)
      {
        preview.reloadImage();
        break;
      }
    }
  }

  private void OnDisable()
  {
    for (int index = 0; index < this.containers.Count; ++index)
    {
      this.previews[index].checkTextureDestroy();
      Object.Destroy((Object) this.containers[index]);
      this.containers[index] = (GameObject) null;
    }
    this.previews.Clear();
    this.containers.Clear();
  }

  private void setID(GameObject pContainer, int pID)
  {
    Transform transform = pContainer.transform.Find("AnimationContainer/Mask/SizeContainer/Button");
    ((Component) transform).GetComponent<SlotButtonCallback>().slotID = pID;
    ((Component) transform).GetComponent<LevelPreviewButton>().loaded = false;
    ((Component) transform).GetComponent<LevelPreviewButton>().worldNetUpload = this.worldNetUpload;
    if (pID > 1)
      ((Component) transform).GetComponent<LevelPreviewButton>().premiumOnly = true;
    this.previews.Add(((Component) transform).GetComponent<LevelPreviewButton>());
    this.containers.Add(pContainer);
  }
}
