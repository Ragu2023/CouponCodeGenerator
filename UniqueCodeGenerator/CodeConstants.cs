
namespace UniqueCodeGenerator
{
    internal class Constants
    {
        /// <summary>
        /// Below mentioned characters are not accepted as they are ambiguous with one-another
        /// Note: Following alphabets are not accepted: "I,L,O,S,U,V,Z"
        /// Note: Following number are not accepted: "0,1,2,5"
        /// </summary>
        internal static readonly char[] AcceptedCodes = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'T', 'W', 'X', 'Y', '3', '4', '6', '7', '8', '9' };
       
        //Swap this value for testing
        //internal static readonly char[] AcceptedCodes = new char[] { 'A', 'B', 'C'};
    }
}
