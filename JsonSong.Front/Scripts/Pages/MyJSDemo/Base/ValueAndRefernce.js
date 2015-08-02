var main = function(windows) {
    var Person = function () {
        this.name = "liyatang";
    };
    Person.prototype = {
        //可以在这里提供Person的基本功能
        getName: function () {
            return this.name;
        }
    }
    var p1 = new Person();
    p1.name = "MyNameIsP1";

    var age = 18;
    var changePerson = function(p) {
        var p2 = p;
        p2.name = "MyNameIsP2";
       var result1 =  p1.getName();
       var result2 =  p2.getName();
    }(p1);
   

    var changeAge = function (pAge) {
        var a2 = pAge;
        a2 = 16;
        console.log(a2);
        console.log(age);
    }(age);

}();

