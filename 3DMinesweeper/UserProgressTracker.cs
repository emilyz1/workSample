// Steven Huang

// User Progress Tracker

public class Progress {
    public int gamesPlayed;
    public int gamesWon;
    public int gamesLost;
    public int flagsPlaced;
    public int correctFlags;
    public int cellsRevealed;
}

private Progress progress;

private void Awake() {
    progress = new Progress();
}

private void Flag() {
    // ...

    progress.flagsPlaced++;

    if (cell.flagged && cell.type == Cell.Type.Mine) {
        progress.correctFlags++;
    }

    // ...
}

private void Reveal() {
    // ...

    if (cell.type == Cell.Type.Mine) {
        progress.gamesLost++;
    } else if (progress.cellsRevealed == width * height - mineCount) {
        progress.gamesWon++;
    }

    progress.cellsRevealed++;

    // ...
}

// Unity Text Display

public UnityEngine.UI.Text gamesPlayedText;
public UnityEngine.UI.Text gamesWonText;
public UnityEngine.UI.Text gamesLostText;
public UnityEngine.UI.Text flagsPlacedText;
public UnityEngine.UI.Text correctFlagsText;
public UnityEngine.UI.Text cellsRevealedText;

private void UpdateUI() {
    gamesPlayedText.text = "Games Played: " + progress.gamesPlayed;
    gamesWonText.text = "Games Won: " + progress.gamesWon;
    gamesLostText.text = "Games Lost: " + progress.gamesLost;
    flagsPlacedText.text = "Flags Placed: " + progress.flagsPlaced;
    correctFlagsText.text = "Correct Flags: " + progress.correctFlags;
    cellsRevealedText.text = "Cells Revealed: " + progress.cellsRevealed;
}
