using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour{
    public enum Type{
        Empty,
        Mine,
        Number,
    }

    public Type type;
    public Vector3 pos;
    public int number = 0;
    public bool revealed = false;
    public bool flagged;

    public int x;
    public int y;
    public int z;
}
