using System.Collections;
using System.Text;
using Authorization;
using Registration;
using Response.Fail;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.Runtime
{
    public class SDKTests
    {
        [UnityTest]
        public IEnumerator RegistrationRequest_WillHaveInvalidFormFail()
        {
            var invalidInput = "*!#1234";
            var request = new UserRegistrationRequest(invalidInput, invalidInput, invalidInput);
            yield return request.Send();
            Debug.Log($"Request result : {(request.Result as InvalidFormFail)?.Message}");
            Assert.IsTrue(request.Result is InvalidFormFail);
        }

        [UnityTest]
        public IEnumerator AuthorizationRequest_WillHaveServerFail()
        {
            var nonexistentLogin = new StringBuilder("nonexistentLogin");
            for (int i = 0; i < 5; i++)
            {
                nonexistentLogin.Append(Random.Range(0, 20).ToString());
            }

            var request = new AuthorizationRequest(nonexistentLogin.ToString(), nonexistentLogin.ToString());
            yield return request.Send();
            Debug.Log($"Request result : {(request.Result as ServerFail)?.Message}");
            Assert.IsTrue(request.Result is ServerFail);
        }
    }
}