using System.Collections;
using System.Collections.Generic;

public class State
{
    // Estado do resultado
    public enum Match {InProgress, PlayerOneWin, PlayerTwoWin, Draw};

    // Estado das celulas
    public enum Cell {Empty, PlayerOne, PlayerTwo};

    // Estado do jogo
    public enum Game {Configure, TurnOfPlayerOne, TurnOfPlayerTwo, Finished};

    //  Estado Jogador
    public enum Player{Person, AIRandom, AIGameTree};

    //  Estado da jogada
    public enum Move{NotStarted, Started, InDevelopment, AlmostComplete, Complete};
}
