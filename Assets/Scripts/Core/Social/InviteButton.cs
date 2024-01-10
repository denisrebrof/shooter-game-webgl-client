
using UnityEngine;
using UnityEngine.UI;
#if VK_SDK
using Plugins.Platforms.VKSDK;
#endif

namespace Core.Social
{
    [RequireComponent(typeof(Button))]
    public class InviteButton : MonoBehaviour
    {
        private void Start() => GetComponent<Button>().onClick.AddListener(Invite);

        private static void Invite()
        {
#if VK_SDK
            VKSDK.instance.ShowInvite();
#endif
        }
    }
}