mergeInto(LibraryManager.library, {
  // 初始化微信SDK
  WX_Init: function(appIdPtr) {
    const appId = Pointer_stringify(appIdPtr);
    if (typeof wx === 'undefined') {
      console.error('微信环境不可用');
      return;
    }
    window.wxAppId = appId;
    console.log('微信SDK初始化成功，AppID:', appId);
  },

  // 调用微信登录获取code
  WX_Login: function() {
    wx.login({
      success: function(res) {
        if (res.code) {
          // 将code发送到C#
          const jsonStr = JSON.stringify({ code: res.code });
          window.Module.OnReceiveCode(jsonStr);
        } else {
          console.error('获取登录code失败:', res.errMsg);
          window.Module.OnLoginError(res.errMsg);
        }
      },
      fail: function(err) {
        console.error('登录失败:', err.errMsg);
        window.Module.OnLoginError(err.errMsg);
      }
    });
  }
});