using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Space Event", menuName = "BoardGame/SpaceEvents/Shop")]
public class ShopSpaceEvent : SpaceEvent
{
    [SerializeField] private ShopUI shopUIPrefab;
    [SerializeField] private ShopPromptUI shopPromptPrefab;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        PlayerStats player = animator.GetComponent<PlayerStats>();

        if (player == null)
        {
            // No player or inventory full - skip shopping
            animator.Paused = false;
            return;
        }

        // Show prompt
        ShopPromptUI prompt = Instantiate(shopPromptPrefab);
        prompt.ShowPrompt(() =>
        {
            // Yes: Enter shop
            ShopUI shopUI = Instantiate(shopUIPrefab);
            shopUI.OpenShop(player, () =>
            {
                animator.Paused = false; // Resume after shop
            });
        },
        () =>
        {
            // No: Continue movement
            animator.Paused = false;
        });
    }
}