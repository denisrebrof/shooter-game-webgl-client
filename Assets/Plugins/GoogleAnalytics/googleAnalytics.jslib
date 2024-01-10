mergeInto(LibraryManager.library, {
  Send: function(ename) {
      var me = UTF8ToString(ename);
      console.log('GA send event ' + me);
      gtag('event', me);
  },
  
  SendNumArg: function(ename, argName, argValue) {
      var me = UTF8ToString(ename);
	  var ar = UTF8ToString(argName);
	  var arv = UTF8ToString(argValue);
      console.log('GA send event ' + me + ' ' + ar + '=' + arv);
      gtag('event', me, {
		[ar]: parseInt(arv, 10)
	  });
  },
  
  SendStringArg: function(ename, argName, argValue) {
      var me = UTF8ToString(ename);
	  var ar = UTF8ToString(argName);
	  var arv = UTF8ToString(argValue);
      console.log('GA send event ' + me + ' ' + ar + '=' + arv);
      gtag('event', me, {
		[ar]: arv
	  });
  }
});