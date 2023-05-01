using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text scoreText;
    [SerializeField]
    Image[] images;
    Image image;

    ParticleSystemForceField particleForceField;
    Coroutine loadingCoroutine;
    Color originalColor;
    int value = 0;
    List<ParticleSystem> particleSystems;
    int score;

    private void Start()
    {
        particleForceField = scoreText.GetComponent<ParticleSystemForceField>();
        particleSystems = new List<ParticleSystem>();
        image = GetComponent<Image>();
        for (int i = 0; i < 10; i++)
        {
            var img = images[i];
            var ps = img.gameObject.GetComponent<ParticleSystem>();
            var imgColor = img.color;
            var col = ps.colorOverLifetime;
            col.color.gradient.colorKeys[0] = new GradientColorKey(imgColor, 0);
            var gradient = new Gradient();
            gradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0.5f), new GradientAlphaKey(0, 1) };
            gradient.colorKeys = new GradientColorKey[] { new GradientColorKey(imgColor, 0), new GradientColorKey(Color.white, 0.8f) };
            col.color = new ParticleSystem.MinMaxGradient(gradient);
            particleSystems.Add(ps);

        }
        originalColor = image.color;
    }

    public void StartGame()
    {
        score = 0;
        scoreText.text = "Score: 0";
    }

    public void StartLoading()
    {
        image.color = originalColor;
        loadingCoroutine = StartCoroutine(RandomizeValue());
    }

    public void AddScore(int toAdd)
    {
        StopCoroutine(loadingCoroutine);
        particleForceField.enabled = false;
        SetValue(toAdd);
        Utils.SetTimeout(this, 3, () =>
        {
            for (int i = 0; i < toAdd; i++)
            {
                particleSystems[i].Play();
                var c = images[i].color;
                Utils.TweenColor(images[i], new Color(c.r, c.g, c.b, 0), 0.3f);
            }
            Utils.SetTimeout(this, 1.5f, () => particleForceField.enabled = true);
            Utils.SetTimeout(this, 2.4f, () =>
             {
                 StartCoroutine(IncreaseTotalScore(toAdd));
                 Utils.TweenColor(image, Color.clear);
             });
        });
    }

    IEnumerator IncreaseTotalScore(int toAdd)
    {
        for (int i = 0; i < toAdd; i++)
        {
            score++;
            scoreText.text = "Score: " + score;
            MusicMixer.instance.PlayEffect(1, 1 + i * 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
        GameManager.Instance.totalScore = score;
    }

    private void SetValue(int newValue)
    {
        value = newValue;
        for (int i = 0; i < 10; i++)
        {
            var c = images[i].color;
            images[i].color = new Color(c.r, c.g, c.b, i < newValue ? 1 : 0);
        }
    }

    IEnumerator RandomizeValue()
    {
        for (; ; )
        {
            var target = UnityEngine.Random.Range(1, 10);
            var diff = (int)Mathf.Sign(target - value);
            while (value != target)
            {
                SetValue(value + diff);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
