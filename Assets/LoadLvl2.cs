using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLvl2 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SceneManager.LoadScene(2);
    }
}

