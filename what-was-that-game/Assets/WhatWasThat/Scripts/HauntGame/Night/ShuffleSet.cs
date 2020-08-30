using System.Collections.Generic;

public class ShuffleSet<T>{
  
  private List<T> shuffle;
  private T[] original;

  public ShuffleSet(T[] original){
    this.original = original;
    shuffle = GetShuffled();
  }

  public bool IsEmpty(){
    return shuffle.Count == 0;
  }

  private List<T> GetShuffled(){
    var dup = new List<T>();
    foreach (var item in original) {
        dup.Add(item);
    }
    var shuffled = new List<T>();
    for(var i = 0; i < original.Length; i++){
      var removeIndex = UnityEngine.Random.Range(0, dup.Count);
      shuffled.Add(dup[removeIndex]);
      dup.RemoveAt(removeIndex);
    }
    return shuffled;
  }

  public T Pop(){
    if(shuffle == null || shuffle.Count == 0){
      shuffle = GetShuffled();
    }
    var item = shuffle[shuffle.Count - 1];
    shuffle.RemoveAt(shuffle.Count-1);
    return item;
  }
}