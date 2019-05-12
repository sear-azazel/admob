using Azuma.Manager;
using UnityEngine;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// Shows the interstitial.
    /// </summary>
    public void ShowInterstitial() => AdMobManager.Instance.DisplayInterstitial();
}
