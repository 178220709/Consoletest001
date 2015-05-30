
//avalon.log(" set baseUrl , current is " + avalon.require.config.baseUrl);
require.config({
    urlArgs: {
       // "*": "v=" + (new Date - 0)
        "*": "v=" + "0.0.0.1"
    },
  
    paths: {
        "libs": "Content/libs",
        "avalon": "Content/libs/avalon",
        "lte": "Content/libs/AdminLTE",
        "public": "Scripts/Public",
        "page": "Scripts/Pages",

        "util": "Scripts/public/util"
    }
});

require(["util"], function (util, b, c, d) {
   //  avalon.log("util is ready ");
    util.setHost(avalon.require.config.baseUrl);
});




