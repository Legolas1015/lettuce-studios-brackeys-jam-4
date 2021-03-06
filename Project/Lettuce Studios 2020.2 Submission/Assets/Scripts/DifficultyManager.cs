﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public GameObject Point, Player, Disc, Knob, RewindText;
    public AudioSource RewindSound;
    public TextMeshProUGUI timelefttext;

    public bool rewind, invertControls, allowControlInversion;
    [SerializeField] float rewindDelay;
    [SerializeField] bool randomRewind;
    [SerializeField] [Tooltip("smaller = faster; ex: 0.5 = once every half second")] float _spawnRate;
    [SerializeField] [Tooltip("x = Catnip, y = thumbtac, z = yarnball")] Vector3 spawnRatio;
    [SerializeField] float _discSpeed = 200;
    [SerializeField] float survivalDuration;

    private float nextRewindTime, nextLevelTime, timeleft;
    DiscSpinner spinner;
    BoxSpawner spawner;
    PlayerMovement movement;

    private void Start()
    {
        nextRewindTime = Time.timeSinceLevelLoad + rewindDelay;
        nextLevelTime = Time.timeSinceLevelLoad + survivalDuration;
        invertControls = false;

        spawner = Point.GetComponent<BoxSpawner>();
        spawner.spawnRate = _spawnRate;

        spinner = Disc.GetComponent<DiscSpinner>();
        spinner.SetRatio(spawnRatio);
        spinner.speed = _discSpeed;

        movement = Player.GetComponent<PlayerMovement>();
        timeleft = survivalDuration;
    }

    IEnumerator SetDefaults()
    {
        

        yield return new WaitForSeconds(2);
    }

    //IEnumerator Rewind()
    //{
    //    while (true)
    //    {

    //    }
    //}

    private void Update()
    {
        timeleft -= Time.deltaTime;
        timelefttext.SetText(Mathf.RoundToInt(timeleft).ToString());
        
        //Rewind Mechanic
        if (!randomRewind)
        {
            //Time based rewind
            if (Time.timeSinceLevelLoad >= nextRewindTime) //if time to rewind, then rewind
            {
                rewind = !rewind;
                if (allowControlInversion)
                {
                    invertControls = !invertControls;
                    if (invertControls) movement.InvertControls();
                }
                //RewindText.SetActive(RewindText.activeInHierarchy);
                spinner.Reverse();
                nextRewindTime += rewindDelay;
            }
        }
        else
        {
            //Randomized Delay based rewind
            rewindDelay = UnityEngine.Random.Range(3, 10);

            if (Time.timeSinceLevelLoad >= nextRewindTime) //if time to rewind, then rewind
            {
                rewind = !rewind;
                if (allowControlInversion) 
                {
                    invertControls = !invertControls;
                    if(invertControls) movement.InvertControls();
                }
                //RewindText.SetActive(RewindText.activeInHierarchy);
                spinner.Reverse();
                nextRewindTime += rewindDelay;
            }
        }

        if (!rewind)
        {
            RewindText.SetActive(true);
        }
        else
        {
            RewindSound.Play();
            RewindText.SetActive(false);
        }

        //if (invertControls)
        //{
        //    movement.InvertControls();
        //}

        //Scene Transition
        if (Time.timeSinceLevelLoad >= survivalDuration)
        {
            SwitchToNextScene();
        }
    }

    private void SwitchToNextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex < 10) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadScene(0);
    }
}
