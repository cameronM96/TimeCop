using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLvl3 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SceneManager.LoadScene("Neo_Noir_New_York");
    }
}
