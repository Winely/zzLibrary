//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZZLibModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class record
    {
        public int id { get; set; }
        public string user { get; set; }
        public int copy { get; set; }
        public System.DateTime borrow_time { get; set; }
        public System.DateTime deadline { get; set; }
        public sbyte renew { get; set; }
        public bool isclosed { get; set; }
        public string @operator { get; set; }
    
        public virtual copy copy1 { get; set; }
        public virtual user user1 { get; set; }
        public virtual user user2 { get; set; }
    }
}