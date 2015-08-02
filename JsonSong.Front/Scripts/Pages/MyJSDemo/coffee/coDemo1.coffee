# CoffeeScript
list = [1, 2, 3, 4, 5]
list2 = _.filter(list,(item)->item%2==0) 
  
mydouble = (x)-> x*2

list3 = (mydouble x for x in list2)

console x.toString() for x in list3