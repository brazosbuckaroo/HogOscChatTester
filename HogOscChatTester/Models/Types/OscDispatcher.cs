using Avalonia.Threading;
using OscCore.Address;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogOscChatTester.Models.Types;

/// <summary>
/// 
/// </summary>
public class OscDispatcher : Models.Interfaces.IDispatcher
{
    /// <inheritdoc/>
    public List<OscAddress> Addresses
    {
        get; 
    }

    /// <summary>
    /// 
    /// </summary>
    public OscDispatcher() 
    { 
        this.Addresses = new List<OscAddress>();
    }

    /// <inheritdoc/>
    public void AddAddress(OscAddress newAddress)
    {
        this.Addresses.Add(newAddress);
    }

    /// <inheritdoc/>
    public bool IsExpectedAddress(string inputAddress)
    {
        foreach (OscAddress address in this.Addresses)
        {
            if (address.Match(inputAddress))
            {
                return true; 
            }
        }

        return false;
    }
}
