using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.IdGeneration
{
    /// <summary>
    /// Specifies a type of running number. IRunningNumber will use derived class of this interface to get a certain running number.
    /// </summary>
    /// <example>
    /// public class NumberForReceipt : INumberSpecification
    /// {
    ///     private DateTime date;
    ///     private static string prefix = "RCPT";
    /// 
    ///     public NumberForReceipt(DateTime dateOfPayment)
    ///     {
    ///         date = dateOfPayment;
    ///     }
    /// 
    ///     public string GetKey()
    ///     {
    ///         return String.Format("{0}-{1:00}{2:00}", prefix, date.Month.ToString("00"), date.Year.ToString().Substring(2, 2));
    ///     }
    /// 
    ///     public string Format(int number)
    ///     {
    ///         return GetKey() + "-" + number.ToString("00000");
    ///     }
    /// }
    /// 
    /// Usage:
    ///     var runningNum = new RunningNumberImpl();
    ///     var receiptNum = runningNum.Next(new NumberForReceipt(DateTime.Now));
    ///     
    /// </example>    
    public interface INumberSpecification
    {
        /// <summary>
        /// Get a key that will indicate a group of running numbers.
        /// This key will be used as unique identifier in DB (or other storage mechanism).
        /// </summary>
        /// <returns></returns>
        string GetKey();

        /// <summary>
        /// Format the number and returns a string with necessary prefix/postfix and other formattings. 
        /// </summary>
        /// <param name="number">The running number retrieved from DB (or other storage mechanism)</param>
        /// <returns></returns>
        string Format(int number);
    }
}

