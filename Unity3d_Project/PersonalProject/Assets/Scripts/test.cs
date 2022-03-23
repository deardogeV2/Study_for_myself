using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace testName
{
    public class IScenesState
    {
        private string m_StateName = "ISceneState";

        public string StateName
        {
            get { return m_StateName; }
            set { m_StateName = value; }
        }

        protected SceneStateC m_Controller = null; // SceneStateC��״̬��������

        public IScenesState()
        {

        }

        public IScenesState(SceneStateC ssc)
        {
            m_Controller = ssc;
        }

        public virtual void StateBegin() { }
        public virtual void StateEnd() { }
        public virtual void StateAction() { }
        public override string ToString()
        {
            return m_StateName;
        }
    }

    public class SceneStateC // �����������ʵ��������Ϊ�����������ǵ���ͬģʽ�����Ծ�û�е���д
    {
        private IScenesState now_State;
        bool isBegin = false;
        List<IScenesState> stateList; // �洢

        public SceneStateC() {
            // ���幹�췽�����Ӷ�����ʵ����ʵ�������������֮��Ϳ����Զ��������е���֪״̬
            // ��ȡ���״̬�ű�
            StateOne a = new StateOne(this); // ��һ��״̬�����ÿ�������ʵ������
            StateTwo b = new StateTwo(this); // �ڶ���״̬
            StateThree c = new StateThree(this); // ������״̬

            // ���״̬����StateList
            stateList.Add(a);
            stateList.Add(b);
            stateList.Add(c);

            if (now_State == null)
            {
                SetState(a.StateName); // �����ǰû��״̬Ĭ������aΪ��ʼ״̬
            }

        } 

        public void SetState(string LoadSceneName) 
            // ����״̬����,����Ϸ�������/��ʼ����ʱ����Ҫ����״̬�ű�
        {
            isBegin = false;
            if (now_State != null)
            {
                // ��һ��״̬��β��ִ�� �߼�
                now_State.StateEnd(); // ����״̬�������õĽ����߼� 
            }
            // ʵ��״̬����
            foreach (IScenesState s in stateList)
            {
                if (LoadSceneName == s.StateName)
                {
                    now_State = s;
                    break;
                }
            }
        }

        public void StateUpdate()
        {
            if (now_State != null && isBegin == false)
            {
                now_State.StateBegin(); // ִ�е�һ������ǰ������
                isBegin = true; // �趨��ǰ״̬�Ѿ�����ִ������
            }
            if (now_State != null)
            {
                now_State.StateAction(); // ִ�и���״̬ʵ������
            }
        }
    }

    public class SceneManager : MonoBehaviour // ״̬������ - ʵ�����ǹ��ص���Ϸ���
    {
        private void Start()
        {
            SceneStateC SSC = new SceneStateC(); // ֱ��ע�������

            SSC.SetState("StateOne"); // ����Ĭ��״̬
        }

        // û���ˣ�ʵ�������Manager��һ��ʵ�����Ʒ���
        // ���ǿ��Կ��źܶ�ӿڸ���Ľű��Ӷ����Ƶ�ǰ״̬
        public void changeState(){ } // ��ʱ��д��
    }
    public class StateOne : IScenesState { 
        // ����״̬ʵ�ʵĴ���
        public StateOne(SceneStateC a):base(a) 
            // �˴���ʵ��״̬��ֻ�̳а��������Ĺ��캯�����Ӷ�ʱ��ʵ��״̬�����Ҫ��������������ʵ����
        {
            this.StateName = "StateOne"; // ���췽���̳�
        }
        public override void StateAction()
        {
            //ִ������ �����߼�����
        }
    }
    public class StateTwo : IScenesState
    {
        public StateTwo(SceneStateC a) : base(a)
        {
            this.StateName = "StateTwo"; // ���췽���̳�
        }
    }
    public class StateThree : IScenesState
    {
        public StateThree(SceneStateC a) : base(a) 
        {
            this.StateName = "StateThree"; // ���췽���̳�
        }
    }

}


