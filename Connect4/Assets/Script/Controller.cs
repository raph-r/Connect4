using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{


    public Canvas SetCell;

    public Text TxtMsg;

    public Dropdown DropdownPlayerOne;

    public Dropdown DropdownPlayerTwo;

    private State.Cell[][] Board;
    private State.Game StateGame;
    private State.Match StateMatch;
    private State.Player StatePlayerOne;
    private State.Player StatePlayerTwo;
    private bool CanPlay;
    
    // Start before the first frame update
    void Start()
    {
        this.Board = new State.Cell[][] {
            new State.Cell[6],
            new State.Cell[6],
            new State.Cell[6],
            new State.Cell[6],
            new State.Cell[6],
            new State.Cell[6],
            new State.Cell[6]
        };
        this.PlayAgain();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.StateMatch == State.Match.InProgress)
        {
            this.ExecuteGameState();
        }
    }

    public void PlayAgain()
    {
        this.StateGame = State.Game.Configure;
        this.ExecuteGameState();
    }

    public void ReConfigureGame()
    {
        this.StateGame = State.Game.Configure;
        if (this.StateMatch != State.Match.InProgress)
        {
            this.StateMatch = State.Match.InProgress;
        }
        this.ExecuteGameState();
    }

    private void ResetBoard()
    {
        for (int x = 0; x < this.Board.Length; x++)
        {
            for (int y = 0; y < this.Board[x].Length; y++)
            {
                this.Board[x][y] = State.Cell.Empty;
                this.SetCell.GetComponentsInChildren<Canvas>()[x + 1].GetComponentsInChildren<Text>()[y + 1].text = "-";
            }
        }
    }

    public void ConfigureGame()
    {
        this.ResetBoard();
        this.TxtMsg.text = "";
        this.StateMatch = State.Match.InProgress;
        this.StatePlayerOne = (State.Player) (int)this.DropdownPlayerOne.value;
        this.StatePlayerTwo = (State.Player) (int)this.DropdownPlayerTwo.value;
        this.DrawPlayer();
        this.CanPlay = true;
    }

    private void ExecuteGameState()
    {
        switch (this.StateGame)
        {
            case State.Game.Configure:
                this.ConfigureGame();
                break;
            case State.Game.TurnOfPlayerOne:
                if (this.CanPlay && this.StatePlayerOne != State.Player.Person)
                {
                    this.MakeMove(GameTree.Execute(this.Board, State.Cell.PlayerOne, this.StatePlayerOne));
                }
                break;
            case State.Game.TurnOfPlayerTwo:
                if (this.CanPlay && this.StatePlayerTwo != State.Player.Person)
                {
                    this.MakeMove(GameTree.Execute(this.Board, State.Cell.PlayerTwo, this.StatePlayerTwo));
                }
                break;
            default:
                break;
        }
    }
    
    public IEnumerator WaitForNextMove()
    {
        this.CanPlay = false;
        yield return new WaitForSeconds(1f);
        this.CanPlay = true;
    }

    private void DrawPlayer()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            this.MsgTurnOfPlayer();
            this.StateGame = State.Game.TurnOfPlayerOne;
        }
        else
        {
            this.MsgTurnOfPlayer(false);
            this.StateGame = State.Game.TurnOfPlayerTwo;
        }
    }
    public void MakeMove(int Column)
    {
        if (this.StateMatch == State.Match.InProgress && this.CanPlay)
        {
            int? Result = Util.TryGetNextEmptyCellInColumn(this.Board[Column]);
            if (Result != null)
            {
                this.UpdateCellState(Column, (int)Result);
            }
            StartCoroutine(this.WaitForNextMove());
        }
    }

    private void UpdateCellState(int Column, int Row)
    {
        this.Board[Column][Row] = (this.StateGame == State.Game.TurnOfPlayerOne) ? State.Cell.PlayerOne : State.Cell.PlayerTwo;
        this.SetCell.GetComponentsInChildren<Canvas>()[Column + 1].GetComponentsInChildren<Text>()[Row + 1].text = this.GetPlayerMark();
        this.UpdateMatchState();
    }

    private string GetPlayerMark()
    {
        return (this.StateGame == State.Game.TurnOfPlayerOne) ? "1" : "2";
    }

    public void UpdateMatchState()
    {
        if (Util.IsWinner(this.Board))
        {
            this.StateMatch = (this.StateGame == State.Game.TurnOfPlayerOne) ? State.Match.PlayerOneWin : State.Match.PlayerTwoWin;
            this.StateGame = State.Game.Finished;
            this.TxtMsg.text = this.FinalMessage();
        }
        else if (Util.IsDraw(this.Board))
        {
            this.StateMatch = State.Match.Draw;
            this.StateGame = State.Game.Finished;
            this.TxtMsg.text = this.FinalMessage();
        }
        else
        {
            this.UpdateGameState();
        }
    }

    private void UpdateGameState()
    {
        if (this.StateGame == State.Game.TurnOfPlayerOne)
        {
            this.StateGame = State.Game.TurnOfPlayerTwo;
            this.MsgTurnOfPlayer(false);
        }
        else
        {
            this.StateGame = State.Game.TurnOfPlayerOne;
            this.MsgTurnOfPlayer();
        }
    }

    private void MsgTurnOfPlayer(bool IsPlayerOne = true)
    {
        if (IsPlayerOne)
        {
            this.TxtMsg.text = "Turn Of Player One";
        }
        else
        {
            this.TxtMsg.text = "Turn Of Player Two";
        }
    }

    private string FinalMessage()
    {
        switch (this.StateMatch)
        {
            case State.Match.PlayerOneWin:
                return "Player One Win!";
            case State.Match.PlayerTwoWin:
                return "Player Two Win!";
            case State.Match.Draw:
                return "Draw!";
            default:
                return "Error";
        }
    }
}
