using UnityEngine;
using UnityEngine.InputSystem;

public class RopeActionController : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private GameObject ropeBridgePrefab;
    [SerializeField] private float ropeRange;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (player.PlayerState != PlayerState.RopeReady) return;
    }

    public void RopeThrowInput(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        ThrowRope();
    }

    private void ThrowRope()
    {
        if (cam == null || player.PlayerState != PlayerState.RopeReady)
            return;

        if()

        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Mathf.Abs(cam.transform.position.z);

        Vector3 world = cam.ScreenToWorldPoint(mousePos);
        world.z = 0f;

        Vector2 dir = ((Vector2)(world - transform.position)).normalized;

        RopeProjectile projectile = Instantiate(projectilePrefab).GetComponent<RopeProjectile>();
        Rope rope = Instantiate(ropePrefab).GetComponent<Rope>();

        projectile.transform.position = transform.position;

        projectile.Launch(rope, player.RopeHolder, dir);

        player.SetPlayerState(PlayerState.None);

    }


}