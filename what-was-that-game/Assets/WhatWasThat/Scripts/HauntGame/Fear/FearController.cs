using UnityEngine;

public class FearController : MonoBehaviour {
  public FearLevel[] factors;

  public void Init(){

  }

  public void AddActor(FearActor actor){
    actor.Init(factors);
  }
}