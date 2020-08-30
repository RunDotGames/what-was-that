using UnityEngine;
using System;
using System.Collections.Generic;

public class BarrierPoint {

  public Transform anchor;
  public bool isHorizontal;

  private int blockCount;
  private float blockIncrement;
  private int maxBlock;
  private GameObject blockPrefab;
  private float spawnTime;
  private MonoBehaviour parent;
  private float blockOffset;
  private ActionLockController actionLockController;
  private List<GameObject> blockers = new List<GameObject>();

  private List<BarrierBlockable> blockables = new List<BarrierBlockable>();

  public void AddBlockable(BarrierBlockable blocked){
    blocked.HandleBlocked();
    blocked.OnBreakdown += HandleBreakDown;
    blocked.OnCancel += HandleBlockableCancel;
    this.blockables.Add(blocked);
  }

  public void RemoveBlockable(BarrierBlockable blocked){
    this.blockables.Remove(blocked);
  }

  public int GetBlockCount(){
    return blockCount;
  }

  public int GetMaxBlock(){
    return maxBlock;
  }

  public void Init(ActionLockController actionLockController, MonoBehaviour parent, GameObject blockPrefab, float blockHeight, int maxBlock, float spawnTime, float blockOffset){
    this.blockPrefab = blockPrefab;
    blockCount = 0;
    this.maxBlock = maxBlock;
    blockIncrement = blockHeight / (maxBlock+1);
    this.parent = parent;
    this.spawnTime = spawnTime;
    this.blockOffset = blockOffset;
    this.actionLockController = actionLockController;
  }

  public void AddBlock(){
    blockCount++;
    var position = Vector3.up * (blockIncrement * blockCount) + Vector3.forward * blockOffset;
    var rotation = Quaternion.Euler(0, !isHorizontal ? 90 : 0, UnityEngine.Random.Range(-25, 25));
    var blocker = GameObject.Instantiate(blockPrefab, anchor);
    blocker.transform.localPosition = position;
    blocker.transform.rotation = rotation;
    blockers.Add(blocker);
    parent.StartCoroutine(SpawnIn(blocker));
  }

  private void HandleBreakDown(BarrierBlockable _){
    if(blockCount == 0){
      return;
    }
    blockCount --;
    var removed = blockers[blockers.Count-1];
    blockers.RemoveAt(blockers.Count -1);
    parent.StartCoroutine(SpawnOut(removed));
    if(blockCount > 0){
      return;
    }
    foreach (var blockable in blockables){
      blockable.OnBreakdown -= HandleBreakDown;
      blockable.OnCancel -= HandleBlockableCancel;
      blockable.HandleReleased();
    }
    blockables.Clear();
  }

  private void HandleBlockableCancel(BarrierBlockable blockable){
    blockables.Remove(blockable);
  }

  private IEnumerator<YieldInstruction> SpawnOut(GameObject blocker){
    var aLock = actionLockController.AddLockAction();
    var duration = 0.0f;
    while(duration < spawnTime){
      duration = duration + Time.deltaTime;
      var scale = new Vector3(Mathf.Lerp(1, 0, duration/spawnTime), 1, 1);
      blocker.transform.localScale = scale;
      yield return new WaitForEndOfFrame();
    }
    GameObject.Destroy(blocker);
    actionLockController.ReleaseLockAction(aLock);
  }

  private IEnumerator<YieldInstruction> SpawnIn(GameObject blocker){
    var aLock = actionLockController.AddLockAction();
    var duration = 0.0f;
    while(duration < spawnTime){
      duration = duration + Time.deltaTime;
      var scale = new Vector3(Mathf.Lerp(0, 1, duration/spawnTime), 1, 1);
      blocker.transform.localScale = scale;
      yield return new WaitForEndOfFrame();
    }
    actionLockController.ReleaseLockAction(aLock);
    blocker.transform.localScale = Vector3.one;
  }

}