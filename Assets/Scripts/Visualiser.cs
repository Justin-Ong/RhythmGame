using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualiser : MonoBehaviour
{
    [Header("Audio Analysis")]
    public int sampleSize = 1024;
    public float rms;
    public float rmsRef = 0.1f; // RMS value at 0 dB
    public float db;

    [Header("Visualiser")]
    public GameObject visualiserBarPrefab;
    public int numObjects = 32;
    public float scalingMod = 200;
    public float decreaseRate = 10;

    // Audio analysis
    private AudioSource source;
    public float[] spectrum;
    private float[] samples;

    // Visualiser
    private Transform[] visualiserObjectTransforms;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        samples = new float[sampleSize];
        spectrum = new float[sampleSize];
        visualiserObjectTransforms = new Transform[numObjects];
        for (int i = 0; i < numObjects; i++)
        {
            GameObject visualiserBar = Instantiate(visualiserBarPrefab);
            visualiserBar.transform.position += Vector3.right * i;
            visualiserObjectTransforms[i] = visualiserBar.transform;
        }
    }

    private void Update()
    {
        AnalyseAudio();
        Visualise();
    }

    private void AnalyseAudio()
    {
        source.GetOutputData(samples, 0);

        // Get RMS (Root Mean Square)
        float sum = 0;
        for (int i = 0; i < sampleSize; i++)
        {
            sum += samples[i] * samples[i];
        }
        rms = Mathf.Sqrt(sum / sampleSize);

        // Get loudness in dB
        db = 20 * Mathf.Log10(rms / rmsRef);

        // Get spectrum
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }

    private void Visualise()
    {
        float newScale;
        float sum = 0;
        int samplesPerObject = sampleSize / numObjects;
        int spectrumIndex = 0;

        for (int i = 0; i < numObjects; i++)
        {
            sum = 0;
            for (int j = 0; j < samplesPerObject; j++)
            {
                sum += spectrum[spectrumIndex];
                spectrumIndex++;
            }
            newScale = (sum / samplesPerObject) * scalingMod;

            // Increase scale of object sharply but decrease slowly
            if (visualiserObjectTransforms[i].localScale.y < newScale) {
                visualiserObjectTransforms[i].localScale = Vector3.one + Vector3.up * newScale;
            }
            else {
                if (visualiserObjectTransforms[i].localScale.y > 1)
                {
                    visualiserObjectTransforms[i].localScale -= Vector3.up * decreaseRate * Time.deltaTime;
                }
                else
                {
                    visualiserObjectTransforms[i].localScale = Vector3.one;
                }
            }
        }
    }
}
