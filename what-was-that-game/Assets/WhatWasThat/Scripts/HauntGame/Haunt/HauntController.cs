using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

[Serializable]
public class HauntIconConfig {
  public HauntType hauntType;
  public Sprite image;
  
}

public class HauntController: MonoBehaviour {

  public float interactionDistance;
  public HauntIndicator indcPrefab;
  public GameObject possibleIndcPrefab;
  public float indcHeight = 4;
  public List<HauntIconConfig> iconConfigs;

  private HauntableItem nearestItem;
  private Haunter haunter;
  private List<HauntableItem> items = new List<HauntableItem>();
  private List<HauntResponder> responders = new List<HauntResponder>();
  private HauntIndicator indc;
  private PositionTranslator translator;
  private HauntType indcType = HauntType.Unknown;
  private Dictionary<HauntType, HauntIconConfig> iconMap = new Dictionary<HauntType, HauntIconConfig>();
  private ActionLockController actionLockController;

  public void Init(ActionLockController actionLockController){
    this.actionLockController = actionLockController;
    foreach (var config in iconConfigs){
        iconMap[config.hauntType] = config;
    }

    indc = GameObject.Instantiate(indcPrefab, Vector3.zero, Quaternion.identity, transform.parent);
    indc.gameObject.SetActive(false);
    actionLockController.OnLock += HandleLock;
  }

  public void SetPositionTranslator(PositionTranslator translator){
    this.translator = translator;
  }

  public void AddHauntable(HauntableItem item) {
    items.Add(item);
  }

  public void AddHaunter(Haunter haunter){
    this.haunter = haunter;
    haunter.onHaunt += HandleHaunt;
  }

  public void AddResponder(HauntResponder responder){
    this.responders.Add(responder);
  }

  private void HandleHaunt(Haunter haunter){
    if(nearestItem == null){
      return;
    }
    nearestItem.HandleHaunt();
    var hauntPosition = translator.TranslatePosition(nearestItem.root.position);
    var hauntNeighboors = translator.GetConnectedPositions(hauntPosition);
    var hauntEvent = new HauntEvent(){hauntType=nearestItem.hauntType, position = nearestItem.root.position, fear=nearestItem.fear};
    foreach (var responder in responders) {
        var responderPosition = translator.TranslatePosition(responder.root.position);
        var isInSameRoom = hauntPosition == responderPosition;
        if(!hauntNeighboors.Contains(responderPosition) && !isInSameRoom) {
          continue;
        }
        responder.Respond(hauntEvent);
    }
  }

  private HauntableItem GetNearestItem(){
    if(haunter == null){
      return null;
    }
    Debug.DrawRay(haunter.root.position, Vector3.right * interactionDistance, Color.magenta);
    var nearestDistance = float.MaxValue;
    HauntableItem nearest = null;
    foreach (var item in items){
      if(item.isExausted){
        continue;
      }

      var distance = (haunter.root.position - item.root.position).magnitude;
      if(distance > interactionDistance){
        continue;
      }
      
      if (distance < nearestDistance) {
        nearest = item;
        nearestDistance = distance;
      }
    }
    return nearest;
  }

  private void HandleLock(){
    indc?.gameObject.SetActive(false);
    nearestItem = null;
  }

  public void Update(){
    if(actionLockController.IsLocked()){
      return;
    }
    if(indc == null){
      return;
    }

    nearestItem = GetNearestItem();
    if(nearestItem == null){
      indc.gameObject.SetActive(false);
      return;
    }

    indc.gameObject.SetActive(true);
    if(indcType != nearestItem.hauntType){
      indcType = nearestItem.hauntType;
      indc.image.sprite = iconMap[indcType].image;
    }
    indc.transform.position = nearestItem.root.position + Vector3.up * indcHeight;
  }

  public Sprite GetIcon(HauntType type){
    return iconMap[type].image;
  }
}