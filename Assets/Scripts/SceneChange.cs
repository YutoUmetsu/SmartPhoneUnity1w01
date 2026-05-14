using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] string ChengeScene;//‘JˆÚæƒV[ƒ“

   public void Onclick()
    {
        //ChengeScene‚ªİ’èÏ‚İ‚È‚ç‘JˆÚ
        if (ChengeScene != null)
        {
            SceneManager.LoadScene(ChengeScene);
        }
        else { Debug.Log("‘JˆÚæ‚ª–¢“o˜^"); }
                
    }
}
