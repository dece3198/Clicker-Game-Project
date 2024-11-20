using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private GameObject characterUi;

    public void XButton()
    {
        characterUi.SetActive(false);
    }

    public void CharacterButton()
    {
        characterUi.SetActive(true);
    }
}
