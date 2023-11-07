using System.Linq;
using System;

namespace HotelCollection.Web.Helpers
{
    public class BLL
    {
        public static string RandomString(int count) 
        {
            Random random = new Random(); 
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + DateTime.Now.Ticks; 
            return new string(Enumerable.Repeat(chars, count).Select(s => s[random.Next(s.Length)]).ToArray()); 
        }
        public static string GetUniqueReferenceNumber(int digit)
        {
            var random = new Random();
            var chars = DateTime.Now.Ticks + "123456789";
            return new string(Enumerable.Repeat(chars, digit)
                              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
