namespace Quizzify.Client.Host.Enums;

public enum SessionState
{
    InLobby,
    InGame,
    ChoosePlayer,
    ChooseQuestion,
    WaitingAnswers,
    CheckAnswer,
    ShowingCorrectAnswer,
    SwitchingRound,
    GameEndingScreen,
}