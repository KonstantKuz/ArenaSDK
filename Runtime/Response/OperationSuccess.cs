using System;

namespace Response
{
    [Serializable]
    public class OperationSuccess : IResponse
    {
        public bool ok;
    }
}