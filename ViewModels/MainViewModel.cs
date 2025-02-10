using Avalonia.Media;
using HogOscChatTester.Models.Types;
using OscCore;
using OscCore.Address;
using OscCore.LowLevel;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace HogOscChatTester.ViewModels;

/// <summary>
/// 
/// </summary>
public class MainViewModel : ViewModelBase
{
    public string ChatLineOne
    {
        get => this._chatLineOne; 
        set => this.RaiseAndSetIfChanged(ref this._chatLineOne, value);
    }

    public string ChatLineTwo
    {
        get => this._chatLineTwo;
        set => this.RaiseAndSetIfChanged(ref this._chatLineTwo, value);
    }

    public string ChatLineThree
    {
        get => this._chatLineThree;
        set => this.RaiseAndSetIfChanged(ref this._chatLineThree, value);
    }

    public int Port
    {
        get => this._port;
        set => this.RaiseAndSetIfChanged(ref this._port, value);
    }

    public bool IsPortOpen
    {
        get => this._isPortOpen;
        set => this.RaiseAndSetIfChanged(ref this._isPortOpen, value);
    }

    public OscServer Server 
    {
        get; 
        set;
    }

    public ReactiveCommand<Unit, Unit> ChangePortStatus
    {
        get;
    }

    private string _chatLineOne;

    private string _chatLineTwo;

    private string _chatLineThree;

    private int _port;

    private bool _isPortOpen;

    /// <summary>
    /// 
    /// </summary>
    public MainViewModel()
    {
        this.ChangePortStatus = ReactiveCommand.Create(this.ChangePortStatusCommand);
        this.ChatLineOne = string.Empty;
        this._chatLineOne = string.Empty;
        this.ChatLineTwo = string.Empty;
        this._chatLineTwo = string.Empty;
        this.ChatLineThree = string.Empty;
        this._chatLineThree = string.Empty;
        this.Port = 7001;
        this._port = 7001;
        this.IsPortOpen = false;
        this._isPortOpen = false;
        this.Server = new OscServer(this.Port);
        this.Server.OscMessageRecieved += this.Server_OscMessageRecieved!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Server_OscMessageRecieved(object sender, OscMessageRecievedEventArgs e)
    {
        // any null values, no need to report them
        if (e.Message[0].ToString() == null)
        {
            return;
        }
        if (this.Server.Dispatcher.Addresses[0].Match(e.Message.Address))
        {
            this.ChatLineOne = e.Message[0].ToString()!;
        }
        if (this.Server.Dispatcher.Addresses[1].Match(e.Message.Address))
        {
            this.ChatLineTwo = e.Message[0].ToString()!;
        }
        if (this.Server.Dispatcher.Addresses[2].Match(e.Message.Address))
        {
            this.ChatLineThree = e.Message[0].ToString()!;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void ChangePortStatusCommand()
    {
        if (!this.IsPortOpen)
        {
            this.Server.EndConnection();
        }
        else
        {
            this.Server.BeginConnection(this.Port);
        }
    }
}
