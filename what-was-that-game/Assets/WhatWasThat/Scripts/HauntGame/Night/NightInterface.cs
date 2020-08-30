using UnityEngine;
using System;


public class NightInterface : MonoBehaviour{
  public event Action OnEndOfNight;
  public GameObject escapeFail;
  public GameObject highFearEnd;
  public GameObject midFearEnd;
  public GameObject lowFearEnd;
  public RectTransform progressionTick;
  public RectTransform fearTick;
  public GameObject restart;

  public GameObject welcome;
  private float fearTarget;
  public float maxTicks = 8;
  public float tickSpeed = 3;
  private int tickCount = 0;
  private float targetRotation;
  private float lastRotation;
  private Guid actionLock = Guid.Empty;
  private Dial fearDial;
  private Dial tickDial;
  private FearController fearController;
  private float maxFear;
  private ActionLockController actionLockController;

  public void Init(FearController fearController, float fearTarget, ActionLockController actionLock){
    this.actionLockController = actionLock;
    this.fearTarget = fearTarget;
    this.fearController = fearController;
    progressionTick.rotation = Quaternion.Euler(0, 0, 0);
    targetRotation = 0;
    lastRotation = 0;
    welcome.SetActive(true);
    escapeFail.SetActive(false);
    lowFearEnd.SetActive(false);
    midFearEnd.SetActive(false);
    highFearEnd.SetActive(false);
    restart.SetActive(false);
    tickDial = new Dial(progressionTick, tickSpeed, -180, actionLockController);
    fearDial = new Dial(fearTick, tickSpeed, 180, actionLockController);
  }

  private void HandleActionLock(){
    tickCount ++;
    if(tickCount > maxTicks){
      return;
    }
    float tickPercent = (float)tickCount/(float)maxTicks;
    tickDial.MoveTo(tickPercent);
    if(tickCount == maxTicks){
      actionLockController.OnUnlock += HandleTickEnd;
    }
  }

  private void HandleTickEnd(){
    actionLockController.OnUnlock -= HandleTickEnd;
    OnEndOfNight?.Invoke();
  }

  public void Update(){
    tickDial.Update();
    UpdateFearDial();
  }

  private void UpdateFearDial(){
    fearDial.Update();
    if(maxFear <= 0){
      return;
    }
    
    float fearPercent = Mathf.Min(fearController.GetCurrentFear()/ (maxFear*fearTarget), 1.0f);
    if(fearPercent == fearDial.GetPercent()){
      return;
    }
    
    fearDial.MoveTo(fearPercent);
  }

  public void HideWelcome(){
    welcome.SetActive(false);
  }

  public void StartTracking(){
    actionLockController.OnLock += HandleActionLock;
    maxFear = fearController.GetMaxFear();
  }

  public void ShowEscapeFail(){
    restart.SetActive(true);
    escapeFail.SetActive(true);
  }

  public void ShowLowFearEnd() {
    restart.SetActive(true);
    lowFearEnd.SetActive(true);
  }

  public void ShowMidFearEnd() {
    restart.SetActive(true);
    midFearEnd.SetActive(true);
  }

  public void ShowHighFearEnd() {
    restart.SetActive(true);
    highFearEnd.SetActive(true);
  }
}