using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public class IconReactionConfig {
  public FearReaction reaction;
  public Color color;
}

public class InvestigatorUI : MonoBehaviour{

  public Image image;
  public float minFearShow = 0.15f;
  public Color fillStartColor;
  public Color fillEndColor;
  public Image iconPrefab;
  public RectTransform iconRow;
  public List<IconReactionConfig> reactionConfigs;
  private FearActor actor;
  private float currentPercent;
  public float fillSpeed;
  
  public void Init(List<HauntReaction> reactions, FearActor actor, HauntController hauntController){
    this.actor = actor;
    currentPercent = minFearShow;
    
    Dictionary<FearReaction, IconReactionConfig> reactMap = new Dictionary<FearReaction, IconReactionConfig>();
    foreach (var config in reactionConfigs){
        reactMap[config.reaction] = config;
    }

    List<HauntReaction> displayReactions = new List<HauntReaction>();
    foreach (var item in reactions){
        if(item.reaction == FearReaction.Ignore){
          continue;
        }
        displayReactions.Add(item);
    }
    
    if(displayReactions.Count == 0){
      return;
    }
    var reactionIncrement = 1.0f / (displayReactions.Count);
    for(var index = 0; index < displayReactions.Count; index++) {
        var reaction = displayReactions[index];
        var icon = GameObject.Instantiate(iconPrefab);
        icon.rectTransform.parent = iconRow;
        icon.rectTransform.localPosition = Vector3.zero;
        icon.rectTransform.localScale = Vector3.one;
        icon.rectTransform.anchorMin = new Vector2(index * reactionIncrement, 0);
        icon.rectTransform.anchorMax = new Vector2((index+1) * reactionIncrement, 1);
        icon.rectTransform.anchoredPosition = Vector2.zero;
        icon.rectTransform.sizeDelta = Vector2.zero;
        icon.rectTransform.localRotation = Quaternion.identity;
        icon.rectTransform.pivot = Vector2.one * .05f;
        icon.sprite = hauntController.GetIcon(reaction.haunt);
        icon.color = reactMap[reaction.reaction].color;
        
    }
  }

  
  public void Update(){
    if(actor == null){
      return;
    }

    float percent = 1.0f - (actor.GetCurrentFear() / actor.maxFear);
    currentPercent = Mathf.Lerp(currentPercent, Math.Min(percent, minFearShow), Time.deltaTime * fillSpeed);
    Debug.Log(currentPercent);
    image.rectTransform.anchorMin = new Vector2(currentPercent, 0);
    image.color = Color.Lerp(fillStartColor, fillEndColor, 1.0f - currentPercent);
  }
}