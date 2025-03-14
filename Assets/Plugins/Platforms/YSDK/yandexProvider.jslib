mergeInto(LibraryManager.library, {

  Purchase: function(id) {
    buy(id);
  },

  AuthenticateUser: function() {
    auth();
  },

  GetUserData: function() {
    getUserData();
  },
  
  InvokeGRA: function () {
      invokeGRA();
  },

  ShowFullscreenAd: function () {
    showFullscreenAd();
  },

  ShowRewardedAd: function(placement) {
    showRewardedAd(placement);
    return placement;
  },

  Hit: function(ename) {
      console.log('Hit');
      addHit(ename);
  },

  OpenWindow: function(link){
    var url = UTF8ToString(link);
      document.onmouseup = function()
      {
        window.open(url);
        document.onmouseup = null;
      }
  },

  GetLang: function() {
    var returnStr = getLang();
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },
  
  GetDeviceType: function() {
	var returnStr = getDeviceType();
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  RequestPlayerId: function() {
    requestPlayerId();
  },
  
  RequestPurchases: function() {
      requestPurchases();
    }
    
});