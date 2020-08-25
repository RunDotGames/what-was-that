using UnityEngine;
using System.Collections.Generic;


public class HauntController: MonoBehaviour {

  public float interactionDistance;
  public GameObject indcPrefab;
  public GameObject possibleIndcPrefab;

  private HauntableItem nearestItem;
  private Haunter haunter;
  private List<HauntableItem> items = new List<HauntableItem>();
  private GameObject indc;

  public void Init(){
    indc = GameObject.Instantiate(indcPrefab, Vector3.zero, Quaternion.identity, transform.parent);
    indc.SetActive(false);
  }

  public void AddHauntable(HauntableItem item) {
    items.Add(item);
  }

  public void AddHaunter(Haunter haunter){
    this.haunter = haunter;
    haunter.onHaunt += HandleHaunt;
  }

  private void HandleHaunt(Haunter haunter){
    if(nearestItem == null){
      return;
    }

    nearestItem.HandleHaunt();
  }

  private HauntableItem GetNearestItem(){
    if(haunter == null){
      return null;
    }
    Debug.DrawRay(haunter.root.position, Vector3.forward * interactionDistance, Color.magenta);
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

  public void Update(){
    nearestItem = GetNearestItem();
    if(nearestItem == null){
      indc.SetActive(false);
      return;
    }

    indc.SetActive(true);
    indc.transform.position = nearestItem.indcAnchor.position;
  }
}