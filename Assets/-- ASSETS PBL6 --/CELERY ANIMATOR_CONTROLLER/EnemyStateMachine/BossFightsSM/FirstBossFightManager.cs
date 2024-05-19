using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class FirstBossFightManager : MonoBehaviour
{
    [Header("Cinematics")]
    [SerializeField] private GameObject virtualCamera;
    [SerializeField] private GameObject cape;
    [SerializeField] private Transform capePosition;
    private Vector3 capeStartPosition;
    private Vector3 capeStartScale;
    private Quaternion capeStartRotation;
    [SerializeField] private float cinematicsTime;
    private Transform startCamFollow;
    [Header("Wing")]
    [SerializeField] private float wingThrowDelay;
    [SerializeField] private GameObject wingPrefab;
    [SerializeField] private GameObject wingChicken;
    [SerializeField] private CapsuleCollider wingChickenCollider;
    [SerializeField] private Transform wingSpawnPoint;
    private GameObject wing;
    [Header("Head")]
    [SerializeField] private float headThrowDelay;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private GameObject headChicken;
    [SerializeField] private Transform headSpawnPoint;
    private GameObject head;
    [Header("Waves")]
    public WaveManager firstWave;
    public WaveManager secondtWave;
    public WaveManager thirdWave;
    [Header("Eggs")]
    private bool isEggActive = false;
    [SerializeField] private List<WaveManager> waveList;
    [SerializeField] private float roundDelay;
    [SerializeField] private bool areWavesInOrder;
    [SerializeField] private float waveDelay;
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private float delayBetweenEggs;
    [Header("Pilar")]
    [SerializeField] private GameObject pilarPrefab;
    [SerializeField] private Transform pilarSpawnPoint;
    private void Awake()
    {
        startCamFollow = virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow;
        capeStartPosition = cape.transform.position;
        capeStartRotation = cape.transform.rotation;
        capeStartScale = cape.transform.localScale;
    }
    private IEnumerator RoomSequence()
    {
        thirdWave.enabled = true;
        while (!thirdWave.waveEnded) yield return null;
        yield return RoundSequence();
        Instantiate(pilarPrefab, pilarSpawnPoint.position, pilarSpawnPoint.rotation * Quaternion.Euler(0, -90, 0), transform);
        Destroy(wing);
        Destroy(head);
    }
    public void StartCameraSequence(float secondsToWait)
    {
        StartCoroutine(CameraSequence(secondsToWait));
    }
    public IEnumerator CameraSequence(float secondsToWait)
    {
        virtualCamera.SetActive(true);
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
        float t = 0;
        while (t < cinematicsTime)
        {
            t += Time.deltaTime;
            float value = t / cinematicsTime;
            virtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = value;
            cape.transform.SetPositionAndRotation(Vector3.Lerp(capeStartPosition, capePosition.position, value), Quaternion.Lerp(capeStartRotation, capePosition.rotation, value));
            cape.transform.localScale = Vector3.Lerp(capeStartScale, capePosition.localScale, value);
            yield return null;
        }
        yield return new WaitForSeconds(secondsToWait);
        virtualCamera.SetActive(false);
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = startCamFollow;
    }
    public void ThrowWing()
    {
        StartCoroutine(WingSequence());
    }
    private IEnumerator WingSequence()
    {
        yield return new WaitForSeconds(wingThrowDelay);
        wing = Instantiate(wingPrefab, wingSpawnPoint.position, wingSpawnPoint.rotation * Quaternion.Euler(0, -90, 0), transform);
        wingChicken.SetActive(false);
        wingChickenCollider.enabled = false;
    }
    public void ThrowHead()
    {
        StartCoroutine(HeadSequence());
    }
    private IEnumerator HeadSequence()
    {
        yield return new WaitForSeconds(headThrowDelay);
        head = Instantiate(headPrefab, headSpawnPoint.position, headSpawnPoint.rotation * Quaternion.Euler(0, -90, 0), transform);
        headChicken.SetActive(false);
    }
    private IEnumerator RoundSequence()
    {
        isEggActive = true;
        while (isEggActive)
        {
            Debug.Log("START ROUNDDDDD");
            var order = Enumerable.Range(1, waveList.Count).ToList();
            var shuffledRoundOrder = order.OrderBy(x => Random.value).ToList();
            var waves = waveList;
            if (!areWavesInOrder) waves = waves.OrderBy(x => Random.value).ToList();
            for (int i = 0; i < waveList.Count; i++)
            {
                waves[i].enabled = true;
                StartCoroutine(EggSequence(shuffledRoundOrder[i]));
                while (!waves[i].waveEnded) yield return null;
                waves[i].enabled = false;
                i++;
                yield return new WaitForSeconds(waveDelay);
            }
            yield return new WaitForSeconds(roundDelay);
        }
    }

    private IEnumerator EggSequence(int eggCount)
    {
        Debug.Log("spawned " + eggCount + " eggs");
        yield return null;
    }
}
