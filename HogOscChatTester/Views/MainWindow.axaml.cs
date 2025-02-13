using Avalonia.Controls;
using Avalonia.ReactiveUI;
using HogOscChatTester.ViewModels;
using ReactiveUI;
using System.Diagnostics;
using System.Threading;

namespace HogOscChatTester.Views;

/// <summary>
/// 
/// </summary>
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    /// <summary>
    /// 
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();

        this.WhenActivated(disposable =>
        {
        });
    }
}
