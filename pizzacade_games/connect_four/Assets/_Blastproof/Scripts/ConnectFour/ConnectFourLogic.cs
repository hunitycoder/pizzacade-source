using Photon.Pun;
using System.Linq;
using UnityEngine;

public class ConnectFourLogic : GameLogicBase
{
    private int columns;
    private int rows;

    protected override void OnEnable()
    {
        base.OnEnable();

        columns = _gridSize.x;
        rows = _gridSize.y;
    }

    protected override void PlayerSelectedTile()
    {
        if (!CanPlaceOnThisColumn())
            return;
            
        base.PlayerSelectedTile();
    }

    private bool CanPlaceOnThisColumn()
    {
        int column = GetColumnFromPos(_playerSelectedTile.Value);

        return _gameState[GetPosFromCoordinates(column, 0)] == 0;
    }

    int GetColumnFromPos(int position)
    {
        return position % columns;
    }
    
    int GetPosFromCoordinates(int column, int row)
    {
        return row * columns + column;
    }

    [PunRPC]
    public override void ActivateTile(int position, int state)
    {
        //transform position to check column
        //see if bottom of column is ok

        Debug.Log($"Activating tile at position {position}");

        int column = GetColumnFromPos(position);
        int rowIndex = -1;
        for(int i = rows - 1; i>= 0; i--)
            if(_gameState[GetPosFromCoordinates(column, i)] == 0)
            {
                rowIndex = i;
                break;
            }
        

        if(rowIndex != -1)
        {
            _gameState[GetPosFromCoordinates(column, rowIndex)] = state;
            _buttons[GetPosFromCoordinates(column, rowIndex)].GetComponent<TileHelperBase>().UpdateTile(state);
        }
        
        _canSelectTile.Value = !_canSelectTile.Value;

        CheckCompletion();
    }

    private bool AllValuesEqual(params int[] values)
    {
        if (values == null || values.Length == 0)
            return true;
    
        return values.All(v => v.Equals(values[0]));
    }

    private int[] GetValuesFromGameStateAtPositions(params int[] positions)
    {
        if (positions == null || positions.Length == 0)
            return new int[] {};

        int[] result = new int[positions.Length]; { };

        for (int i = 0; i < positions.Length; i++)
            result[i] = _gameState[positions[i]];

        return result;
    }

    private bool CheckIfSomeoneWonAtPositions(params int[] positions)
    {
        // Debug.Log($"Checking Positions {positions[0]}, {positions[1]}, {positions[2]}, {positions[0]}");
        int[] values = GetValuesFromGameStateAtPositions(positions);

        // Debug.Log($"Got Values{values[0]}, {values[1]}, {values[2]}, {values[3]}");
         return AllValuesEqual(values);
    }

    int CheckRows()
    {
        int result = 0;
        
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns - 3; j++)
            {
                int position = GetPosFromCoordinates(j, i);
                int positionToCheck = _gameState[position];

                if (positionToCheck != 0 && CheckIfSomeoneWonAtPositions(position, position + 1, position + 2, position + 3))
                {
                    result = positionToCheck;
                    return result;
                }
            }

        return result;
    }
    
    int CheckColumns()
    {
        int result = 0;

        for (int i = 0; i < columns; i++)
            for (int j = 0; j < rows - 3; j++)
            {
                int position = GetPosFromCoordinates(i, j);
                int positionToCheck = _gameState[position];

                if (positionToCheck != 0 && CheckIfSomeoneWonAtPositions(position, 
                    GetPosFromCoordinates(i, j + 1), 
                    GetPosFromCoordinates(i, j + 2),
                    GetPosFromCoordinates(i, j + 3)))
                {
                    result = positionToCheck;
                    return result;
                }
            }

        return result;
    }

    int CheckFirstDiagonal()
    {
        int result = 0;

        for (int i = 0; i < columns - 3; i++)
            for (int j = 0; j < rows - 3; j++)
            {
                int position = GetPosFromCoordinates(i, j);
                int positionToCheck = _gameState[position];

                if (positionToCheck != 0 && CheckIfSomeoneWonAtPositions(position,
                    GetPosFromCoordinates(i + 1, j + 1),
                    GetPosFromCoordinates(i + 2, j + 2),
                    GetPosFromCoordinates(i + 3, j + 3)))
                {
                    result = positionToCheck;
                    return result;
                }
            }

        return result;
    }

    int CheckSecondDiagonal()
    {
        int result = 0;

        for (int i = 3; i < columns; i++)
            for (int j = 0; j < rows - 3; j++)
            {
                int position = GetPosFromCoordinates(i, j);
                int positionToCheck = _gameState[position];

                if (positionToCheck != 0 && CheckIfSomeoneWonAtPositions(position,
                    GetPosFromCoordinates(i - 1, j + 1),
                    GetPosFromCoordinates(i - 2, j + 2),
                    GetPosFromCoordinates(i - 3, j + 3)))
                {
                    result = positionToCheck;
                    return result;
                }
            }

        return result;
    }

    bool CheckIfFull()
    {
        return _gameState.All(t => t != 0);
    }

    protected override void CheckCompletion()
    {
        int winner = GetWinnerIfExists();

        if (winner != 0)
        {
            Debug.Log($"Winner {winner}");
            GameEnded(winner);

            return;
        }

        if (CheckIfFull())
        {
            Debug.Log("Draw");
            GameEnded(0);
        }
    
        Debug.Log("No winner");
    }

    private int GetWinnerIfExists()
    {
        int winner = CheckRows();

        if (winner == 0)
            winner = CheckColumns();

        if (winner == 0)
            winner = CheckFirstDiagonal();

        if (winner == 0)
            winner = CheckSecondDiagonal();

        return winner;
    }
    
    protected override void Test()
    {
        Debug.Log($"DASSD AS:----------------------------------------------------------------- ");

        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        
        
        Debug.Assert(CheckRows() == 0, "Error - Check Rows was expected to return 0");
        
        _gameState = new[]
        {
            1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckRows() == 1, "Error - Check Rows was expected to return 1");
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1,
        };
        
        Debug.Assert(CheckRows() == 1, "Error - Check Rows was expected to return 1");
    
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 1, 1, 1, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckRows() == 1, "Error - Check Rows was expected to return 1");
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 1, 1, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckRows() == 0, "Error - Check Rows was expected to return 0");
        
        _gameState = new[]
        {
            1, 0, 0, 1, 0, 0, 0,
            0, 1, 0, 1, 0, 1, 0,
            0, 0, 1, 0, 1, 1, 0,
            0, 0, 1, 1, 0, 0, 0,
            1, 0, 1, 0, 1, 1, 0,
            0, 0, 0, 0, 0, 1, 0,
        };
        
        Debug.Assert(CheckRows() == 0, "Error - Check Rows was expected to return 0");
        Debug.Assert(CheckColumns() == 0, "Error - Check Columns was expected to return 0");
    
    
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
    
        Debug.Assert(CheckColumns() == 0, "Error - Check Columns was expected to return 0");
        
        
        _gameState = new[]
        {
            1, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckColumns() == 1, "Error - Check Columns was expected to return 1");
        
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 1,
            0, 0, 0, 0, 0, 0, 1,
            0, 0, 0, 0, 0, 0, 1,
            0, 0, 0, 0, 0, 0, 1,
        };
        
        Debug.Assert(CheckColumns() == 1, "Error - Check Columns was expected to return 1");
        
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckColumns() == 1, "Error - Check Columns was expected to return 1");
        
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
        };
        
        Debug.Assert(CheckColumns() == 1, "Error - Check Columns was expected to return 1");
        
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckColumns() == 1, "Error - Check Columns was expected to return 1");
        

        _gameState = new[]
        {
            1, 0, 0, 0, 0, 0, 0,
            0, 1, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckFirstDiagonal() == 1, "Error - Check First Diagonal was expected to return 1");
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 1, 0, 0,
            0, 0, 0, 0, 0, 1, 0,
        };
        
        Debug.Assert(CheckFirstDiagonal() == 1, "Error - Check First Diagonal was expected to return 1");
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 1, 0, 0,
            0, 0, 0, 0, 0, 1, 0,
            0, 0, 0, 0, 0, 0, 1,
        };
        
        Debug.Assert(CheckFirstDiagonal() == 1, "Error - Check First Diagonal was expected to return 1");
        
        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            0, 1, 0, 0, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
        };
        
        Debug.Assert(CheckFirstDiagonal() == 1, "Error - Check First Diagonal was expected to return 1");
        
        
        _gameState = new[]
        {
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 1, 0, 0,
            0, 0, 0, 0, 0, 1, 0,
            0, 0, 0, 0, 0, 0, 1,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };
        
        Debug.Assert(CheckFirstDiagonal() == 1, "Error - Check First Diagonal was expected to return 1");

        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 1,
            0, 0, 0, 0, 0, 1, 0,
            0, 0, 0, 0, 1, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };

        Debug.Assert(CheckSecondDiagonal() == 1, "Error - Check Second Diagonal was expected to return 1");

        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 1, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0,
            0, 1, 0, 0, 0, 0, 0,
        };

        Debug.Assert(CheckSecondDiagonal() == 1, "Error - Check Second Diagonal was expected to return 1");


        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0,
            0, 1, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
        };

        Debug.Assert(CheckSecondDiagonal() == 1, "Error - Check Second Diagonal was expected to return 1");


        _gameState = new[]
        {
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 1,
            0, 0, 0, 0, 0, 1, 0,
            0, 0, 0, 0, 1, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
        };

        Debug.Assert(CheckSecondDiagonal() == 1, "Error - Check Second Diagonal was expected to return 1");

        _gameState = new[]
        {
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0,
            0, 1, 0, 0, 0, 0, 0,
            1, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0,
        };

        Debug.Assert(CheckSecondDiagonal() == 1, "Error - Check Second Diagonal was expected to return 1");


        _gameState = new[]
        {
            0, 0, 1, 0, 0, 0, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 1, 1, 1, 1, 0,
            0, 0, 0, 1, 1, 1, 0,
            0, 0, 0, 1, 0, 0, 0,
            0, 0, 1, 0, 0, 0, 0,
        };

        Debug.Assert(CheckSecondDiagonal() == 1, "Error - Check Second Diagonal was expected to return 1");
        Debug.Assert(CheckFirstDiagonal() == 1, "Error - Check First Diagonal was expected to return 1");
        Debug.Assert(CheckRows() == 1, "Error - Check Rows was expected to return 1");
        Debug.Assert(CheckColumns() == 1, "Error - Check Columns was expected to return 1");
    }
}
