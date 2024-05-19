using System.Collections;
using UnityEngine;
using System;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    private float currentSpeed;
    private Vector3 _direction;
    private float targetAngle; //Rotació
    private float _currentRotVel; //Rotation Damp
    [SerializeField] private float smoothTime = 0.05f; //Rotation Damp
    [Header("Dash")]
    [SerializeField] private Cooldown dashCooldown;
    [SerializeField] private float dashSpeed = 16f;
    [SerializeField] private float dashDistance = 0.3f;
    [SerializeField] private float dashLandRadius = 0.5f;
    [SerializeField] private int dashMaxCharges = 3; // 3 Dashes Definitivament
    [SerializeField] private float dashChargeCooldown = 2;
    public event Action<bool> OnDashStart;
    public bool IsDashing
    {
        get { return isDashing; }
        set { isDashing = value; OnDashStart?.Invoke(value); }
    }
    private bool isDashing;
    [SerializeField] private bool canDash = true; //Para debug
    public event Action<float> OnDashChargeChanged;
    public float CurrentDashCharges
    {
        get { return _dashCharges; }
        set { _dashCharges = value; OnDashChargeChanged?.Invoke(value); }
    }
    private float _dashCharges;
    private bool isDashCharging;
    [SerializeField] private VisualEffect dashParticle;
    [SerializeField] private Material dashMaterial;
    [SerializeField] private AnimationCurve dashCurve;
    [Header("Dash Trail")]
    [SerializeField] private float meshRefreshRate = 0.2f;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] private Material dashTrailMat;
    [SerializeField] private float meshDestroyDelay = 3f;
    [SerializeField] string shaderVarRef;
    [Header("Gizmos")]
    [SerializeField] private bool moveGizmo;
    [SerializeField] private bool dashGizmo;

    private NavMeshAgent agent;
    private GameObject MainCamera => GameObject.FindGameObjectWithTag("MainCamera");
    //private PlayerCombat combat;
    private SkillAbilities abilities;
    [SerializeField] private CapsuleCollider capsule;
    private void Awake()
    {
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        dashMaterial.SetFloat("_Controlador", 0);
        agent = GetComponent<NavMeshAgent>();
        abilities = GetComponent<SkillAbilities>();
    }
    private void Start()
    {
        currentSpeed = moveSpeed;
        StartCoroutine(DashRecharge());
    }

    private void Update()
    {
        PlayerTranslate();
        if (abilities.skillTime.IsCoolingDown) return;
        HandleDash();
        ProjectInputVector();
        if (_direction.sqrMagnitude == 0) return;
        PlayerRotate();
    }
    #region Player Run
    private void ProjectInputVector()
    {
        Vector3 cameraForward = Vector3.Scale(MainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        if (IsDashing && PlayerInputHandler.MoveInput.sqrMagnitude == 0) return;
        _direction = PlayerInputHandler.MoveInput.x * MainCamera.transform.right + PlayerInputHandler.MoveInput.y * cameraForward;
    }

    private void PlayerTranslate()
    {
        if (!GetComponent<NavMeshAgent>().enabled) return;
        agent.Move(currentSpeed * Time.deltaTime * _direction);
    }
    private void PlayerRotate()
    {
        //if (!IsDashing)
        {
            targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        }
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentRotVel, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    #endregion
    #region Player Dash
    private void HandleDash()
    {
        if (CurrentDashCharges < 1 || dashCooldown.IsCoolingDown || !PlayerInputHandler.DashJustPressed || !canDash) return;
        CurrentDashCharges--;
        if(!isDashCharging) StartCoroutine(DashRecharge());
        StartCoroutine(Dash());
        dashCooldown.StartCooldown();
    }
    IEnumerator Dash()
    {
        IsDashing = true;
        AudioManager.Instance.PlaySFXOnce("dash");
        dashParticle.Play();
        StartCoroutine(DashMaterialChange());
        StartCoroutine(DashTrail());
        Vector3 dashDirection = _direction.sqrMagnitude == 0 ? transform.forward : _direction; //Dash Forward if there is no move input
        Vector3 destination = transform.position + dashDistance * dashDirection; //Calculate destination Position
        bool raycastHit = NavMesh.Raycast(transform.position, destination, out _, NavMesh.AllAreas);
        GetComponent<CapsuleCollider>().isTrigger = true;
        //If no walls, destination has NavMesh and no lineal path to it, then Void Dash
        if (raycastHit && !WillHitWall(destination) && NavMesh.SamplePosition(destination - Vector3.up * capsule.height / 2, out NavMeshHit navmeshhit, dashLandRadius, NavMesh.AllAreas))
        {
            yield return VoidDash(navmeshhit);
        }
        else
        {
            yield return GroundDash();
        }
        GetComponent<CapsuleCollider>().isTrigger = false;
        dashParticle.Stop();
        IsDashing = false;
    }
    private bool WillHitWall(Vector3 newPos)
    {
        Ray ray = new(transform.position, newPos - transform.position);
        if (Physics.Raycast(ray, out RaycastHit raycasthit, dashDistance)) //Raycast possible walls (colliders)
        {
            return raycasthit.collider.CompareTag("Wall");
        }
        return false;
    }
    private IEnumerator VoidDash(NavMeshHit navmeshhit)
    {
        Debug.Log("Void Dash");
        agent.enabled = false;
        Vector3 destination = new(navmeshhit.position.x, transform.position.y, navmeshhit.position.z);
        float t = 0f;
        float dashDistance2 = Vector3.Distance(destination, transform.position);
        Vector3 originalPos = transform.position;
        while (t < dashDistance2 / dashSpeed)
        {
            t += Time.deltaTime;
            float value = t / (dashDistance2 / dashSpeed);
            transform.position = Vector3.Lerp(originalPos, destination, value);
            yield return null;
        }
        agent.enabled = true;
    }
    private IEnumerator GroundDash()
    {
        currentSpeed = dashSpeed;
        float t = 0f;
        while (t < dashDistance / dashSpeed)
        {
            t += Time.deltaTime;
            _direction = _direction.sqrMagnitude == 0 ? transform.forward : _direction;
            yield return null;
        }
        _direction = _direction.sqrMagnitude == 0 ? Vector2.zero : _direction;
        currentSpeed = moveSpeed;
    }
    private IEnumerator DashRecharge()
    {
        isDashCharging = true;
        while (CurrentDashCharges < dashMaxCharges)
        {
            CurrentDashCharges += Time.deltaTime / dashChargeCooldown;
            yield return null;
        }
        isDashCharging = false;
    }
    private IEnumerator DashMaterialChange()
    {
        float t = 0;
        dashMaterial.SetFloat("_Type", 0);
        while (isDashing)
        {
            t += Time.deltaTime;
            float value = dashCurve.Evaluate(t);
            if (dashMaterial != null) dashMaterial.SetFloat("_Controlador", value);
            yield return null;
        }
        dashMaterial.SetFloat("_Controlador", 0);
    }
    #endregion
    #region Dash Trail
    private IEnumerator DashTrail()
    {
        while (IsDashing)
        {
            foreach (SkinnedMeshRenderer skinMesh in skinnedMeshRenderers)
            {
                GameObject gObj = new();
                gObj.transform.SetPositionAndRotation(transform.position, transform.rotation);

                var renderer = gObj.AddComponent<MeshRenderer>();
                var filter = gObj.AddComponent<MeshFilter>();
                Mesh mesh = new();
                skinMesh.BakeMesh(mesh);

                filter.mesh = mesh;
                renderer.material = dashTrailMat;

                StartCoroutine(AnimateMaterialFloat(renderer.material));

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
    }
    private IEnumerator AnimateMaterialFloat(Material mat)
    {
        float t = 0;
        while (t < meshDestroyDelay)
        {
            t += Time.deltaTime;
            mat.SetFloat(shaderVarRef, t/meshDestroyDelay);
            yield return null;
        }
    }
    #endregion
    public IEnumerator StunPlayer(float time, float speedDecrease)
    {
        currentSpeed -= speedDecrease;
        canDash = false;
        yield return new WaitForSeconds(time);
        canDash = true;
        currentSpeed += speedDecrease;
    }
    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        // Draw a capsule at pointX with the same dimensions as the CapsuleCollider
        if (moveGizmo)
        {
            Gizmos.color = Color.blue;
            Vector3 newPos = transform.position + moveSpeed * transform.forward - Vector3.up * capsule.height / 2;
            Gizmos.DrawLine(transform.position - Vector3.up * capsule.height / 2, newPos);
            Gizmos.DrawWireSphere(newPos, capsule.radius);
        }
        if (dashGizmo)
        {
            Gizmos.color = Color.red;
            Vector3 newPos = transform.position + dashDistance * transform.forward - Vector3.up * capsule.height / 2;
            Gizmos.DrawLine(transform.position - Vector3.up * capsule.height / 2, newPos);
            Gizmos.DrawWireSphere(newPos, dashLandRadius);
        }
    }
    #endregion
}
