using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer; // ����(�� ��) �浹 ������ ���̾�

    private RangeWeaponHandler rangeWeaponHandler; // �߻翡 ���� ���� ���� ����

    private float currentDuration; // ������� ����ִ� �ð�
    private Vector2 direction; // �߻� ����
    private bool isReady; // �߻� �غ� �Ϸ� ����
    private Transform pivot; // �Ѿ��� �ð� ȸ���� ���� �ǹ�

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestory = true; // �浹 �� ����Ʈ�� �������� ����
    private ProjectileManager projectileManager;

    // �ʱ� ������Ʈ ���� ����
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0); // �ǹ� ������Ʈ (��������Ʈ ȸ����)
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        // ���� �ð� ����
        currentDuration += Time.deltaTime;

        // ������ ���� �ð� �ʰ� �� �ڵ� �ı�
        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        // ���� �̵� ó�� (���� * �ӵ�)
        _rigidbody.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �浹 �� �� �ణ �� ��ġ�� ����Ʈ �����ϰ� �ı�
        if (levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestory);
        }
        // ���� ��� ���̾ �浹���� ���
        else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer)))
        {
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if (resourceController != null)
            {
                resourceController.ChangeHealth(-rangeWeaponHandler.Power);
                if (rangeWeaponHandler.IsOnKnockback)
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if (controller != null)
                    {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }
            }

            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
        }
    }


    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler, ProjectileManager projectileManager)
    {
        this.projectileManager = projectileManager;
        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;

        // ũ�� �� ���� ����
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        // ȸ�� ���� ���� (���� ���� ����)
        transform.right = this.direction;

        // X ���⿡ ���� ��������Ʈ ���Ʒ� ���� (�¿� �߻� ��)
        if (this.direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0);

        isReady = true;
    }

    // ����ü �ı� �Լ�
    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            projectileManager.CreateImpactParticlesAtPostion(position, rangeWeaponHandler);
        }
        Destroy(this.gameObject);
    }
}
