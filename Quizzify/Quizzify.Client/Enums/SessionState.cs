namespace Quizzify.Client.Enums;

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