using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class TestTriggers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        // Get the plain object
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Add ObservableXxxTrigger for handle MonoBehaviour's event as Observable
        cube.AddComponent<ObservableUpdateTrigger>()
            .UpdateAsObservable()
            .SampleFrame(60)    // run each 30 frame
            .Subscribe(x => Debug.Log("cube"), () => Debug.Log("destroy"));

        cube.AddComponent<DragAndDropOnce>();

        // destroy after 10 second:)
        Destroy(cube, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
