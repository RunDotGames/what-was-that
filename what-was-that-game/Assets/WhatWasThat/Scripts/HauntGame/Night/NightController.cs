using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NightController : MonoBehaviour {

  public NightInterface uiPrefab;
  public int investigatorCount;
  public int reactionCount;
  public int fleeCount;
  public InvestigatorController investigatorPrefab;
  public GameObject[] investigatorModels;
  public PlayerController playerPrefab;
  
  public float offsetDistance;
  public float fearTarget = 1.0f;
  private ActionLockController actionLock;
  private BarrierController barrierController;
  private FearController fearController;
  private KeyBindingsController keyBindings;
  private CameraController cameraController;
  private KinimaticMotorController motorController;
  private HouseController houseController;
  private HauntController hauntController;
  private NodePathController pathController;

  private bool isStarted;
  private bool isEnded;

  private List<InvestigatorController> investigators = new List<InvestigatorController>();
  private PlayerController player;
  private NightInterface ui;

  public void Start() {
    actionLock = new ActionLockController();
    barrierController = GetComponent<BarrierController>();
    fearController = GetComponent<FearController>();
    keyBindings = GetComponent<KeyBindingsController>();
    cameraController = GetComponent<CameraController>();
    motorController = GetComponent<KinimaticMotorController>();
    houseController = GetComponent<HouseController>();
    hauntController = GetComponent<HauntController>();
    pathController = GetComponent<NodePathController>();

    barrierController.Init(actionLock);
    fearController.Init();
    keyBindings.Init();
    cameraController.Init();
    motorController.Init();
    hauntController.Init(actionLock);
    houseController.Init(motorController, pathController, hauntController, barrierController);

    ui = GameObject.Instantiate(uiPrefab, transform);
    ui.OnEndOfNight += HandleEndOfNight;
    ui.Init(fearController, fearTarget, actionLock);

    houseController.Generate();

    var invShuffle = new ShuffleSet<GameObject>(investigatorModels);
    var roomShuffle = new ShuffleSet<Vector2Int>(houseController.GetNonStartingRooms());
    var rotationIncrement = 360.0f / investigatorCount;
    for(int i = 0; i < investigatorCount; i++){
      var rotation = i * rotationIncrement;
      var offset = Quaternion.Euler(0, rotation, 0) * (Vector3.right* offsetDistance);
      SpawnInvestigator(invShuffle, offset);
    }
    SpawnPlayer();
  }

  private void HandleEndOfNight(){
    isEnded = true;
    player.SetAllowInput(false);
    float fearPercent = fearController.GetCurrentFear() / (fearController.GetMaxFear()*fearTarget);
    if(fearPercent < 0.333f){
      ui.ShowLowFearEnd();
      return;
    }
    if(fearPercent < 0.666f){
      ui.ShowMidFearEnd();
      return;
    }

    ui.ShowHighFearEnd();

  }

  private void SpawnInvestigator(ShuffleSet<GameObject> invShuffle, Vector3 offset){
    var investigator = GameObject.Instantiate(investigatorPrefab, transform);
    var model = GameObject.Instantiate(invShuffle.Pop(), investigator.modelRoot);
    model.transform.localRotation = Quaternion.identity;
    model.transform.localScale = Vector3.one;
    model.transform.localPosition = Vector3.zero;

    var startingPoint = houseController.GetStartingPoint();
    investigator.transform.position = startingPoint.position + offset;

    investigator.reactions.Clear();
    var hauntShuffle = new ShuffleSet<HauntType>((HauntType[])Enum.GetValues(typeof(HauntType)));
    
    for( var index = 0; index < reactionCount; index++){
      if(hauntShuffle.IsEmpty()){
        break;
      }
      var reaction = new HauntReaction(){haunt=HauntType.Unknown};
      while(reaction.haunt == HauntType.Unknown || hauntShuffle.IsEmpty()){
        reaction.haunt = hauntShuffle.Pop();
      }
      if(reaction.haunt == HauntType.Unknown){
        break;
      }
      reaction.reaction = index < fleeCount ? FearReaction.Flee : FearReaction.Approach;
      investigator.reactions.Add(reaction);
      
    }

    while(!hauntShuffle.IsEmpty()){
      investigator.reactions.Add(new HauntReaction(){reaction=FearReaction.Ignore, haunt=hauntShuffle.Pop()});
    }
    investigator.OnEscape += HandleEscape;
    investigator.finalOffset = offset;
    investigator.Init(motorController, pathController, hauntController, houseController, fearController, barrierController, actionLock);
    investigators.Add(investigator);
  }

  private void SpawnPlayer(){
    player = GameObject.Instantiate(playerPrefab, transform);
    
    var startingPoint = houseController.GetStartingPoint();
    player.transform.position = startingPoint.position;
    player.Init(motorController, cameraController, hauntController, keyBindings, barrierController, actionLock);
    
  }

  private void StartTheGame(){
    player.SetAllowInput(true);
    actionLock.Lock();
    actionLock.OnUnlock += HandleInvestigatorsPlaced;
    ui.HideWelcome();
    var roomShuffle = new ShuffleSet<Vector2Int>(houseController.GetNonStartingRooms());
    foreach (var investigator in investigators) {
        var position = roomShuffle.Pop();
        investigator.GoTo(houseController.TranslateInversePosition(position));
    }
  }

  private void HandleInvestigatorsPlaced(){
    actionLock.OnUnlock -= HandleInvestigatorsPlaced;
    ui.StartTracking();
  }

  public void Update(){
    if(Input.GetKeyUp(KeyCode.Space)){
      if(isEnded){
        SceneManager.LoadScene(0);
        return;
      }
      if(!isStarted){
        isStarted = true;
        StartTheGame();
      }
      
    }
  }

  private void HandleEscape(){
    isEnded = true;
    player.SetAllowInput(false);
    ui.ShowEscapeFail();
  }


}