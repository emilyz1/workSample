// Steven Huang

// Timer

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
    public Text timerText;

    private Board board;
    private Cell[,] state;
    private bool gameover;
    private float elapsedTime;

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

    private void Update() {
        if (!gameover) {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void NewGame() {
        (int width, int height) = boardSizes[difficultyLevel];
        int mineCount = mineCounts[difficultyLevel];

        state = new Cell[width, height];
        gameover = false;
        elapsedTime = 0f;

        GenerateCells();
        GenerateMines(mineCount);
        GenerateNumbers();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        board.Draw(state);
    }

    // ... Rest of the methods

    // Update the text component to display the elapsed time
    private void UpdateTimerDisplay() {
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;
    }
}
