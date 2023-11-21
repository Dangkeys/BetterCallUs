
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjects;
    void Start()
    {
        // Player.Instance.OnSelectedCounterChanged += ChangedCounterVisual;
    }

    private void ChangedCounterVisual(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        Show(e.selectedCounter == baseCounter);
    }

    private void Show(bool isShow)
    {
        foreach(GameObject visualGameObject in visualGameObjects)
        {
            visualGameObject.SetActive(isShow);
        }
    }
}
