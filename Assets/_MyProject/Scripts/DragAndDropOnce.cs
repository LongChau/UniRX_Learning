using UniRx;
using UniRx.Triggers; // need UniRx.Triggers namespace for extend gameObejct
using UnityEngine;

public class DragAndDropOnce : MonoBehaviour
{
    void Start()
    {
        // All events can subscribe by ***AsObservable
        this.OnMouseDownAsObservable()
            .SelectMany(_ => this.UpdateAsObservable())
            .TakeUntil(this.OnDestroyAsObservable())
            .Select(_ => Input.mousePosition)
            .Subscribe(x => Debug.Log(x));
    }
}