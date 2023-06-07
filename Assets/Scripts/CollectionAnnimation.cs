using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectionAnnimation : MonoBehaviour
{
    private float fadeTime = 1f;
    private TextMeshProUGUI textMesh;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        //Debug.Log(textMesh);
        StartCoroutine(FadeText());
    }
    void Update()
    {
        Vector3 currentPosition = transform.position;
        // calculate the new y position based on the current time and speed
        float newYPosition = currentPosition.y + 50 * Time.deltaTime;
        // set the new position of the TextMeshPro object
        transform.position = new Vector3(currentPosition.x, newYPosition, currentPosition.z);
    }
    private IEnumerator FadeText()
    {
        // Get the starting alpha of the text
        float startAlpha = textMesh.color.a;
        // Set the target alpha to 0 (fully transparent)
        float targetAlpha = 0f;
        // Initialize the timer
        float timer = 0f;
        while (timer < fadeTime)
        {
            // Increment the timer
            timer += Time.deltaTime;
            // Calculate the current alpha based on the timer
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeTime);
            // Set the new alpha on the text
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, currentAlpha);
            // Wait for the next frame
            yield return null;
        }
        // Destroy the object when the fade is complete
        Destroy(gameObject);
    }
}
