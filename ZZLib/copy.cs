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
    
    public partial class copy
    {
        public copy()
        {
            this.record = new HashSet<record>();
        }
    
        public int id { get; set; }
        public string book { get; set; }
        public sbyte status { get; set; }
    
        public virtual book book1 { get; set; }
        public virtual ICollection<record> record { get; set; }
    }
}
