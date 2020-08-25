using UnityEngine;

public class AnimatedHauntable : MonoBehaviour, HauntableProvider {

  public Transform indicatorAnchor;
  public string animName;
  public Animator animator;

  public HauntableItem GetHauntable(){
      var item = new HauntableItem(){
        indcAnchor = indicatorAnchor,
        root = gameObject.transform,
      };
      item.onHaunt += HandleHaunt;
      return item;
  }

  private void HandleHaunt(){
    animator.Play(animName);
  }

    
}