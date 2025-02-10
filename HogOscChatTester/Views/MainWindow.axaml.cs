using Avalonia.Controls;
using Avalonia.ReactiveUI;
using HogOscChatTester.ViewModels;
using ReactiveUI;

namespace HogOscChatTester.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        this.InitializeComponent();

        this.WhenActivated(disposables => 
        { 
        });
    }
}
