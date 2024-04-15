using Quizzify.Client.Model.Users;
using System.Windows;

namespace Quizzify.Client.View;

public class GameLobbyViewModel
{
    private static List<PlayerModel> _players;

    public GameLobbyViewModel(PlayerModel player)
    {
        if (_players.Count <= 5)
        {
            _players.Add(player);
        }
        else
        {
            MessageBox.Show("Lobby is full");
        }

        ReceivePlayersInfo();
        RecievePackageInfo();

        if(_players.Count == 1)
        {
            GrantMaster();
        }
    }

    // Если игрок самый первый в лобби = он ведущий
    private void GrantMaster()
    {
        //
    }

    // Полученеи информации о всех игроках в текущей сессии
    private void ReceivePlayersInfo()
    {
       //
    }

    // Полученеи игроком всего пакета
    private void RecievePackageInfo()
    {
        // 
    }

    // Ведущий стартует игру
    private void StartGame()
    {
        // 
    }

    // Выбор вопроса игроком
    private void ChooseQuestion()
    {

    }

    // Ответ неверный(сказал ведущий)
    private void IncorrectAnswer()
    {

    }

    // Ответ верный(сказал ведущий)
    private void CorrectAnswer()
    {
        //
    }

    // Переключить раунд
    private void SwitchRound()
    {
        // 
    }

    // Игрок решил выйти - отключить его
    private void DisconnectPlayer()
    {
        // 
    }
}