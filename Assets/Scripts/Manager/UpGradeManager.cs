using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    public static UpGradeManager instance;
    public UpGradeSlot[] upGradeSlots;

    private void Awake()
    {
        instance = this;
    }
}
