namespace HogOscChatTester.Models.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDispatcher
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
    /// <param name="address"></param>
    void AddAddress(OscAddress address);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputAddress"></param>
    /// <returns></returns>
    bool IsExpectedAddress(string inputAddress);
}
