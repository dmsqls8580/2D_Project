using UnityEngine;
using Random = UnityEngine.Random;

public class RangeWeaponHandler : WeaponHandler
{

    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1;
    public float BulletSize { get { return bulletSize; } }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private float spread;
    public float Spread { get { return spread; } }

    [SerializeField] private int numberofProjectilesPerShot;
    public int NumberofProjectilesPerShot { get { return numberofProjectilesPerShot; } }

    [SerializeField] private float multipleProjectilesAngel;
    public float MultipleProjectilesAngel { get { return multipleProjectilesAngel; } }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    private ProjectileManager projectileManager; // 투사체를 발사하는 매니저 참조
    protected override void Start()
    {
        base.Start();
        projectileManager = ProjectileManager.Instance;
    }


    public override void Attack()
    {
        base.Attack();

        float projectilesAngleSpace = multipleProjectilesAngel;
        int numberOfProjectilesPerShot = numberofProjectilesPerShot;

        float minAngle = -(numberOfProjectilesPerShot / 2f) * projectilesAngleSpace;


        for (int i = 0; i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        // 회전된 방향 벡터 계산 후 투사체 발사 요청
        projectileManager.ShootBullet(
            this,                                 // 어떤 무기에서 발사했는지 (RangeWeaponHandler)
            projectileSpawnPosition.position,     // 총알이 나올 위치
            RotateVector2(_lookDirection, angle)  // 방향 벡터 회전 (탄 퍼짐, 확산 등 구현에 사용)
        );
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
