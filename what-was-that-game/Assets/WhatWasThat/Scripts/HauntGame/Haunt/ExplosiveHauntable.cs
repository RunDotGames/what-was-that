using UnityEngine;

public class ExplosiveHauntable : MonoBehaviour, HauntableProvider
{
  public Transform indcAnchor;
  public Transform explodePoint;
  public float explodeForce;
  public float explodeRadius;

  public HauntableItem GetHauntable() {
    var item = new HauntableItem(){indcAnchor= indcAnchor, root=transform};
    item.onHaunt += OnHaunt;
    return item;
  }

  private void OnHaunt(){
    var hits = Physics.OverlapSphere(explodePoint.position, explodeRadius);
    foreach (var hit in hits){
      if(!hit.attachedRigidbody){
        continue;
      }
      var direction = (hit.transform.position - explodePoint.transform.position).normalized;
      hit.attachedRigidbody.AddForce(direction * explodeForce, ForceMode.Impulse);
    }
  }
}