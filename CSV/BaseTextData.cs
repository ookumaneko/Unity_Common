using UnityEngine;
using System.Collections.Generic;

public class BaseTextData
{
    public string Message { get; set; }

    public BaseTextData()
    {
        Message = "";
    }

    public virtual void Setup(string message)
    {
        Message = message;
    }
}
