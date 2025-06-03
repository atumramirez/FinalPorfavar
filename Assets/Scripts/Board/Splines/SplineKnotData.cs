using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.Splines;
using System.Collections;
using UnityEditor;

public class SplineKnotData : NetworkBehaviour
{
    public SplineKnotIndex knotIndex;

    [HideInInspector] public UnityEvent<int> OnLand;

    [SerializeField] private bool pauseMovement = false;
    [SerializeField] public bool skipStepCount = false;

    public enum SpaceType {None, Full, Passing}
    private readonly SpaceType spaceType = SpaceType.Full;

    public enum Event {None, Coin, Trap, Gamble, Showtime, Item, Shop}
    [SerializeField] private Event eventSpace = Event.None;

    private SpaceEvent spaceEvent;

    private void Start()
    {
       spaceEvent = gameObject.GetComponent<SpaceEvent>();
       pauseMovement = spaceEvent.PauseMovement();
       skipStepCount = spaceEvent.SkipStepCount();
    }

    public void EnterKnot(SplineKnotAnimate splineKnotAnimator)
    {
        splineKnotAnimator.Paused = pauseMovement;

        if (spaceEvent != null && spaceType == SpaceType.Passing)

            spaceEvent.StartEvent(splineKnotAnimator);
    }

    public void Land(SplineKnotAnimate splineKnotAnimator)
    {
        if (spaceEvent != null && spaceType == SpaceType.Full)

            spaceEvent.StartEvent(splineKnotAnimator);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            if (this == null) return; 
            ApplyEventScriptEditor();
        };
    }

    private void ApplyEventScriptEditor()
    {
        RemoveEventComponentsEditor();

        switch (eventSpace)
        {
            case Event.Coin:
                gameObject.AddComponent<CoinSpace>();
                break;
            case Event.Trap:
                gameObject.AddComponent<TrapSpace>();
                break;
            case Event.Item:
                gameObject.AddComponent<ItemSpace>();
                break;
            case Event.Shop:
                gameObject.AddComponent<ShopSpace>();
                break;
        }
    }

    private void RemoveEventComponentsEditor()
    {
        var toRemove = new System.Type[]
        {
            typeof(CoinSpace), typeof(TrapSpace), typeof(ItemSpace), typeof(ShopSpace)
        };

        foreach (var type in toRemove)
        {
            var comp = GetComponent(type);
            if (comp != null)
                DestroyImmediate(comp, true);
        }
    }
#endif
}

