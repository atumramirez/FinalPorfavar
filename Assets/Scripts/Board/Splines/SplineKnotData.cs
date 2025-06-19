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

    private bool pauseMovement = false;
    [HideInInspector] public bool skipStepCount = false;

    public enum Event {None, Coin, Trap, Gamble, Showtime, Item, Shop, Wall}
    [SerializeField] private Event eventSpace = Event.None;

    [SerializeField] private SpaceEvent spaceEvent;
    private SpaceEvent.SpaceType spaceType;

    private void UpdateValues()
    {
        pauseMovement = spaceEvent.PauseMovement();
        skipStepCount = spaceEvent.SkipStepCount();
        spaceType = spaceEvent.GetSpaceType();
        spaceEvent.GetSpaceLogic();
    }

    public void EnterKnot(SplineKnotAnimate splineKnotAnimator)
    {
        splineKnotAnimator.Paused = pauseMovement;

        if (spaceEvent != null && spaceType == SpaceEvent.SpaceType.Passing)

            spaceEvent.StartEvent(splineKnotAnimator);
    }

    public void Land(SplineKnotAnimate splineKnotAnimator)
    {
        if (spaceEvent != null && spaceType == SpaceEvent.SpaceType.Full)

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
                spaceEvent = gameObject.GetComponent<CoinSpace>();
                UpdateValues();
                break;
            case Event.Trap:
                gameObject.AddComponent<TrapSpace>();
                spaceEvent = gameObject.GetComponent<TrapSpace>();
                UpdateValues();
                break;
            case Event.Item:
                gameObject.AddComponent<ItemSpace>();
                spaceEvent = gameObject.GetComponent<ItemSpace>();
                UpdateValues();
                break;
            case Event.Shop:
                gameObject.AddComponent<ShopSpace>();
                spaceEvent = gameObject.GetComponent<ShopSpace>();
                UpdateValues();
                break;
            case Event.Wall:
                gameObject.AddComponent<WallSpace>();
                spaceEvent = gameObject.GetComponent<WallSpace>();
                UpdateValues();
                break;
        }
    }

    private void RemoveEventComponentsEditor()
    {
        var toRemove = new System.Type[]
        {
            typeof(CoinSpace), typeof(TrapSpace), typeof(ItemSpace), typeof(ShopSpace), typeof(WallSpace)
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

