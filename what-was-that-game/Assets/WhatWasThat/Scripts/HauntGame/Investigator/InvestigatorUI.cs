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
  
  public void Init(List<HauntReaction> reactions, FearActor actor, HauntController hauntController){
    this.actor = actor;
    
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
        icon.rectTransform.pivot = Vector2.one * .05f;
        icon.sprite = hauntController.GetIcon(reaction.haunt);
        icon.color = reactMap[reaction.reaction].color;
        
    }
  }

  
  public void Update(){
    if(actor == null){
      return;
    }

    float percent = actor.GetCurrentFear() / actor.maxFear;
    Debug.Log(actor.GetCurrentFear());
    var percentClamped = Math.Max(percent, minFearShow);
    image.rectTransform.anchorMax = new Vector2(percentClamped, 1.0f);
    image.color = Color.Lerp(fillStartColor, fillEndColor, percentClamped);
  }
}