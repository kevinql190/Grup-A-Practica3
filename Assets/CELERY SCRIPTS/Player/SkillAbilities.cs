using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAbilities : MonoBehaviour
{
    [SerializeField] private GameObject skillCanvas;
    private Image skillImage;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float targetLockAngle;
    private Collider targetLockCollider;
    [SerializeField] private float skillPushSpeed;
    public Cooldown skillTime;
    [SerializeField] private AnimationCurve skillAlphaCurve;
    [SerializeField] private Sprite cursorSprite;
    [Header("Raycast layers")]
    [SerializeField] private LayerMask enemyLayer;
    private Plane raycastPlane;
    public bool isSkillActive = false;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 lookPosition;
    private float targetAngle;
    private Vector3 targetPosition;
    private CookingSystem _cookingSystem;
    private PanController _panController;
    private FoodType CurrentFoodType => GetComponent<PanController>().currentFoodType;
    #region Ability Variables
    [Header("Tomato")]
    [SerializeField] private GameObject tomatoBullet;
    [SerializeField] private float speedBulletTomato = 10f;
    [Header("Carrot")]
    [SerializeField] private GameObject carrotBullet;
    [SerializeField] private float speedBulletCarrot = 10f;
    [Header("Leek")]
    [SerializeField] private GameObject leekWeapon;
    [SerializeField] private float leekDistance;
    [SerializeField] private float leekRadiusDegrees = 90;
    [Header("Mushroom")]
    [SerializeField] private GameObject areaCanvas;
    [SerializeField] private GameObject mushroomBomb;
    [SerializeField] private float areaRadius;
    [SerializeField] private float bombRadius;
    [SerializeField] private float gamepadMoveSpeed;
    #endregion
    private void Start()
    {
        raycastPlane = new(Vector3.up, -0.5f);
        _cookingSystem = GetComponent<CookingSystem>();
        _cookingSystem.OnCookingProgressChanged += ChargeAbility;
        _panController = GetComponent<PanController>();
        skillImage = skillCanvas.GetComponentInChildren<Image>();
        float radius = areaRadius / areaCanvas.GetComponent<Image>().rectTransform.localScale.x * 2;
        areaCanvas.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(radius, radius);
        skillCanvas.SetActive(false);
    }
    #region Handles
    public IEnumerator HandleSkill()
    {
        while(isSkillActive == true)
        {
            if (PlayerInputHandler.AttackJustPressed)
            {
                transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
                GetComponent<PlayerAnimatorManager>().TriggerAnimation("attack");
                spawnPoint.eulerAngles = new Vector3(0, targetAngle, 0);
                Ability();
                _cookingSystem.skillCasted = true;
                skillTime.StartCooldown();
                //yield return StartCoroutine(_playerMovement.ForwardPush(skillPushSpeed, skillTime.CooldownTime));
                SetAbility(false);
            }
            yield return null;
        }
    }
    private void HandleSkillAim()
    {
        if (PlayerInputHandler.SkillAimInput.x != 0) lookPosition.x = PlayerInputHandler.SkillAimInput.x;
        if (PlayerInputHandler.SkillAimInput.y != 0) lookPosition.y = PlayerInputHandler.SkillAimInput.y;
    }
    #endregion
    #region Ability Canvas Set
    public void SetAbility(bool value)
    {
        Cursor.SetCursor(cursorSprite.texture, new Vector2(cursorSprite.texture.width/2, cursorSprite.texture.height/2), CursorMode.Auto);
        skillCanvas.SetActive(value);
        isSkillActive = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        if (value == true)
        {
            SetCanvas();
            StartCoroutine(UpdateSkillCanvas());
            _cookingSystem.skillCasted = false;
        }
    }

    private void SetCanvas()
    {
        skillImage.sprite = _panController.CurrentSkillSprite;
        skillImage.rectTransform.localPosition = new Vector3(0, skillImage.rectTransform.localPosition.y, 0);
        if (CurrentFoodType == FoodType.Mushroom)
        {
            skillImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            float radius = bombRadius / skillImage.rectTransform.localScale.magnitude * 4;
            skillImage.rectTransform.sizeDelta = new Vector2(radius, radius);
        }
        else
        {
            skillImage.SetNativeSize();
            skillImage.rectTransform.pivot = new Vector2(0.5f, 0f);
        }
    }
    #endregion
    #region Skill Canvas Update
    private void ChargeAbility(float value)
    {
        if (value == 0) return;
        value = skillAlphaCurve.Evaluate(value);
        skillCanvas.GetComponentInChildren<Image>().color = new Color(value, value, value, value);
    }
    public IEnumerator UpdateSkillCanvas()
    {
        areaCanvas.SetActive(CurrentFoodType == FoodType.Mushroom);
        while (isSkillActive)
        {
            if (IsUsingGamepad())
            {
                Cursor.lockState = CursorLockMode.Locked;
                HandleSkillAim();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if
                (CurrentFoodType == FoodType.Mushroom) TargetCanvas();
            RotateCanvas();
            yield return null;
        }
        areaCanvas.SetActive(false);
    }
    private void RotateCanvas()
    {
        if (IsUsingGamepad())
        {
            if (Mathf.Abs(lookPosition.x) != 0 && Mathf.Abs(lookPosition.y) != 0)
            {
                targetAngle = -Mathf.Rad2Deg * Mathf.Atan2(-lookPosition.y, -lookPosition.x);
            }
        }
        else
        {
            GetRayPosition();
            targetAngle = Quaternion.LookRotation(lookPosition - transform.position).eulerAngles.y;
        }
        Vector3 direction = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        Ray ray = new(transform.position, direction);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer) && targetLockCollider == null)
        {
            targetLockCollider = hit.collider;
        }
        if (targetLockCollider != null)
        {
            float lockAngle = Quaternion.LookRotation(targetLockCollider.transform.position - transform.position).eulerAngles.y;
            if (Mathf.Abs(targetAngle - lockAngle) < targetLockAngle) targetAngle = lockAngle;
            else targetLockCollider = null;
        }
        spawnPoint.eulerAngles = new Vector3(0, targetAngle, 0);
        skillCanvas.transform.eulerAngles = new Vector3(0, targetAngle, 0);
    }
    private void TargetCanvas()
    {
        targetPosition = transform.position;
        if (IsUsingGamepad())
        {
            Vector2 input = PlayerInputHandler.SkillAimInput;
            input.Normalize();
            Vector2 movement = gamepadMoveSpeed * Time.deltaTime * input;
            Vector3 position = skillImage.rectTransform.position + new Vector3(movement.x, 0, movement.y);
            targetPosition = new Vector3(position.x, skillImage.rectTransform.position.y, position.y);
        }
        else
        {
            GetRayPosition();
            targetPosition = new Vector3(lookPosition.x, skillImage.rectTransform.position.y, lookPosition.z);
        }
        float distance = (targetPosition - transform.position).magnitude;

        if (distance > areaRadius)
        {
            Vector3 borderPos = transform.position + (targetPosition-transform.position).normalized * areaRadius;
            targetPosition = new Vector3(borderPos.x, skillImage.rectTransform.position.y, borderPos.z);
        }
        skillImage.rectTransform.position = targetPosition;
    }
    private void GetRayPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (raycastPlane.Raycast(ray, out var hit))
        {
            Vector3 hitPoint = ray.GetPoint(hit);
            lookPosition = new Vector3(hitPoint.x, 0, hitPoint.z);
        }
    }
    #endregion
    #region Abilities
    private void Ability()
    {
        switch (CurrentFoodType)
        {
            case FoodType.Default:
                break;
            case FoodType.Tomato:
                SkillTomato();
                break;
            case FoodType.Carrot:
                SkillCarrot();
                break;
            case FoodType.Leek:
                StartCoroutine(SkillLeek());
                break;
            case FoodType.Mushroom:
                SkillMushroom();
                break;
        }
    }
    private void SkillTomato()
    {
        AudioManager.Instance.PlaySFXOnce("tomaquet_habilitat");
        InstantiateBullet(tomatoBullet, speedBulletTomato);
    }
    private void SkillCarrot()
    {
        AudioManager.Instance.PlaySFXOnce("pastanaga_habilitat");
        InstantiateBullet(carrotBullet, speedBulletCarrot);
    }
    private IEnumerator SkillLeek()
    {
        AudioManager.Instance.PlaySFXOnce("porro_habilitat");
        GameObject leek = Instantiate(leekWeapon, spawnPoint.transform.position + spawnPoint.forward * leekDistance, spawnPoint.transform.rotation, spawnPoint.transform);
        leek.transform.RotateAround(transform.position, Vector3.up, -leekRadiusDegrees/2f);
        float t = 0;
        while (t < skillTime.CooldownTime)
        {
            float rotationAmount = leekRadiusDegrees / skillTime.CooldownTime * Time.deltaTime;
            if (leek != null) leek.transform.RotateAround(spawnPoint.transform.position, Vector3.up, rotationAmount);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(leek);
    }
    private void SkillMushroom()
    {
        //AudioManager.Instance.PlaySFXOnce("porro_habilitat");
        GameObject mushroom = Instantiate(mushroomBomb, spawnPoint.transform.position + spawnPoint.forward * 1.155f, spawnPoint.transform.rotation, spawnPoint.transform);
        StartCoroutine(mushroom.GetComponent<Bomb>().ChargeBomb(targetPosition, bombRadius));
    }
    private void InstantiateBullet(GameObject prefab, float speedBullet)
    {
        GameObject bulletObj = Instantiate(prefab, spawnPoint.transform.position + spawnPoint.forward * 1.155f, spawnPoint.transform.rotation);
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(spawnPoint.forward * speedBullet, ForceMode.VelocityChange);
    }
    #endregion
    private bool IsUsingGamepad()
    {
        return PlayerInputHandler.CurrentControlScheme != "Keyboard Mouse";
    }
    #region
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
        Gizmos.color = Color.green;
        if(isSkillActive && CurrentFoodType == FoodType.Mushroom) Gizmos.DrawWireSphere(transform.position, bombRadius);
    }
    #endregion
}