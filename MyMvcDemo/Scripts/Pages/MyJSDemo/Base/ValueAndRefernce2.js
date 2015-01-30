var obj = { x: 1 };
function foo(o) {
    o = 100;
}
foo(obj);
console.log(obj.x); // 仍然是1, obj并未被修改为100.
function foo2(obj) {
    obj={x:4};
}
foo2(obj);
console.log(obj.x);
var a1;
a1 = 1;
var a2 = 1;
var a3 = "1";
var a2 = [];


var value = 1;
function foo3(o) {
    o.value = 1.1;
}

foo3(value);
console.log(value.value);//值类型没有prop的原因？ 无法完成赋值

var str = "abc";
var s1 = str[0]; // "a"
str[0] = "d";
var s2 = str; // 仍然是"abc";赋值是无效的。没有任何办法修改字符串的内容

var test4 = function() {
    var obj = { x: 1 };
    obj.x = 100;
    var o = obj;
    o.x = 1;
    var s1 = obj.x; // 1, 被修改
    o = true; //这个时候 obj其实没有没改变，但是o变了，从一个“引用类型”变为了“值类型”
    var s3 = o.x; //undefined
    var s2 = obj.x; // 1, 不会因o = true改变
    var s4 = obj; //仍然还是原来的obj
}();

var test5 = function() {
    var obj = { x: 1 };
    obj = true;
    var s2 = obj.x; // 1, 不会因o = true改变
    var s4 = obj; //仍然还是原来的obj
}();

var test6 = function () {
    
    var obj = {};
    var obj2 = new Object();
    var r1 = obj2.__prop__ === Object.prototype;
    var value = "";
   

}();

var test7 = function () {

    var obj = null;



}();

