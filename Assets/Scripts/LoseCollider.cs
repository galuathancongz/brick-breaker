﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    private readonly string GAME_OVER_SCENE_NAME = "Scenes/GameOver";
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ball triggered an event with the lose collider 
        if (other.name.ToLower() == "ball")
        {
            SceneManager.LoadScene(GAME_OVER_SCENE_NAME);
        }
    }
    
}