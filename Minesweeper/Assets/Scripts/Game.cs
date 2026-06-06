using UnityEngine;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;

    private Board board;
    private Cell[,] state;
    private bool gameIsOver;
    private bool gameHasStarted;

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        state = new Cell[width, height];
        gameHasStarted = false;
        gameIsOver = false;

        GenerateCells();
        
        Camera.main.transform.position = 
            new Vector3(width / 2f, height / 2f, -10);
        
        board.Draw(state);
    }

    private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines(Cell firstCell)
    {
        if (mineCount > width * height)
        {
            throw new System.InvalidOperationException(
                "Can't have more mines than tiles."
            );
        }

        for (int i = 0; i < mineCount; i++)
        {
            Cell cell;
            
            do
            {
                cell = GetRandomCell();
            } while (cell.type == Cell.Type.Mine 
                  || IsExcluded(cell, firstCell));
            
            state[cell.position.x, cell.position.y].type
                = Cell.Type.Mine;
        }
    }

    private Cell GetRandomCell()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, height);
        return state[x, y];
    }

    private bool IsExcluded(Cell cell, Cell excluded)
    {
        // If dx or dy <= 1, then the cells neighbour each other.
        return Mathf.Abs(cell.position.x - excluded.position.x) <= 1
            && Mathf.Abs(cell.position.y - excluded.position.y) <= 1;
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                if (cell.type == Cell.Type.Mine) continue;
                cell.number = CountMines(cell.position);
                if (cell.number == 0) continue;
                cell.type = Cell.Type.Number;
                // cell.revealed = true;
                state[x, y] = cell;
            }
        }
    }

    private int CountMines(Vector3Int cellPos)
    {
        int count = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            int x = cellPos.x + dx;

            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int y = cellPos.y + dy;

                if (GetCell(x, y).type == Cell.Type.Mine)
                    count++;
            }
        }

        return count;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Flag();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (gameIsOver)
            {
                NewGame();
                return;
            }
            if (!gameHasStarted)
                StartGame(GetCellAtMousePos());
            Reveal();
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
            return;
        }
    }

    private void StartGame(Cell firstCell)
    {
        gameHasStarted = true;
        GenerateMines(firstCell);
        GenerateNumbers();
    }

    private void Flag()
    {
        Cell cell = GetCellAtMousePos();

        if (cell.type == Cell.Type.Invalid || cell.revealed)
            return;

        cell.flagged = !cell.flagged;
        state[cell.position.x, cell.position.y] = cell;
        board.Draw(state);
    }

    private void Reveal()
    {
        Cell cell = GetCellAtMousePos();

        if (cell.revealed || cell.flagged)
            return;

        switch (cell.type)
        {
            case Cell.Type.Empty:
                Flood(cell);
                CheckWinCondition();
                break;

            case Cell.Type.Mine:
                Explode(cell);
                break;
            
            default:
                cell.revealed = true;
                state[cell.position.x, cell.position.y] = cell;
                CheckWinCondition();
                break;
        }

        board.Draw(state);
    }

    private void Flood(Cell cell)
    {
        if (cell.revealed) return;
        if (cell.type == Cell.Type.Mine) return;
        if (cell.type == Cell.Type.Invalid) return;

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if (cell.type == Cell.Type.Empty)
        {
            Flood(GetCell(cell.position.x + 1, cell.position.y));
            Flood(GetCell(cell.position.x - 1, cell.position.y));
            Flood(GetCell(cell.position.x, cell.position.y + 1));
            Flood(GetCell(cell.position.x, cell.position.y - 1));
        }
    }

    private Cell GetCellAtMousePos()
    {
        Vector3 worldPosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition =
            board.Tilemap.WorldToCell(worldPosition);
        return GetCell(cellPosition.x, cellPosition.y);
    }

    private Cell GetCell(int x, int y)
    {
        if (IsValidPos(x, y))
            return state[x, y];
        else return new Cell(); // Default is Invalid.
    }

    private void Explode(Cell cell)
    {
        Debug.Log("Game Over");

        cell.revealed = true;
        cell.exploded = true;

        state[cell.position.x, cell.position.y] = cell;
        EndGame();
    }

    private void EndGame()
    {
        RevealAll();
        gameIsOver = true;
        gameHasStarted = false;
    }

    private void CheckWinCondition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!state[x, y].revealed
                  && state[x, y].type != Cell.Type.Mine)
                    return;
            }
        }

        // If didn't return above, won!
        Win();
    }

    private void Win()
    {
        Debug.Log("Winner!");
        EndGame();
    }

    // Currently only reveals bombs.
    private void RevealAll()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Comment these two lines to reveal all.
                Cell cell = state[x, y];
                if (cell.type != Cell.Type.Mine) continue;
                state[x, y].revealed = true;
            }
        }
    }

    private bool IsValidPos(int x, int y)
    {
        return (
            x >= 0 && x <= width  - 1
         && y >= 0 && y <= height - 1
        );
    }
}
