using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public Image[] buffImage;

    private void Awake()
    {
        instance = this;
    }
}
