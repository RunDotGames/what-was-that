using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExplosiveHauntable))]
public class ExplosiveHauntableEditor : Editor {
    
    public void OnSceneGUI() {
        var explosive = (ExplosiveHauntable)target;
        if(explosive.explodePoint == null){
            return;
        }
        Handles.DrawWireDisc(explosive.explodePoint.position, Vector3.up, explosive.explodeRadius);
        

    }
}