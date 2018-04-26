using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLvl1 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SceneManager.LoadScene("Davis_Standard_Venice");
    }
}
