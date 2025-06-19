using UnityEngine;

public class ShopSpace : SpaceEvent
{
    [SerializeField] private ShopLogic shopLogic;

    [SerializeField] private bool pauseMovement = true;
    public bool skipStepCount = true;

    [SerializeField] private SpaceType spaceType = SpaceType.Passing;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        shopLogic.OpenPromptMenu();
    }

    public override bool PauseMovement()
    {
        return pauseMovement;
    }

    public override bool SkipStepCount()
    {
        return skipStepCount;
    }

    public override SpaceType GetSpaceType()
    {
        return spaceType;
    }

    public override void GetSpaceLogic()
    {
        string Tag = "ShopLogic";
        shopLogic = GameObject.Find(Tag).GetComponent<ShopLogic>(); ;   
    }
}