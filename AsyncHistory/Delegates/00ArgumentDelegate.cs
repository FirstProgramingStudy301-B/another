using System;
class test{
  //デリゲート型を作成
  delegate void argDelegate(string etc);
  public static void Main(string[] args){
    //デリゲート変数を宣言し代入
    argDelegate DelVariable;
    DelVariable = new argDelegate(ShowMessga);
    //マルチキャストとして追加登録
    DelVariable += new argDelegate(ShowLine);
    //デリゲートを引数として使用
    Action(DelVariable,"Hello World!");
  }

  //これはライブラリ等でメッソッドを引数に取り実行するだけ。
  //実行すべきメソッドは利用者がデリゲートに変換し引数として渡す。
  static void Action(argDelegate del,string message){
    del(message);
  }
  //----------------   Call Delegate Method   --------------------
  static void ShowLine(string msg){
    Console.WriteLine("------------------");
  }
  //----------------   Call Delegate Method   --------------------
  static void ShowMessga(string mes){
    Console.WriteLine(mes);
  }
}
