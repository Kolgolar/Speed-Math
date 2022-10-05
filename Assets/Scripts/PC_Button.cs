using UnityEngine;
using UnityEngine.EventSystems;

public class PC_Button : MonoBehaviour, IPointerClickHandler
{
    public PlayCircuit PlayCircuit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayCircuit)
        {
            PlayCircuit.OnButtonPressed(name);
        }
    }
}
