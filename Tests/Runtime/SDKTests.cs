using System.Text;
using Authorization;
using NUnit.Framework;
using Registration;
using Response.Fail;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Runtime
{
    public class SDKTests
    {
        [Test]
        public void RegistrationRequest_WillHaveInvalidFormFail()
        {
            var invalidInput = "*!#1234";
            var request = new UserRegistrationRequest(invalidInput, invalidInput, invalidInput);
            
            CoroutineHelper.RunSynchronously(request.Send());

            Debug.Log($"Request result : {(request.Result as InvalidFormFail)?.Message}");
            Assert.IsTrue(request.Result is InvalidFormFail);
        }

        [Test]
        public void AuthorizationRequest_WillHaveUnexpectedFail()
        {
            var nonexistentLogin = new StringBuilder("nonexistentLogin");
            for (int i = 0; i < 5; i++)
            {
                nonexistentLogin.Append(Random.Range(0, 20).ToString());
            }

            var request = new AuthorizationRequest(nonexistentLogin.ToString(), nonexistentLogin.ToString());
            CoroutineHelper.RunSynchronously(request.Send());
            Debug.Log($"Request result : {(request.Result as UnexpectedFail)?.Message}");
            Assert.IsTrue(request.Result is UnexpectedFail);
        }
    }
}