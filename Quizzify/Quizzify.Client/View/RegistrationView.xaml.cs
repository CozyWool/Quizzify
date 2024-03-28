using System.Windows;
using Quizzify.Client.ViewModel;

namespace Quizzify.Client.View;
public partial class RegistrationView : Window
{
    public RegistrationView(RegistrationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}