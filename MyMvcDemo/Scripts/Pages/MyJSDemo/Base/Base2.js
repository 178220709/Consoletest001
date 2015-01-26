function extend(subClass, superClass) {
    var F = function () { };
    F.prototype = superClass.prototype;
    subClass.prototype = new F();
    subClass.prototype.constructor = subClass;
    subClass.superclass = superClass.prototype; //加多了个属性指向父类本身以便调用父类函数
    if (superClass.prototype.constructor == Object.prototype.constructor) {
        superClass.prototype.constructor = superClass;
    }
}

var Person = function (name) {
    this.name = name;
    this._name2 = "baseName2";
    console.log("base constructor is call");
};
Person.prototype = {
    getName: function () {
        return this.name;
    }
};

var Chinese = function (name, nation) {
    Person.call(this, name);
    this.nation = nation;
};
extend(Chinese, Person);
Chinese.prototype.getNation = function () {
    return this.nation;
};

var c = new Chinese("liyatang", "China");
console.log(c.getName());

var p = new Person();
var res = (p.__proto__ == Person.prototype);

