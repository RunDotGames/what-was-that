using UnityEngine;
using System.Collections;

public class AnimatedHauntable : MonoBehaviour, HauntableProvider {

  public HauntType hauntType;
  public float fear;
  public string animName = "";
  private Animator animator;
  private AudioSource source;
  public float audioCut = 0;

  public void Start(){
    source = GetComponentInChildren<AudioSource>();
    source.playOnAwake = false;
    animator = GetComponentInChildren<Animator>();
  }
  
  
  public HauntableItem GetHauntable(){
      var item = new HauntableItem(){
        root = gameObject.transform,
        hauntType=hauntType,
        fear=fear,
      };
      item.onHaunt += HandleHaunt;
      return item;
  }

  private void HandleHaunt(){
    if(animName.Length > 0){
      animator?.Play(animName);
    }
    source?.Play();
    if(audioCut > 0){
      StartCoroutine(CutAudio());
    }
  }

  private IEnumerator CutAudio(){
    var duration = 0.0f;
    while (duration < audioCut) {
      duration = duration + Time.deltaTime;
      float percent = duration / audioCut;
      source.volume = Mathf.Lerp(1, 0, percent);
      yield return new WaitForEndOfFrame();
    }
    source?.Stop();
  }

  

    
}