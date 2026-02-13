// Decompiled with JetBrains decompiler
// Type: RewardUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RewardUI : MonoBehaviour
{
  public Image powerSprite;
  public Text text;
  public Text text_description;
  public Text window_title;
  public Text free_power_unlocked;
  public List<PowerButton> rewardPowers;
  public RewardAnimation rewardAnimation;

  internal void setRewardInfo(List<PowerButton> pButtons)
  {
    this.rewardPowers = pButtons;
    this.nextReward();
  }

  internal bool hasRewards() => this.rewardPowers != null && this.rewardPowers.Count > 0;

  internal PowerButton popLowestReward()
  {
    int num1 = 10000;
    int index = 0;
    int num2 = 0;
    foreach (PowerButton rewardPower in this.rewardPowers)
    {
      if (rewardPower.godPower.rank < (PowerRank) num1)
      {
        index = num2;
        num1 = (int) rewardPower.godPower.rank;
      }
      ++num2;
    }
    PowerButton rewardPower1 = this.rewardPowers[index];
    this.rewardPowers.RemoveAt(index);
    return rewardPower1;
  }

  internal void nextReward()
  {
    if (!this.hasRewards())
      return;
    PowerButton powerButton = this.popLowestReward();
    this.powerSprite.sprite = powerButton.icon.sprite;
    ((Component) this.text).GetComponent<LocalizedText>().setKeyAndUpdate(powerButton.godPower.getLocaleID());
    ((Component) this.text_description).gameObject.SetActive(true);
    ((Component) this.text_description).GetComponent<LocalizedText>().setKeyAndUpdate(powerButton.godPower.getDescriptionID());
    if (powerButton.godPower.id == "clock")
    {
      ((Component) this.window_title).GetComponent<LocalizedText>().key = "free_hourglass_title";
      ((Component) this.free_power_unlocked).GetComponent<LocalizedText>().key = "free_hourglass_unlocked";
      this.rewardAnimation.quickReward = true;
    }
    else
    {
      ((Component) this.window_title).GetComponent<LocalizedText>().key = "free_power";
      ((Component) this.free_power_unlocked).GetComponent<LocalizedText>().key = "free_power_unlocked";
      this.rewardAnimation.quickReward = false;
    }
    PlayerConfig.instance.data.lastReward = powerButton.godPower.id;
    ((Component) this.window_title).GetComponent<LocalizedText>().updateText();
    ((Component) this.free_power_unlocked).GetComponent<LocalizedText>().updateText();
  }

  public void bottomButtonClick()
  {
    if (this.rewardAnimation.state == RewardAnimationState.Open)
    {
      if (this.hasRewards())
      {
        this.rewardAnimation.resetAnim();
        this.nextReward();
      }
      else
        ((Component) this).GetComponent<ButtonEvent>().hideRewardWindowAndHighlightPower();
    }
    else
    {
      if (this.rewardAnimation.state != RewardAnimationState.Idle)
        return;
      this.rewardAnimation.clickAnimation();
    }
  }

  internal void setRewardInfo(string pSpritePath, string pText)
  {
    this.powerSprite.sprite = SpriteTextureLoader.getSprite("ui/Icons/" + pSpritePath);
    ((Component) this.text).GetComponent<LocalizedText>().key = pText;
    ((Component) this.text).GetComponent<LocalizedText>().updateText();
    ((Component) this.text_description).gameObject.SetActive(false);
    ((Component) this.window_title).GetComponent<LocalizedText>().key = "free_saveslots_title";
    ((Component) this.window_title).GetComponent<LocalizedText>().updateText();
    ((Component) this.free_power_unlocked).GetComponent<LocalizedText>().key = "free_saveslots_unlocked";
    ((Component) this.free_power_unlocked).GetComponent<LocalizedText>().updateText();
    this.rewardAnimation.quickReward = true;
  }
}
