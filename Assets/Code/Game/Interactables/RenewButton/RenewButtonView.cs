using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Interactables.RenewButton
{
    public sealed class RenewButtonView : MonoBehaviour
    {
         public Button Button { get; private set; }
         
         public void Init()
         {
             Button = GetComponent<Button>();
         } 
    }
}