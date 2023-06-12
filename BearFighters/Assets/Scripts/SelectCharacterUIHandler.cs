using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacterUIHandler : MonoBehaviour
{    
    private GameObject active;

    [SerializeField]
    private GameObject[] character;

    public void Start()
    {
        if (DataManager.Instance != null)
        {
            // Load the character that was chosen the last time
            active = character[DataManager.Instance.LoadData()];
            active.gameObject.SetActive(true);
        }
        else
        {
            FemaleWarrior();
        }
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void FemaleWarrior()
    {
        ResetActive(0); 
    }

    public void Warrior()
    {
        ResetActive(1); 
    }

    private void ResetActive(int index)
    {
        if (active != null)
        {
            active.gameObject.SetActive(false);
        }
        active = character[index];
        active.SetActive(true);

        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveInfo(index);
        }        
    }       
}