using UnityEngine;
using System.Collections;

public class ExplosiveHauntable : MonoBehaviour, HauntableProvider
{
  public HauntType hauntType;
  public float fear;
  public Transform explodePoint;
  public float explodeForce;
  public float explodeRadius;
  public float audioCut = 0;
  private AudioSource source;

  public void Start(){
    source = GetComponentInChildren<AudioSource>();
  }
  public HauntableItem GetHauntable() {
    var item = new HauntableItem(){root=transform, hauntType=hauntType, fear=fear};
    item.onHaunt += OnHaunt;
    return item;
  }

  private void OnHaunt(){
    source?.Play();
    if(audioCut > 0){
      StartCoroutine(CutAudio());
    }
    var hits = Physics.OverlapSphere(explodePoint.position, explodeRadius);
    foreach (var hit in hits){
      if(!hit.attachedRigidbody){
        continue;
      }
      var direction = (hit.transform.position - explodePoint.transform.position).normalized;
      hit.attachedRigidbody.AddForce(direction * explodeForce, ForceMode.Impulse);
      hit.attachedRigidbody.AddTorque(direction * explodeForce, ForceMode.Impulse);
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