using System;
class test{
  delegate void HelloDelegate(string name);
  class Foo{
    public void Hello(string name){
      Console.WriteLine("(Method) : Hello {0}",name);
    }
  }
  static void StaticHello(string name){
    Console.WriteLine("(Static) : Hello {0}",name);
  }
  // C#1.0
  delegate void Delegate10(string str);
  static void Main(string[] args){
    // C# 1.0
    Delegate10 del10 = new Delegate10(StaticHello);
    del10("This function create new delegatetion of C#.0");
    // C#2.0
    Delegate10 del20 = StaticHello;
    del20("C#2.0からデリゲートの作成はnew演算子を使わなくでも代入できるのだ！");
    
    var foo = new Foo();
    // Declare delegate
    HelloDelegate funcs;
    funcs = foo.Hello;
    funcs += StaticHello;
    // C#3.0 lambda Expresstion
    funcs += (string name) => {
      Console.WriteLine("(lambda) : Hello {0}",name);
    };
    // C#3.0 組込delegate型
    Action<string> tlambda = str => { Console.WriteLine(str);};
    // Castting To HelloDelegate 
    funcs += new HelloDelegate( tlambda);
    // NG : funcs += (HelloDelegate) tlambda;
    //型推論で省略できる。
    funcs += str => {Console.WriteLine(str);};
    // C#2.0 Anonymouse Method
    funcs += delegate(string name){
      Console.WriteLine("(delegate) : Hello {0}",name);
    };
    funcs("World");
    }
}
