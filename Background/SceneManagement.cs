using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public AnimationCurve Fade;

    public Image fader;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(int _scene)
    {
        StartCoroutine(FadeOut(_scene));
    }
    public void FadeToString(string _scene)
    {
        StartCoroutine(FadeOutString(_scene));
    }

    IEnumerator FadeIn()
    {
        float progress = 1f;

        while(progress > 0f)
        {
            fader.color = new Color(0, 0, 0, Fade.Evaluate(progress));
            progress -= Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator FadeOut(int _scene)
    {
        float progress = 0f;

        while (progress < 1f)
        {
            fader.color = new Color(0, 0, 0, Fade.Evaluate(progress));
            progress += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(_scene);
    }
    IEnumerator FadeOutString(string _scene)
    {
        float progress = 0f;

        while (progress < 1f)
        {
            fader.color = new Color(0, 0, 0, Fade.Evaluate(progress));
            progress += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(_scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}