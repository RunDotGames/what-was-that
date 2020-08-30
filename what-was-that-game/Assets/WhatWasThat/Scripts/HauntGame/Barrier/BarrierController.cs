using UnityEngine;
using System;
using System.Collections.Generic;


public class BarrierController : MonoBehaviour {

  public float buildDistance;
  public float blockDistance;
  public GameObject indicatePrefab;
  public float indicateHeight;
  public GameObject blockPrefab;
  public float blockHeight;
  public int maxBlock;
  public int buildCount = 2;
  public float spawnTime = 3;
  public float blockOffset = 0.02f;
  public float indcTime = 0.02f;
  private BarrierBuilder builder;
  private List<BarrierPoint> points = new List<BarrierPoint>();
  private List<BarrierBlockable> blockables = new List<BarrierBlockable>();
  private GameObject indicator;
  private BarrierPoint nearestToBuilder;
  private Coroutine indcRoutine;
  private bool indcShow;
  private ActionLockController actionLockController;

  public void Init(ActionLockController actionLockController){
    this.actionLockController = actionLockController;
    actionLockController.OnLock += HandleLock;
    indicator = GameObject.Instantiate(indicatePrefab, Vector3.zero, Quaternion.identity, transform);
    indicator.SetActive(false);
    indicator.transform.localScale = Vector3.zero;
  }

  private BarrierPoint GetNearest(float maxDistance, Vector3 to){
    BarrierPoint nearest = null;
    float distance = float.MaxValue;
    foreach (var point in points) {
        var pointDistance = (point.anchor.position - to).magnitude;
        if(pointDistance <=maxDistance && pointDistance < distance){
          distance = pointDistance;
          nearest = point;
        }
    }
    return nearest;
  }

  private IEnumerator<YieldInstruction> IndcRoutine(Vector3 to, bool thenActive){
    float duration = 0;
    Vector3 from = indicator.transform.localScale;
    indicator.SetActive(true);
    while(duration < indcTime) {
      duration = duration + Time.deltaTime;
      indicator.transform.localScale = Vector3.Lerp(from, to, duration/indcTime);
      yield return new WaitForEndOfFrame();
    }
    indicator.SetActive(thenActive);
  }

  private void Unindicate(){
    if(indcShow){
      indcShow = false;
      if(indcRoutine != null) StopCoroutine(indcRoutine);
      indcRoutine = StartCoroutine(IndcRoutine(Vector3.zero, false));
    }
    if(builder.IsIndicating()){
      builder.HandleUnIndicate();
    }
  }

  private void Indicate(BarrierPoint point){
    if(!indcShow){
      indcShow = true;
      if(indcRoutine != null) StopCoroutine(indcRoutine);
      indcRoutine = StartCoroutine(IndcRoutine(Vector3.one, true));
      indicator.transform.position = point.anchor.position + (Vector3.up * indicateHeight);
    }
    if(!builder.IsIndicating()){
      builder.HandleIndicate();
    }
  }

  private void UpdateBuilder(){
    if(actionLockController.IsLocked()){
      return;
    }
    if(builder == null){
      return;
    }
    
    nearestToBuilder = GetNearest(buildDistance, builder.root.position);
    if(nearestToBuilder == null){
      Unindicate();
      return;
    }
    if(nearestToBuilder.GetBlockCount() >= nearestToBuilder.GetMaxBlock()){
      Unindicate();
      return;
    }
    
    Indicate(nearestToBuilder);
  }

  private void UpdateBlockables(){
    foreach (var blockable in blockables) {
      if(blockable.IsBlocked()){
        continue;
      }
      var nearest = GetNearest(blockDistance, blockable.root.position);
      if(nearest == null){
        continue;
      }
      if(nearest.GetBlockCount() > 0){
        nearest.AddBlockable(blockable);
      }
    }
  }

  public void Update(){
    UpdateBlockables();
    UpdateBuilder();
  } 

  private void HandleLock(){
    Unindicate();
  }

  private void HandleBuildRequest(){
    if(nearestToBuilder == null){
      return;
    }
    for(int i = 0; i < buildCount; i++ ){
      if(nearestToBuilder.GetBlockCount() == nearestToBuilder.GetMaxBlock()){
        break;
      }
      nearestToBuilder.AddBlock();
    }
  }

  public void AddPoint(BarrierPoint point){
    point.Init(actionLockController, this, blockPrefab, blockHeight, maxBlock, spawnTime, blockOffset);
    points.Add(point);
  }

  public void AddBlockable(BarrierBlockable blockable) {
    blockables.Add(blockable);
  }

  public void SetBuilder(BarrierBuilder builder) {
    this.builder = builder;
    builder.OnBuildRequest += HandleBuildRequest;
  }
}