using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Space Event", menuName = "BoardGame/SpaceEvents")]
public class SpaceEvent : MonoBehaviour
{
    virtual public void StartEvent(SplineKnotAnimate animator)
    {

    }

    virtual public bool PauseMovement()
    {
        return false;
    }

    virtual public bool SkipStepCount()
    {
        return false;
    }

}
