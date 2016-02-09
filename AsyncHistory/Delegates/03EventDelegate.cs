using System;

delegate void HelloDelegate(string name);
class Button{
  public event HelloDelegate SomeEvent = _ => {};
  //Call Event
  public void Clicked(string name){
    Console.WriteLine("<Button Clicked>");
    SomeEvent(name);
  }
}
class Foo{
  public void Hello(string name){
    Console.WriteLine("(Method) : Hello {0}",name);
  }
}
class Program{
  static void StaticHello(string name){
    Console.WriteLine("(Static) : Hello {0}",name);
  }
  static void Main(string[] args){
    var foo = new Foo();
    Button btn = new Button();

    btn.SomeEvent += foo.Hello;
    btn.SomeEvent +=  StaticHello;
    btn.SomeEvent += (string name) =>{
      Console.WriteLine("(lambda) : Hello {0}",name);
    };
    btn.Clicked("World !");
  }
}
