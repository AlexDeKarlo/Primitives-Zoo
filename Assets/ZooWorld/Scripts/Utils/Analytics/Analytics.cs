using UnityEngine;
using Zenject;

public class AnalyticsOldGUI : MonoBehaviour
{
    [Inject] private IAnimalFabric _fabric;

    private int _preyDeathCount;
    private int _predatorDeathCount;
    
    private Rect _windowRect = new Rect(Screen.width - 280, 10, 260, 180);

    void OnEnable()
    {
        if (_fabric == null)
        {
            Debug.LogError("AnalyticsOldGUI: IAnimalFabric не внедрён через Zenject!", this);
            enabled = false;
            return;
        }
        _fabric.OnSpawn += HandleAnimalSpawned;
    }

    void OnDisable()
    {
        if (_fabric != null)
            _fabric.OnSpawn -= HandleAnimalSpawned;
    }

    private void HandleAnimalSpawned(Animal animal)
    {
        if (animal is Predator predator)
            predator.huntBehavior.OnHuntCompleted += HandleHuntCompleted;
    }

    private void HandleHuntCompleted(Animal hunter, Animal prey)
    {
        if (prey is Prey)       _preyDeathCount++;
        else if (prey is Predator) _predatorDeathCount++;
    }

    void OnGUI()
    {
        _windowRect = GUI.Window(0, _windowRect, DrawWindow, "Kill Stats & Camera");
    }

    private void DrawWindow(int id)
    {
        GUI.Label(new Rect(10, 20, 240, 20), $"Prey died: {_preyDeathCount}");
        GUI.Label(new Rect(10, 45, 240, 20), $"Pred died: {_predatorDeathCount}");
        
        if (GUI.Button(new Rect(10, 75, 120, 30), "Front"))
        {
            Camera.main.transform.position = new Vector3(-40f, 15f,  0f);
            Camera.main.transform.eulerAngles = new Vector3(25f,  90f, 0);
        }
        
        if (GUI.Button(new Rect(10, 115, 120, 30), "Isometric"))
        {
            Camera.main.transform.position = new Vector3(-30f, 45f, -30f);
            Camera.main.transform.eulerAngles = new Vector3(45f,  45f,   0f);
        }
        
        if (GUI.Button(new Rect(140, 75, 110, 30), "Top"))
        {
            Camera.main.transform.position = new Vector3(0f, 50f,  0f);
            Camera.main.transform.eulerAngles = new Vector3(90f,  90f,   0f);
        }

        GUI.DragWindow();
    }
}
