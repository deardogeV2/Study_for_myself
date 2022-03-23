using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    List<StateBase> listState = new List<StateBase>();

    private StateBase curState; 
    public StateBase CurState
    {
        get { return curState; }
    }
    public void addState(StateBase state)
    {
        if (state == null) return;

        if (listState.Count == 0)
        {
            curState = state;
            listState.Add(state);
        }
        else
        {
            if (!listState.Contains(state))
            {
                listState.Add(state);
            }
        }
    }

    public void delState(EStateName estateName)
    {
        if (estateName == EStateName.None) return;
        for (int i = 0; i < listState.Count; i++)
        {
            if (listState[i].StateName == estateName)
            {
                listState.Remove(listState[i]);
                break;
            }
        }
    }

    public void doTranstion(EStateName estateName)
    {
        if (estateName == EStateName.None) return;

        for (int i = 0; i < listState.Count; i++)
        {
            if (listState[i].StateName == estateName)
            {
                curState.doBeforLeaving();
                curState = listState[i];
                curState.doAfterEnter();
                break;
            }
        }
    }
}
