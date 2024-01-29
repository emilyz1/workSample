using UnityEngine;

// creating custom cell data structure
public struct Cell {
    // various types of cells
    public enum Type {
        Invalid,
        Empty, 
        Mine,
        Number,
    }
    
    // initialize basic states
    public Vector3Int position;
    public Type type;
    public int number;
    public bool revealed;
    public bool flagged;
    public bool exploded;
}
