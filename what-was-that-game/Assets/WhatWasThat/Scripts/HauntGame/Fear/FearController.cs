using UnityEngine;
using System.Collections.Generic;

public class FearController : MonoBehaviour {
  public FearLevel[] factors;

  private List<FearActor> actors = new List<FearActor>();

  public void Init(){

  }

  public void AddActor(FearActor actor){
    actor.Init(factors);
    actors.Add(actor);
  }

  public float GetMaxFear(){
    var fearTotal = 0.0f;
    foreach (var actor in actors){
      fearTotal += actor.maxFear;
    }
    return fearTotal;
  }

  public float GetCurrentFear(){
    var fearTotal = 0.0f;
    foreach (var actor in actors){
      fearTotal += actor.GetCurrentFear();
    }
    return fearTotal;
  }
}