// Steven Huang

// Difficulty Level

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public enum DifficultyLevel {
    Beginner,
    Intermediate,
    Expert
}

public class Game : MonoBehaviour {
    public DifficultyLevel difficultyLevel = DifficultyLevel.Beginner;

    private Board board;
    private Cell[,] state;
    private bool gameover;

    private readonly Dictionary<DifficultyLevel, (int width, int height)> boardSizes = new Dictionary<DifficultyLevel, (int width, int height)> {
        { DifficultyLevel.Beginner, (9, 9) },
        { DifficultyLevel.Intermediate, (16, 16) },
        { DifficultyLevel.Expert, (30, 16) }
    };

    private readonly Dictionary<DifficultyLevel, int> mineCounts = new Dictionary<DifficultyLevel, int> {
        { DifficultyLevel.Beginner, 10 },
        { DifficultyLevel.Intermediate, 40 },
        { DifficultyLevel.Expert, 99 }
    };

    public Tilemap tilemap;
    // ... Rest of the Tile variables

    private void Awake() {
        Application.targetFrameRate = 60;
        board = GetComponentInChildren<Board>();
    }

    private void Start() {
        NewGame();
    }

    private void NewGame() {
        (int width, int height) = boardSizes[difficultyLevel];
        int mineCount = mineCounts[difficultyLevel];

        state = new Cell[width, height];
        gameover = false;

        GenerateCells();
        GenerateMines(mineCount);
        GenerateNumbers();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        board.Draw(state);
    }

    // ... Rest of the methods

    private void GenerateMines(int mineCount) {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        // ...

        state[x, y].type = Cell.Type.Mine;
    }

    // ... Rest of the methods

    // Add UI functionality to change the difficulty level
    public void SetDifficultyLevel(Dropdown dropdown) {
        DifficultyLevel selectedLevel = (DifficultyLevel)dropdown.value;
        difficultyLevel = selectedLevel;
    }
}
