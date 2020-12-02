using System.Collections;
using System.Collections.Generic;

public static class Util
{
    public static int? TryGetNextEmptyCellInColumn(State.Cell[] Column)
    {
        for (int i = Column.Length - 1; i >= 0; i--)
        {
            if (Column[i] == State.Cell.Empty)
            {
                return i;
            }
        }
        return null;
    }

    public static bool IsDraw(State.Cell[][] Board)
    {
        foreach (var Column in Board)
        {
            if (Util.TryGetNextEmptyCellInColumn(Column) != null)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsWinner(State.Cell[][] Board)
    {
        return Util.IsHorizontalWinner(Board) || Util.IsVerticalWinner(Board) || Util.IsPositiveDiagonalWinner(Board) || Util.IsNegativeDiagonalWinner(Board);
    }

    private static bool IsHorizontalWinner(State.Cell[][] Board)
    {
        for (int x = 0; x < Board.Length - 3; x++)
        {
            for (int y = Board[x].Length - 1; y >= 0; y--)
            {
                if (Board[x][y] != State.Cell.Empty && Board[x][y] == Board[x + 1][y] && Board[x][y] == Board[x + 2][y]  && Board[x][y] == Board[x + 3][y])
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool IsVerticalWinner(State.Cell[][] Board)
    {
        for (int x = 0; x < Board.Length; x++)
        {
            for (int y = Board[x].Length - 1; y >= 3; y--)
            {
                if (Board[x][y] != State.Cell.Empty && Board[x][y] == Board[x][y - 1] &&  Board[x][y] == Board[x][y - 2] && Board[x][y] == Board[x][y - 3])
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool IsPositiveDiagonalWinner(State.Cell[][] Board)
    {
        for (int x = 0; x < Board.Length - 3; x++)
        {
            for (int y = Board[x].Length - 1; y >= 3; y--)
            {   
                if (Board[x][y] != State.Cell.Empty && Board[x][y] == Board[x + 1][y - 1] && Board[x][y] == Board[x + 2][y - 2] && Board[x][y] == Board[x + 3][y - 3])
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool IsNegativeDiagonalWinner(State.Cell[][] Board)
    {
        for (int x = 0; x < Board.Length - 3; x++)
        {
            for (int y = Board[x].Length - 4; y >= 0; y--)
            {
                if (Board[x][y] != State.Cell.Empty && Board[x][y] == Board[x + 1][y + 1] && Board[x][y] == Board[x + 2][y + 2] && Board[x][y] == Board[x + 3][y + 3])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static int Evaluate(State.Cell[][] Board, State.Cell StateCell)
    {
        int Score, BestScore;
        Score = BestScore = Util.ScoreHorizontalOccurrence(Board, StateCell);
        
        BestScore += Util.ScoreVerticalOccurrence(Board, StateCell);
        // if (Score > BestScore)
        // {
        //     BestScore = Score;
        // }

        BestScore += Util.ScorePositiveDiagonalWinner(Board, StateCell);
        // if (Score > BestScore)
        // {
        //     BestScore = Score;
        // }

        BestScore += Util.ScoreNegativeDiagonalWinner(Board, StateCell);
        // if (Score > BestScore)
        // {
        //     BestScore = Score;
        // }
        
        return BestScore;
    }

    private static int EvaluateOccurrence(int OccurrenceStateCell)
    {
        switch (OccurrenceStateCell)
        {
            case 2:
                return 2;
            case 3:
                return 5;
            case 4:
                return 100;
            default:
                return 0;
        }
    }

    private static int ScoreHorizontalOccurrence(State.Cell[][] Board, State.Cell StateCell)
    {
        int OccurrenceStateCell, OccurrenceEmpty, Result = 0;
        for (int x = 0; x < Board.Length - 3; x++)
        {
            for (int y = Board[x].Length - 1; y >= 0; y--)
            {
                OccurrenceStateCell = OccurrenceEmpty = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Board[x + i][y] == StateCell)
                    {
                        OccurrenceStateCell++;
                    }
                    else if (Board[x + i][y] == State.Cell.Empty)
                    {
                        OccurrenceEmpty++;
                    }
                }
                if ((OccurrenceStateCell + OccurrenceEmpty) == 4 && OccurrenceStateCell > 1)
                {
                    Result += Util.EvaluateOccurrence(OccurrenceStateCell);
                }
            }
        }
        return Result;
    }

    private static int ScoreVerticalOccurrence(State.Cell[][] Board, State.Cell StateCell)
    {
        int OccurrenceStateCell, OccurrenceEmpty, Result = 0;
        for (int x = 0; x < Board.Length; x++)
        {
            for (int y = Board[x].Length - 1; y >= 3; y--)
            {
                OccurrenceStateCell = OccurrenceEmpty = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Board[x][y - i] == StateCell)
                    {
                        OccurrenceStateCell++;
                    }
                    else if (Board[x][y - i] == State.Cell.Empty)
                    {
                        OccurrenceEmpty++;
                    }
                }
                if ((OccurrenceStateCell + OccurrenceEmpty) == 4 && OccurrenceStateCell > 1)
                {
                    Result += Util.EvaluateOccurrence(OccurrenceStateCell);
                }
            }
        }
        return Result;
    }

    private static int ScorePositiveDiagonalWinner(State.Cell[][] Board, State.Cell StateCell)
    {
        int OccurrenceStateCell, OccurrenceEmpty, Result = 0;
        for (int x = 0; x < Board.Length - 3; x++)
        {
            for (int y = Board[x].Length - 1; y >= 3; y--)
            {
                OccurrenceStateCell = OccurrenceEmpty = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Board[x + i][y - i] == StateCell)
                    {
                        OccurrenceStateCell++;
                    }
                    else if (Board[x + i][y - i] == State.Cell.Empty)
                    {
                        OccurrenceEmpty++;
                    }
                }
                if ((OccurrenceStateCell + OccurrenceEmpty) == 4 && OccurrenceStateCell > 1)
                {
                    Result += Util.EvaluateOccurrence(OccurrenceStateCell);
                }
            }
        }
        return Result;
    }

    private static int ScoreNegativeDiagonalWinner(State.Cell[][] Board, State.Cell StateCell)
    {
        int OccurrenceStateCell, OccurrenceEmpty, Result = 0;
        for (int x = 0; x < Board.Length - 3; x++)
        {
            for (int y = Board[x].Length - 4; y >= 0; y--)
            {
                OccurrenceStateCell = OccurrenceEmpty = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Board[x + i][y + i] == StateCell)
                    {
                        OccurrenceStateCell++;
                    }
                    else if (Board[x + i][y + i] == State.Cell.Empty)
                    {
                        OccurrenceEmpty++;
                    }
                }
                if ((OccurrenceStateCell + OccurrenceEmpty) == 4 && OccurrenceStateCell > 1)
                {
                    Result += Util.EvaluateOccurrence(OccurrenceStateCell);
                }
            }
        }
        return Result;
    }
}