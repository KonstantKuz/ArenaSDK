using System;
using Response;
using UnityEngine;

namespace Registration.ResponseForm
{
    [Serializable]
    public class EmailConfirmationSuccess : IResponse
    {
        public bool ok;
        
        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}