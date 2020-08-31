using System;
using System.Collections.Generic;
using UnityEngine;


public enum FearReaction {
  Approach, Flee, Ignore
}


[Serializable]
public class FearLevel {
  public FearReaction reaction;
  public float nextRoomFactor;
  public float sameRoomFactor;
}

public class FearActor {
  
  public event Action<FearActor> OnPanic;
  public event Action<FearActor> OnScared;
  public event Action<FearActor> OnCuriouse;
  
  public Transform root;
  public HouseController house;
  public float maxFear;
  
  private Dictionary<FearReaction, Func<Vector3, Vector3>> responseActions = new Dictionary<FearReaction, Func<Vector3, Vector3>>();
  private Dictionary<FearReaction, FearLevel> levelMap = new Dictionary<FearReaction, FearLevel>();
  private bool isPaniced;
  private float currentFear;

  public void Init(FearLevel[] levels){
    foreach (var level in levels){
        levelMap[level.reaction] = level;
    }
    responseActions[FearReaction.Approach] = Approach;
    responseActions[FearReaction.Flee] = Flee;
    responseActions[FearReaction.Ignore] = Ignore;
  }

  public Vector3 HandleFear(float fear, FearReaction response, Vector3 from){
    if(isPaniced){
      return root.position;
    }

    var fromPosition = house.TranslatePosition(from);
    var myPosition = house.TranslatePosition(root.position);
    var isInSameRoom = myPosition == fromPosition;

    var level = levelMap[response];
    var factor = isInSameRoom ? level.sameRoomFactor : level.nextRoomFactor;
    currentFear = Mathf.Min(maxFear, currentFear + factor * fear);
    Debug.Log(currentFear + " " + maxFear);
    if(currentFear >= maxFear){
      Debug.Log("panic!");
      isPaniced = true;
      OnPanic?.Invoke(this);
      return house.GetEntrance();
    }

    return responseActions[response](from);
      
  }

  private Vector3 Flee(Vector3 from) {
    OnScared?.Invoke(this);
    var actorPosition = house.TranslatePosition(root.position);
    var hauntPosition = house.TranslatePosition(from);
    var actorConnected = house.GetConnectedPositions(actorPosition);
    actorConnected.Remove(hauntPosition);
    if(actorConnected.Count == 0){
      //yikes, nowhere to run to
      return root.position;
    }
    var targetPosition = actorConnected[UnityEngine.Random.Range(0, actorConnected.Count)];
    return house.TranslateInversePosition(targetPosition);
  }

  private Vector3 Approach(Vector3 from) {
    OnCuriouse?.Invoke(this);
    return from;
  }

  private Vector3 Ignore(Vector3 from) {
    return root.position;
  }

  public float GetCurrentFear(){
    return currentFear;
  }
}