using UnityEngine;

public class Game : MonoBehaviour {
    // board dimensions
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;

    private Board board;
    private Cell[,] state;
    private bool gameover;

    private void OnValidate() {
        mineCount = Mathf.Clamp(mineCount, 0, width * height);
    }

    private void Awake() {
        Application.targetFrameRate = 60;
        //game script and board script not assigned to the same component
        board = GetComponentInChildren<Board>();
    }

    private void Start() {
        NewGame();
    }

    // Create new game
    private void NewGame() {
        state = new Cell[width, height];
        gameover = false;

        GenerateCells();
        GenerateMines();
        GenerateNumbers();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        board.Draw(state);
    }

    private void GenerateCells() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                // create new cell, add to state
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines() {
        // loop over each mine
        for (int i = 0; i < mineCount; i++) {
            // generate random coordinates for each mine
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            // generate different coordinates if initially assigned coordinates are already taken
            while (state[x, y].type == Cell.Type.Mine) {
                x++;
                
                if (x >= width) {
                    x = 0;
                    y++;

                    if (y >= height) {
                        y = 0;
                    }
                }
            }

            state[x, y].type = Cell.Type.Mine;
        }
    }

    // generate number of adjacent mines onto each cell
    private void GenerateNumbers() {
        // traverse each cell of board
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Cell cell = state[x, y];

                // skip if mine
                if (cell.type == Cell.Type.Mine) {
                    continue;
                }
                
                cell.number = CountMines(x, y);

                // check if cell is not blank
                if (cell.number > 0) {
                    cell.type = Cell.Type.Number;
                }

                state[x, y] = cell;
            }
        }
    }

    // get number of mines adjacent to inputted cell
    private int CountMines(int cellX, int cellY) {
        int count = 0;

        // checking each adjacent cell, i and j represent the index offset from the cell inputted
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++) {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++) {
                // check if we are looking at inputted cell
                if (adjacentX == 0 && adjacentY == 0) {
                    continue;
                }
                // get coordinates for adjacent cell
                int x = cellX + adjacentX;
                int y = cellY + adjacentY;
                
                // check if adjacent cell is mine, increase count
                if (GetCell(x, y).type == Cell.Type.Mine) {
                    count++;
                }
            }
        }
        return count;
    }

    private void Update() {
        // if user right clicks/attempts to flag cell
        if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
        }
        else if (!gameover) {
            if (Input.GetMouseButtonDown(1)) {
                Flag();
            }
            else if (Input.GetMouseButtonDown(0)) {
                Reveal();
            }
        }
    }

    private void Flag() {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        // edge case
        if (cell.type == Cell.Type.Invalid || cell.revealed) {
            return;
        }

        // set flagged state to opposite of current
        cell.flagged = !cell.flagged;
        // reassign back to state array
        state[cellPosition.x, cellPosition.y] = cell;
        // redraw board
        board.Draw(state);
    }

    private void Reveal() {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        // check if cell cannot be revealed
        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged) {
            return;
        }

        switch (cell.type) {
            case Cell.Type.Mine:
                Explode(cell);
                break;
            
            case Cell.Type.Empty:
                Flood(cell);
                CheckWinCondition();
                break;
            
            default:
                cell.revealed = true;
                state[cellPosition.x, cellPosition.y] = cell;
                CheckWinCondition();
                break;
        }

        // redraw board
        board.Draw(state);
    }

    // flood open cells if they are empty
    private void Flood(Cell cell) {
        // base cases
        if (cell.revealed) {
            return;
        }
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) {
            return;
        }

        // Reveal cell
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        // if cell is empty, check adjacent cells to see if empty - keep flooding if so
        if (cell.type == Cell.Type.Empty) {
            Flood(GetCell(cell.position.x - 1, cell.position.y));
            Flood(GetCell(cell.position.x + 1, cell.position.y));
            Flood(GetCell(cell.position.x, cell.position.y - 1));
            Flood(GetCell(cell.position.x, cell.position.y + 1));
        }
    }

    private void Explode(Cell cell) {
        Debug.Log("Game Over!");
        gameover = true;

        // Set cell state as exploded
        cell.exploded = true;
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        // Reveal all mines
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                cell = state[x, y];

                if (cell.type == Cell.Type.Mine) {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    private void CheckWinCondition() {
        // traverse each cell within state array
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Cell cell = state[x, y];

                // check if all non-mine cells are not revealed - no win
                if (cell.type != Cell.Type.Mine && !cell.revealed) {
                    return;
                }
            }
        }

        Debug.Log("Winner!");
        gameover = true;
        
        // flag all mines
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Cell cell = state[x, y];
                
                if (cell.type == Cell.Type.Mine) {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    // Get cell at given coordinates
    private Cell GetCell(int x, int y) {
        // check if xy coordinates are valid
        if (x >= 0 && x < width && y >= 0 && y > height) {
            return state[x, y];
        } 
        // return an invalid cell
        else {
            return new Cell();
        }
    }
}
