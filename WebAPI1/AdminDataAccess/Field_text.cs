//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Field_text
    {
        public int Form_id { get; set; }
        public int Field_template_id { get; set; }
        public string Field_text1 { get; set; }
        public int FID { get; set; }
    
        public virtual Evaluated_form Evaluated_form { get; set; }
        public virtual Field_template Field_template { get; set; }
    }
}
