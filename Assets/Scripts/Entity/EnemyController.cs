using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager enemyManager;
    private Transform target; // ������ ���

    [SerializeField] private float followRange = 15f; // Ÿ���� �Ѿư� �ִ� �Ÿ�

    // ���� ������ �� �ʱ�ȭ
    public void Init(EnemyManager enemyManager, Transform target)
    {
        this.enemyManager = enemyManager;
        this.target = target;
    }

    // Ÿ�ٰ��� �Ÿ� ���
    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    // Ÿ���� ���� ���� ���� ���
    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }

    // �� �����Ӹ��� ���� ���� �Ǵ� �� ó��
    protected override void HandleAction()
    {
        // �θ𿡼� ������ �⺻ ���� ó��
        base.HandleAction();

        // ���⳪ Ÿ���� ������ �ƹ� �ൿ�� ���� ����
        if (weaponHandler == null || target == null)
        {
            // ���� ó��
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }

        float distance = DistanceToTarget(); // Ÿ�ٱ��� �Ÿ�
        Vector2 direction = DirectionToTarget(); // Ÿ���� ���� ����

        isAttacking = false; // �⺻������ ���� �� �ƴ�

        // Ÿ���� ���� �Ÿ� �ȿ� ���� ���� ���� ����
        if (distance <= followRange)
        {
            lookDirection = direction; // ���� ��ȯ

            // ���� ��Ÿ� ������ ������ ���
            if (distance <= weaponHandler.AttackRange)
            {
                int layerMaskTarget = weaponHandler.target;

                // �տ� ��ֹ��� ������ Ȯ���ϸ�, ����� �´��� üũ
                RaycastHit2D hit = Physics2D.Raycast(
                    transform.position,
                    direction,
                    weaponHandler.AttackRange * 1.5f, // ��Ÿ����� �ణ ���� �ְ�
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget
                );

                // ���� ������ ����� ��Ȯ�� �¾��� ���� ����
                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                // ���� ���� ���̹Ƿ� ����
                movementDirection = Vector2.zero;
                return;
            }

            // ���� ������ �ƴϹǷ� Ÿ���� ���� �̵�
            movementDirection = direction;
        }

    }

    public override void Death()
    {
        base.Death();
        enemyManager.RemoveEnemyOnDeath(this);
    }

}
