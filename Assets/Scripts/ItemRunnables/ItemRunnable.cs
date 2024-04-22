using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRunnable
{
    protected KeyCode key;
    protected Item item;
    protected int count;

    public virtual void Init(Item i)
    {
        // This method will be overridden by the child classes
    }
    public virtual void Run()
    {
        // This method will be overridden by the child classes
    }

    public virtual void Stop()
    {
        // This method will be overridden by the child classes
    }
}