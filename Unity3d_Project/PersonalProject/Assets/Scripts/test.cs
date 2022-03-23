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

        protected SceneStateC m_Controller = null; // SceneStateC是状态控制器类

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

    public class SceneStateC // 这个控制器其实可以设置为单例，但考虑到不同模式，所以就没有单独写
    {
        private IScenesState now_State;
        bool isBegin = false;
        List<IScenesState> stateList; // 存储

        public SceneStateC() {
            // 定义构造方法，从而可以实现在实例化这个控制器之后就可以自动加入所有的已知状态
            // 获取多个状态脚本
            StateOne a = new StateOne(this); // 第一个状态依赖该控制器的实例生成
            StateTwo b = new StateTwo(this); // 第二个状态
            StateThree c = new StateThree(this); // 第三个状态

            // 多个状态加入StateList
            stateList.Add(a);
            stateList.Add(b);
            stateList.Add(c);

            if (now_State == null)
            {
                SetState(a.StateName); // 如果当前没有状态默认设置a为初始状态
            }

        } 

        public void SetState(string LoadSceneName) 
            // 设置状态方法,在游戏对象挂载/初始化的时候需要设置状态脚本
        {
            isBegin = false;
            if (now_State != null)
            {
                // 上一个状态的尾部执行 逻辑
                now_State.StateEnd(); // 各个状态自身设置的结束逻辑 
            }
            // 实际状态控制
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
                now_State.StateBegin(); // 执行第一次启动前的内容
                isBegin = true; // 设定当前状态已经是在执行中了
            }
            if (now_State != null)
            {
                now_State.StateAction(); // 执行各个状态实际内容
            }
        }
    }

    public class SceneManager : MonoBehaviour // 状态管理器 - 实际上是挂载的游戏组件
    {
        private void Start()
        {
            SceneStateC SSC = new SceneStateC(); // 直接注册控制器

            SSC.SetState("StateOne"); // 设置默认状态
        }

        // 没有了，实际上这个Manager是一个实例控制方法
        // 我们可以开放很多接口给别的脚本从而控制当前状态
        public void changeState(){ } // 暂时不写了
    }
    public class StateOne : IScenesState { 
        // 各个状态实际的代码
        public StateOne(SceneStateC a):base(a) 
            // 此处让实际状态类只继承包含参数的构造函数，从而时间实际状态类必须要给定控制器才能实例化
        {
            this.StateName = "StateOne"; // 构造方法继承
        }
        public override void StateAction()
        {
            //执行内容 各个逻辑操作
        }
    }
    public class StateTwo : IScenesState
    {
        public StateTwo(SceneStateC a) : base(a)
        {
            this.StateName = "StateTwo"; // 构造方法继承
        }
    }
    public class StateThree : IScenesState
    {
        public StateThree(SceneStateC a) : base(a) 
        {
            this.StateName = "StateThree"; // 构造方法继承
        }
    }

}


