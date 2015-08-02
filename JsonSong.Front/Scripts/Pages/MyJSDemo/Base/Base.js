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


    var Chinese = function (name, nation) {
        //继承，需要调用父类的构造函数，可以用call调用，this指向Chinese
        //使Person在此作用域上，才可以调用Person的成员
        Person.call(this, name);
        this.nation = nation;
    };
    Chinese.prototype = Person.prototype;
    //这里不可和以前一样，因为覆盖掉了prototype属性
    //Chinese.prototype = {
    //  getNation : function(){
    //      return this.nation;
    //  }
    //};
    //以后的方法都需要这样加
    Chinese.prototype.getNation = function () {
        return this.nation;
    };

    var c = new Chinese("liyatang", "China");
    console.log(c.getName());// liyatang

}();

