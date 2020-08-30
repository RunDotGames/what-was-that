using UnityEngine;

public class MotorAnimator {


  private DirectionProvider directionProvider;
  private bool isMoving;
  private string walkAnimation;
  private string idleAnimation;
  private Animator animator;
  private bool isPaused;
  
  public MotorAnimator(DirectionProvider directionProvider, Animator anim, string walkAnimation, string idleAnimation) {
    this.directionProvider = directionProvider;
    this.animator = anim;
    this.walkAnimation = walkAnimation;
    this.idleAnimation = idleAnimation;
    anim.Play(idleAnimation);
  }

  public void SetIdleAnim(string idleAnimName){
    this.idleAnimation = idleAnimName;
    if(isPaused){
      return;
    }
    if(!isMoving){
      animator.Play(idleAnimName);
    }
  }

  public void SetWalkAnim(string walkAnim){
    this.walkAnimation = walkAnim;
    if(isPaused){
      return;
    }
    if(isMoving){
      animator.Play(walkAnim);
    }
  }

  public void Pause(){
    isPaused = true;
  }

  public void Resume(){
    isPaused = false;
    var direction = directionProvider.GetDirection();
    isMoving = direction.magnitude > 0;
    animator.Play(isMoving ? walkAnimation : idleAnimation);
  }

  public void Update(){
    if(isPaused){
      return;
    }
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