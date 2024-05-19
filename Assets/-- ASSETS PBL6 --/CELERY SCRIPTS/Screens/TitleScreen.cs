using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
            StartCoroutine(GoToMenu());
    }
    private IEnumerator GoToMenu()
    {
        yield return GetComponent<BlackFade>().FadeToBlack(1.5f);
        SceneManager.LoadScene("Menu");
    }
}
