using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;

public class TestTriggers : MonoBehaviour
{
    [SerializeField]
    private Button _testBtn;
    private CompositeDisposable _disposables = new CompositeDisposable();

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        var trigger = _testBtn.GetComponent<ObservableLongPointerDownTrigger>();
        trigger.OnLongPointerDownAsObservable().Subscribe();

        // Get the plain object
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Add ObservableXxxTrigger for handle MonoBehaviour's event as Observable
        var cubeObservable = cube.AddComponent<ObservableUpdateTrigger>()
            .UpdateAsObservable()
            .SampleFrame(60)    // run each 30 frame
            .Subscribe(x => Debug.Log("cube"), () => Debug.Log("destroy"));

        cube.AddComponent<DragAndDropOnce>();

        _disposables.Add(cubeObservable);

        // destroy after 10 second:)
        Destroy(cube, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
        _disposables.Clear();
    }
}
