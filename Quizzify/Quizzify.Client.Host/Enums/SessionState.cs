namespace Quizzify.Client.Host.Enums;

public enum SessionState
{
    InLobby,
    ChoosingPlayer,
    ChoosingQuestion,
    ShowingQuestion,
    WaitingAnswers,
    WaitingMasterVerdict,
    ShowingCorrectAnswer,
    SwitchingRound,
    GameEndingScreen,
}