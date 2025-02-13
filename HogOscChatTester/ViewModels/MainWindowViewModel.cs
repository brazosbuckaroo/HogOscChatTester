using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogOscChatTester.ViewModels;

public class MainWindowViewModel : WindowViewModelBase
{
    public MainViewModel MainViewModel
    {
        get;
    }

    public MainWindowViewModel()
    {
        this.MainViewModel = new MainViewModel(this);

        this.Router.Navigate.Execute(this.MainViewModel);
    }
}
