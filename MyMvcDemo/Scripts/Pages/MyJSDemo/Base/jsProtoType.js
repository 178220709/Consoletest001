var main = function(windows) {
    var Person = function () {
        this.name = "liyatang";
    };

    var Person2 = {
        name: "liyatang"
    }


    Person.prototype.Say = function () {
        console.log("Person say");
    }
    Person.prototype.Salary = 50000;
    var Programmer = function () { };
    Programmer.prototype = new Person();
    Programmer.prototype.WriteCode = function () {
        console.log("programmer writes code");
    };
    Programmer.prototype.Salary = 500;
    var p = new Programmer();
    p.Say();
    p.WriteCode();

    var re = p.Salary;
    p.Salary = 600;
    var re2= p.Salary;

}();

