namespace Quizzify.Client.Host.Enums;

public enum SessionState
{
    //TODO:Проработать состояния
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