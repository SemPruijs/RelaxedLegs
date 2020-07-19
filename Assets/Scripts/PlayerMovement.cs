﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
      //Input
      [Header("Input")]
      private float _moveHorizontal;
      private float _moveVertical;
      private Vector2 _movement;
      
      //Speed
      [Header("Speed")]
      public float moveSpeed;
      public float jumpForce;
      
      //Data
      [Header("Data")]
      public int amountOfJumps;
      private int _jumpsLeft;
      public bool turnAroundAnimation;
      public bool topDown;

      //Components
      [Header("Components")]
      private Rigidbody2D _rb2d;
      private AudioSource _audioSource;
      
      //Sound
      [Header("AudioClips")]
      public AudioClip jumpSound;
      public AudioClip trampolineSound;
      public AudioClip deadSound;

      void Start()
      {
          _rb2d = GetComponent<Rigidbody2D>();
          _audioSource = GetComponent<AudioSource>();
      }
  
      void Update()
      {
          // _moveHorizontal = Input.GetAxis("Horizontal");

          if (topDown)
          {
              _moveVertical = Input.GetAxis("Vertical");   
              
              _movement = new Vector2 (_moveHorizontal, _moveVertical);
          }
          else
          {
              _movement = new Vector2 (_moveHorizontal, 0);
          }
          

          if (!topDown)
          {
              if (Input.GetKeyDown("space") || Input.GetKeyDown("w") || Input.GetKeyDown("up") || Input.GetKeyDown("space"))
              {
                 ActivateJump();
              }
          }
          
          //animation
          if (turnAroundAnimation && GameManager.Instance.state == GameManager.State.InGame)
          {
              if (_moveHorizontal < 0)
              {
                  transform.rotation = Quaternion.Euler(0, 180, 0);
              } else if (_moveHorizontal > 0)
              {
                  transform.rotation = Quaternion.Euler(0, 0, 0);
              }
          }
      }

      private void OnCollisionEnter2D(Collision2D other)
      {
          if (!topDown)
          {
              if (other.gameObject.CompareTag("Ground"))
              {
                  _jumpsLeft = amountOfJumps;
              }

              if (other.gameObject.CompareTag("Spike"))
              {
                  GameManager.Instance.PlayAgain();
                  AudioManager.Instance.PlayAudioClip(deadSound);
                  gameObject.GetComponent<SpriteRenderer>().enabled = false;
              }
              
              if (other.gameObject.CompareTag("Trampoline"))
              {
                  Jump();
                  AudioManager.Instance.PlayAudioClip(trampolineSound);
              }
          }
      }

      private void FixedUpdate()
      {
          if (GameManager.Instance.state == GameManager.State.InGame)
          {
              _rb2d.AddForce (_movement * moveSpeed);
          }
      }

      private void Jump()
      {
          if (GameManager.Instance.state == GameManager.State.InGame)
          {
              _rb2d.AddForce(new Vector2(_rb2d.velocity.x, jumpForce));
          }
      }

      public void ActivateJump()
      {
          if (_jumpsLeft > 0)
          {
              _jumpsLeft--;
              Jump();
              _audioSource.PlayOneShot(jumpSound);
          }
      }
}
