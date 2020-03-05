# CodeGen
拖拽组件连线生成C#代码
这个工程是为了用来以简易方法创建可运行代码所做的；
# 运行逻辑
运行逻辑利用了C#的反射，每个组件实体生成一个方法，方法名由标签和随机数构成，每个生成的方法的返回值包含接下来要调用的方法的名字和输出的值，
输出的值保存在Dictionary中。然后根据返回的方法名字调用接下来的方法，直到返回的方法名字为空。
# 界面

![UI](https://raw.githubusercontent.com/CLOWREAD/CodeGen/master/SharedScreenshot.jpg)

# 生成代码示例

生成的代码如下：


      public void APIMain (dynamic param)
      {
                  System.Collections.Generic.Dictionary<String, Object> result = new Dictionary<string, object>();
                  var this_type = this.GetType();
                  String F_Name_invoking = "Main_ENTITY_D8A5736CBA6E665DB24723CBCA135A35";
                  var method=this_type.GetMethod(F_Name_invoking);
                  dynamic d = new System.Dynamic.ExpandoObject();

                while (F_Name_invoking != null && F_Name_invoking.Length != 0 )
                  {
                      d = new System.Dynamic.ExpandoObject();
                      d.GLOBAL_OUTPUT = result;
                      dynamic invoke_res =method.Invoke(this, new object[]{d});
                      result[F_Name_invoking] = invoke_res;
                      if(invoke_res.FUNCTIONNAMETOINVOKE==null)
                       {
                           break;
                       } 
                      F_Name_invoking = invoke_res.FUNCTIONNAMETOINVOKE;
                      method = this_type.GetMethod(F_Name_invoking);
                  }
      }

      public dynamic Main_ENTITY_15C7E34FC7F061B6502BC2D575FCB94E (dynamic param)
      {
      System.Collections.Generic.Dictionary<String, Object> GLOBAL_OUTPUT = param.GLOBAL_OUTPUT;
      var I_2=((dynamic)GLOBAL_OUTPUT["Main_ENTITY_D8A5736CBA6E665DB24723CBCA135A35"]).O_1;
      dynamic d=new System.Dynamic.ExpandoObject();
      d.FUNCTIONNAMETOINVOKE="";
      return d;
      }
      public dynamic Main_ENTITY_D8A5736CBA6E665DB24723CBCA135A35 (dynamic param)
      {
      System.Collections.Generic.Dictionary<String, Object> GLOBAL_OUTPUT = param.GLOBAL_OUTPUT;

      dynamic d=new System.Dynamic.ExpandoObject();
      d.FUNCTIONNAMETOINVOKE="Main_ENTITY_15C7E34FC7F061B6502BC2D575FCB94E";
      d.O_1="2";
      return d;
      }
