// Classe responsavel por determinar o comportamento da IA
public static class GameTree
{
    public static int Execute(State.Cell[][] OriginalBoard, State.Cell StateCell, State.Player StatePlayer)
    {
        return (StatePlayer == State.Player.AIGameTree) ? GameTree.BestMove(OriginalBoard, StateCell) : GameTree.RandomMove(OriginalBoard);
    }

    // Efetua uma jogada random valida
    private static int RandomMove(State.Cell[][] OriginalBoard)
    {
        int RandomColumn;
        {
            RandomColumn = UnityEngine.Random.Range(0, OriginalBoard.Length);
        }
        while (Util.TryGetNextEmptyCellInColumn(OriginalBoard[RandomColumn]) == null);
        return RandomColumn;
    }

    // Procura o melhor movimento para a arvore, utilizando o algoritmo MiniMax
    private static int BestMove(State.Cell[][] OriginalBoard, State.Cell StateCell, int Depth = 4)
    {
        State.Cell[][] Board = OriginalBoard; 
        int Score, BestMove = 0, BestScore = -10000;
        for (int x = 0; x < Board.Length; x++)
        {
            int? y = Util.TryGetNextEmptyCellInColumn(Board[x]);
            if (y != null)
            {
                Board[x][(int)y] = StateCell;
                Score = GameTree.MiniMax(Board, Depth, false, (StateCell == State.Cell.PlayerOne) ? State.Cell.PlayerTwo : State.Cell.PlayerOne);
                // Peso adicional para a coluna central
                if (x == 3)
                {
                    Score = (Score >= 0) ? Score + 1 : Score - 1;
                }
                Board[x][(int)y] = State.Cell.Empty;
                if (Score > BestScore)
                {
                    BestMove = x;
                    BestScore = Score;
                }
            }
        }
        return BestMove;
    }

    // Aplicação do algoritmo MiniMax
    private static int MiniMax(State.Cell[][] Board, int Depth, bool IsMaximizing, State.Cell StateCell)
    {
        // Valor da jogada anterior
        int Result = Util.Evaluate(Board, (StateCell == State.Cell.PlayerOne) ? State.Cell.PlayerTwo : State.Cell.PlayerOne);
        if (Depth == 0 || Result >= 100) // 100 Valor de vitoria garantida
        {
            return (IsMaximizing) ? Result  * -1: Result;
        }

        int? y = null;
        int BestScore = (IsMaximizing) ? int.MinValue : int.MaxValue;
        int ResultMiniMax;
        for (int x = 0; x < Board.Length; x++)
        {
            y = Util.TryGetNextEmptyCellInColumn(Board[x]);
            if (y != null)
            {
                Board[x][(int)y] = StateCell;
                ResultMiniMax = GameTree.MiniMax(Board, Depth - 1, !IsMaximizing, (StateCell == State.Cell.PlayerOne) ? State.Cell.PlayerTwo : State.Cell.PlayerOne);
                BestScore= (IsMaximizing) ? System.Math.Max(BestScore, ResultMiniMax) : System.Math.Min(BestScore, ResultMiniMax);
                Board[x][(int)y] = State.Cell.Empty;
            }
        }
        return BestScore;
    }
}
