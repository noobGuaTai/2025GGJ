using UnityEngine;

public class DoorTrader : MonoBehaviour
{
    [Header("Need Set")]
    public int needCoin;
    [Header("Not Set")]
    public bool deal;
    public bool Trade()
    {
        var playerInv = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        if (playerInv.coins >= needCoin)
        {
            playerInv.coins -= needCoin;
            return TradeSuccess();
        }
        return TradeFailed();
    }
    public bool TradeFailed()
    {
        return false;
    }
    public bool TradeSuccess()
    {
        deal = true;
        return true;
    }
}
