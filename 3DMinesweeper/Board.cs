using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour {
    public Tilemap tilemap {
        // creating getter and setter
        get; private set;
    }

    public Tile TileUnknown;
    public Tile TileEmpty;
    public Tile TileMine;
    public Tile TileExploded;
    public Tile TileFlag;
    public Tile Tile1;
    public Tile Tile2;
    public Tile Tile3;
    public Tile Tile4;
    public Tile Tile5;
    public Tile Tile6;
    public Tile Tile7;
    public Tile Tile8;

    // initializes board
    private void Awake() {
        tilemap = GetComponent<Tilemap>();
    }

    // draw gameboard
    public void Draw(Cell[,] state) {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        // traverse board cell by cell
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Cell cell = state[i, j];
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
    }

    // get corresponding Tile for cell state
    private Tile GetTile(Cell cell) {
        if (cell.revealed) {
            return GetRevealedTileType(cell);
        }
        else if (cell.flagged) {
            return TileFlag;
        }
        else {
            return TileUnknown;
        }
    } 
    // get corresponding Tile for revealed cell
    private Tile GetRevealedTileType(Cell cell) {
        switch(cell.type) {
            case Cell.Type.Empty: return TileEmpty;
            case Cell.Type.Mine: return TileMine;
            case Cell.Type.Number: return GetTileNumber(cell);
            default: return null;
        }
    }
    
    // get corresponding number Tile
    private Tile GetTileNumber(Cell cell) {
        switch (cell.number) {
            case 1: return Tile1;
            case 2: return Tile2;
            case 3: return Tile3;
            case 4: return Tile4;
            case 5: return Tile5;
            case 6: return Tile6;
            case 7: return Tile7;
            case 8: return Tile8;
            default: return null;
        }
    }
}
