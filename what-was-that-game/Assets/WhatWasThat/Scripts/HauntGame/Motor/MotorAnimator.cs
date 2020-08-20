using UnityEngine;

public class MotorAnimator {


  private DirectionProvider directionProvider;
  private bool isMoving;
  private string walkAnimation;
  private string idleAnimation;
  private Animator animator;
  
  public MotorAnimator(DirectionProvider directionProvider, Animator anim, string walkAnimation, string idleAnimation) {
    this.directionProvider = directionProvider;
    this.animator = anim;
    this.walkAnimation = walkAnimation;
    this.idleAnimation = idleAnimation;
    anim.Play(idleAnimation);
  }

  public void Update(){
  var direction = directionProvider.GetDirection();
    var wasMoving = isMoving;
    isMoving = direction.magnitude > 0;
    if(isMoving && !wasMoving){
      animator.Play(walkAnimation);
    }
    if(!isMoving && wasMoving){
      animator.Play(idleAnimation);
    }
  }
}