using System.Text;
using Response;
using Response.Fail;
using UnityEngine;

namespace Request
{
    public static class RequestExtension
    {
        public static byte[] ToBytes(this object value)
        {
            var jsonForm = JsonUtility.ToJson(value);
            return Encoding.UTF8.GetBytes(jsonForm);
        }

        public static bool IsFormValid(IValidatedForm form, out IFailResponse fail)
        {
            fail = null;
            if (form.IsValid(out var description))
            {
                return true;
            }

            fail = new InvalidFormFail(description);
            return false;
        }
        
        public static IResponse GetResponse<TSuccessResponse>(this IRequest request) where TSuccessResponse : IResponse
        {
            return request.TryGetResponse<TSuccessResponse>(out var success) ? success : request.GetFailResponse();
        }
        
        public static bool TryGetResponse<T>(this IRequest request, out T response) where T : IResponse
        {
            response = default;
            var responseValue = request.Body.downloadHandler.text;
            if (responseValue.IsNullOrEmpty()) return false;
            response = JsonUtility.FromJson<T>(responseValue);
            return true;
        }
        
        public static IResponse GetFailResponse(this IRequest request)
        {
            if (!request.Body.error.IsNullOrEmpty())
            {
                return UnityFail.CreateFromRequest(request.Body);
            }
            if (request.Body.downloadHandler.text.IsNullOrEmpty())
            {
                return new UnexpectedFail();
            }
            
            return JsonUtility.FromJson<ServerFail>(request.Body.downloadHandler.text);
        }
    }
}