using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static bool DisplayHat;
    public GameObject hatIcon;

    void Update()
    {
        hatIcon.SetActive(DisplayHat);
    }
}