// FILE: D:/Just IT Training/BillManagment/Classes//Provider.cs

// In this section you can add your own using directives
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D62 begin
// section -64--88-0-12--65b75d98:1535bf612db:-8000:0000000000000D62 end

/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Entities
{
    public class Provider
    {
        // Attributes

        public int ID {get; set; }

        public int AddressId {get; set;}

        public int Source {get; set;}

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Fax { get; set; }

        public string RIB { get; set; }

        public string Comment { get; set; }
    } /* end class Provider */
}