using System;
using System.Collections.Generic;

class userDelegateResultValue{
  delegate string RevaleDelegate(int i,double g);
  static void Main(string[] args){
    Func<int,double,string> f = (i,g) => { 
      return string.Format("1:{1} * {0} = {2}",i,g,g*i);
    };
    Func<int,double,string> k = (i,g) => { 
      return string.Format("2:{1} * {0} = {2}",i,g,g*i);
    };
    Func<int,double,string> m = (i,g) => { 
      return string.Format("3:{1} * {0} = {2}",i,g,g*i);
    };
    
    RevaleDelegate funcs;
    funcs = new RevaleDelegate(f);
    funcs += new RevaleDelegate(k);
    funcs += new RevaleDelegate(m);
    //Get Lst result only!
    var res = funcs(10,11.001);
    Console.WriteLine(res);
    //Processing Delegates!
    string[] results = Array.ConvertAll<Delegate, string>
      ( funcs.GetInvocationList(), d =>((RevaleDelegate)d)(10,5.005));
          //delegate( Delegate d ) { return ( ( RevaleDelegate ) d )( 10, 5.005 ); } );

    foreach(var Or in results){
      Console.WriteLine(Or);
    }
  }
}
