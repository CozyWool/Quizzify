using System.Windows;
using Quizzify.Client.ViewModel;

namespace Quizzify.Client.View;

public partial class AuthorizationView : Window
{
    public AuthorizationView(AuthorizationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}