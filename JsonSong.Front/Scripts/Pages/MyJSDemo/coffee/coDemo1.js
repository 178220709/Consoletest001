(function() {
  var list, list2, list3, mydouble, x, _i, _len;

  list = [1, 2, 3, 4, 5];

  list2 = _.filter(list, function(item) {
    return item % 2 === 0;
  });

  mydouble = function(x) {
    return x * 2;
  };

  list3 = (function() {
    var _i, _len, _results;
    _results = [];
    for (_i = 0, _len = list2.length; _i < _len; _i++) {
      x = list2[_i];
      _results.push(mydouble(x));
    }
    return _results;
  })();

  for (_i = 0, _len = list3.length; _i < _len; _i++) {
    x = list3[_i];
    console(x.toString());
  }

}).call(this);

//# sourceMappingURL=coDemo1.js.map
