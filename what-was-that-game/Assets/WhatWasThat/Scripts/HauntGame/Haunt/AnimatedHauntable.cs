using UnityEngine;

public class AnimatedHauntable : MonoBehaviour, HauntableProvider {

  public HauntType hauntType;
  public float fear;
  public string animName;
  public Animator animator;
  
  
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
    animator.Play(animName);
  }

    
}