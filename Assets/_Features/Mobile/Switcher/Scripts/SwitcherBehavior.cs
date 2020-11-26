using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Gameplay
{
    public class SwitcherBehavior : MonoBehaviour, ISwitchable
    {
        [Header("---References---")]
        public CallableFunction _switch = default;
        public CallableFunction _sendSwitcherChange = default;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private Text timerDisplayer;
        public List<Object> nodes = default;

        [Header("---IMPORTANT---")]
        [Range(0, 1), SerializeField] private int state;
        [Range(0, 1), SerializeField] private int power;
        [Range(0, 10)]
        public float ID = default;
        public enum SwitchTimer { None, Fixed}

        public SwitchTimer switchTimer = default;
        [HideInInspector]
        public float timer = 0;
        private float currentTimer = 0;
        public int State 
        { 
            get { return state; } 
            set { state = value; }
        
        }

        public int Power
        {
            get { return power; }
            set 
            { 
                power = value;
                if (power == 1)
                {
                    if (state == 1) 
                    {
                        TurnOn();
                        
                    }
                    else 
                    { 
                        TurnOff(); 
                    }
                }
                else
                {
                    TurnOff();
                }
            }
        }

        public GameObject MyGameObject { get { return this.gameObject; } set { MyGameObject = value; } }



        private void Start() 
        {
            if (switchTimer == SwitchTimer.Fixed) { timerDisplayer.transform.parent.gameObject.SetActive(true); ResetTimer(); }
            else timerDisplayer.transform.parent.gameObject.SetActive(false);
            

            Power = power;
        }


        public void CallSwitchManager()
        { 
            _switch.Raise(ID);
            _sendSwitcherChange.Raise(ID);
        }

        private void SwitchNode()
        {
            foreach (ISwitchable node in nodes)
            {
                if (node.Power == 1) node.Power = 0;
                else node.Power = 1;
                if (node.MyGameObject.GetComponent<SwitcherBehavior>() != null)
                    node.MyGameObject.GetComponent<SwitcherBehavior>().SwitchChildrens();
            }


        }

        public void TriggerSwitch()
        {
            if(switchTimer == SwitchTimer.Fixed)
            {
                StartCoroutine(DelaySwitchNode());
                DOTween.To(() => currentTimer, x => currentTimer = x, timer, timer).SetEase(Ease.Linear).OnUpdate(SetTimerDisplayer).OnComplete(ResetTimer);

            }
            else
            {

                SwitchNode();
            }

            
        }

        public void SwitchChildrens()
        {

            foreach (ISwitchable node in nodes)
            {
                if (Power == 1)
                {
                    if (node.Power == 1) node.TurnOn();
                    else node.TurnOff();


                }
                else
                {
                    node.TurnOff();
                }

                if (node.MyGameObject.GetComponent<SwitcherBehavior>() != null)
                    node.MyGameObject.GetComponent<SwitcherBehavior>().SwitchChildrens();

            }
        }

        IEnumerator DelaySwitchNode()
        {
            SwitchNode();
            TurnOff();
            yield return new WaitForSeconds(timer);
            SwitchNode();
            TurnOn();
            
            yield break;
        }


        public void SearchReferences()
        {
            if (nodes.Count > 0)
                nodes.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.GetComponent<ISwitchable>() != null)
                {
                    nodes.Add(transform.GetChild(i).gameObject.GetComponent<ISwitchable>() as Object);
                }

            }
        }

        public void TurnOn()
        {
            ChangeSwitch(true);

        }

        public void TurnOff()
        {
            ChangeSwitch(false);
            
        }

        void ChangeSwitch(bool buttonState)
        {
            if (button != null)
                button.interactable = buttonState;
        }

        private void SetTimerDisplayer()
        {
            timerDisplayer.text = ((int)(timer - currentTimer)).ToString() + "s";
        }

        private void ResetTimer() 
        {
            
            currentTimer = 0;
            timerDisplayer.text = timer.ToString() + "s";

        }
    }

}
