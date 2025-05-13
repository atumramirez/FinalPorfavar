using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Space Event", menuName = "BoardGame/SpaceEvents/Shop")]
public class ShopSpaceEvent : SpaceEvent
{
    [SerializeField] private ServerUIManager serverUIManager;

    public override void StartEvent(SplineKnotAnimate animator)
    {
        PlayerStats player = animator.GetComponent<PlayerStats>();

        if (player == null)
        {
            animator.Paused = false;
            return;
        }

        // Show prompt
        /*
        ShopPromptUI prompt = Instantiate(shopPromptPrefab);
        prompt.ShowPrompt(() =>
        {
            ShopUI shopUI = Instantiate(shopUIPrefab);
            shopUI.OpenShop(player, () =>
            {
                animator.Paused = false;
            });
        },
            () =>
            {
                animator.Paused = false;
            });
        */

        serverUIManager.ShowShopPromptUI();
    }
}