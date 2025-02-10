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
public class OscDispatcher
{
    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newAddress"></param>
    public void AddAddress(OscAddress newAddress)
    {
        this.Addresses.Add(newAddress);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputAddress"></param>
    /// <returns></returns>
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
