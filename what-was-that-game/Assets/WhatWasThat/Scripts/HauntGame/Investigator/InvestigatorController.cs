using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class HauntReaction {
  public HauntType haunt;
  public FearReaction reaction;
}



public class InvestigatorController : MonoBehaviour {
  private static readonly Vector3 INVALID_DESTINATION = Vector3.one * float.MaxValue;  
  public float maxFear;
  public string walkStateName;
  public string runAnim;
  public string idleStateName;
  public string fearAnim;
  public string curiouseAnim;
  public string breakDownAnim;
  public string fearBreakDownAnim;
  public KinimaticMotorConfig motorConfig;
  public List<HauntReaction> reactions;
  public Animator animator;
  public InvestigatorUI ui;

  private KinimaticMotor motor;
  private PathDirectionController pather;
  private MotorAnimator motorAnimator;
  private HouseController house;
  private FearActor fearActor;
  private BarrierBlockable blockable;

  private Dictionary<HauntType, HauntReaction> reactionMap = new Dictionary<HauntType, HauntReaction>();
  
  private bool isMoving;

  private Guid traverseLock = Guid.Empty;
  private Action traverseEvent;
  private Vector3 traverseDestination = INVALID_DESTINATION;
  private string currentBlockAnim;

  public void Init(
    KinimaticMotorController motorController,
    NodePathController nodePath,
    HauntController hauntController,
    HouseController house,
    FearController fearController,
    BarrierController barrierController
  ){
    ActionLockController.OnLock += HandleActionLock;
    var body = GetComponent<Rigidbody>();
    pather = new PathDirectionController(transform, nodePath.GetRoute);
    motor = motorController.GetMotor(motorConfig, body, pather);
    motorAnimator = new MotorAnimator(pather, animator, walkStateName, idleStateName);
    var hauntResponder = new HauntResponder(){root=transform};
    hauntResponder.onRespond += HandleHaunt;
    hauntController?.AddResponder(hauntResponder);
    this.house = house;  
    foreach (var reaction in reactions) {
        reactionMap[reaction.haunt] = reaction;
    }
    
    fearActor = new FearActor(){house = house, root=transform, maxFear = maxFear};
    fearActor.OnScared += HandleScared;
    fearActor.OnPanic += HandlePanic;
    fearActor.OnCuriouse += HandleCuriouse;
    fearController?.AddActor(fearActor);

    blockable = new BarrierBlockable(){root=transform};
    blockable.OnBlock += HandleBlock;
    blockable.OnUnblock += HandleUnblock;
    barrierController?.AddBlockable(blockable);
    currentBlockAnim = breakDownAnim;

    ui.Init(reactions, fearActor, hauntController);
  }

  private void HandleActionLock(){
    if(blockable.IsBlocked()) {
      blockable.RequestBreakdown();
    }
  }

  private void HandleScared(FearActor actor){
    UpdateBreakdownAnim(true);
    currentBlockAnim = fearBreakDownAnim;
    motorAnimator.SetIdleAnim(fearAnim);
    motorAnimator.SetWalkAnim(runAnim);
  }

  private void HandlePanic(FearActor actor){
    UpdateBreakdownAnim(true);
    currentBlockAnim = fearBreakDownAnim;
    motorAnimator.SetIdleAnim(fearAnim);
    motorAnimator.SetWalkAnim(runAnim);
  }

  private void HandleCuriouse(FearActor actor){
    UpdateBreakdownAnim(false);
    motorAnimator.SetIdleAnim(curiouseAnim);
    motorAnimator.SetWalkAnim(walkStateName);
  }

  public void Update(){
    pather.Update();
    motorAnimator.Update();
    
  }

  public void FixedUpdate() {
    if(motor == null) {
      return;
    }
    motor.FixedUpdate();
  }

  public void GoTo(Vector3 to){
    traverseDestination = to;
    ResumeTraversal();
  }

  private void HandleBlock(BarrierBlockable blockable){
    CancelTraversal();
    motorAnimator.Pause();
    animator.Play(currentBlockAnim);
  }

  private void UpdateBreakdownAnim(bool isAfraid){
    currentBlockAnim = breakDownAnim;
    if(isAfraid){
      currentBlockAnim = fearBreakDownAnim;
    }
    if(blockable.IsBlocked()){
      animator.Play(currentBlockAnim);
    }
  }

  private void HandleUnblock(BarrierBlockable blockable){
    if(traverseDestination == INVALID_DESTINATION) {
      return;
    }
    motorAnimator.Resume();
    ResumeTraversal();
  }

  private void CancelTraversal(){
    if(traverseLock != Guid.Empty){
      ActionLockController.ReleaseLockAction(traverseLock);
      traverseLock = Guid.Empty;
      pather.OnArrival -= traverseEvent;
      pather.Cancel();
    }
  }

  private void ResumeTraversal(){
    if(blockable.IsBlocked()){
      return;
    }
    CancelTraversal();
    if(traverseDestination == INVALID_DESTINATION){
      return;
    }
    pather.Navigate(transform.position, traverseDestination);
    traverseLock = ActionLockController.AddLockAction();
    traverseEvent = () => {
      ActionLockController.ReleaseLockAction(traverseLock);
      pather.OnArrival-= traverseEvent;
    };
    pather.OnArrival += traverseEvent;
  }

  private void HandleHaunt(HauntEvent hauntEvent){
    var reaction = reactionMap[hauntEvent.hauntType];
    traverseDestination = fearActor.HandleFear(hauntEvent.fear, reaction.reaction, hauntEvent.position);
    if(traverseDestination == transform.position){
      return;
    }
    ResumeTraversal();
  }
}