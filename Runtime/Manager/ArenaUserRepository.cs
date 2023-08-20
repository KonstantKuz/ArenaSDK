using System;
using Response;
using Response.Fail;
using User;

namespace Manager
{
    internal class ArenaUserRepository : IDisposable
    {
        private readonly ArenaSDKManager _manager;
        private readonly int _maxAttemptCount;
        private readonly Action<UserInfoLoadFail> _failCallback;
        private UserInfo _userInfo;
        private int _attempt;
        private bool _disposed;

        public UserInfo Info => _userInfo;
        
        public ArenaUserRepository(ArenaSDKManager manager, int maxAttemptCount, Action<UserInfoLoadFail> failCallback)
        {
            _manager = manager;
            _maxAttemptCount = maxAttemptCount;
            _failCallback = failCallback;
            _manager.LoadUserInfo(OnLoadUserInfoResponse);
        }

        private void OnLoadUserInfoResponse(IResponse response)
        {
            if(_disposed) return;
            if (response is UserInfo userInfo)
            {
                _userInfo = userInfo;
            }
            else
            {
                _attempt++;
                if (_attempt > _maxAttemptCount)
                {
                    _failCallback?.Invoke(new UserInfoLoadFail());
                    return;
                }

                _manager.LoadUserInfo(OnLoadUserInfoResponse);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}