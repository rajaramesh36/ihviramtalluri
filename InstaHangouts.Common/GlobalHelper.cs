using InstaHangouts.Common.Constants;
using InstaHangouts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace InstaHangouts.Common
{
  public  class GlobalHelper
    {
      
        public static Header ConstructHeader(string message="")
        {
            Header header = new Header();
            header.ResponseCode = "200";
            header.ResponseMessage = message == "" ? Messages.Succesfully : message;
            return header;
        }

        public static Header ConstructHeader(string message,int code)
        {
            Header header = new Header();
            header.ResponseCode = code.ToString();
            header.ResponseMessage = message;
            return header;
        }

        public static Header ConstructHeader(Exception exception, string message="", int code= (int)System.Net.HttpStatusCode.NotAcceptable)
        {
            Header header = new Header();
            header.ResponseCode = code.ToString();
            InstaHangoutsException ex = exception as InstaHangoutsException;
            if (ex != null)
            {
                /*This means custom Exception*/
                header.ResponseMessage = ex.ExcptionMessage;
                header.Details = message;
            }
            else
            {
                /*General Exception*/
                header.ResponseMessage = message;
                header.Details = GetExceptionDetails(exception);
            }
            return header;
        }


        public static string GetExceptionDetails(Exception ex)
        {
            string message = string.Empty;
            do
            {
                message = message + ex.Message + Environment.NewLine;
                ex = ex.InnerException;
            } while (ex!=null);
            return message;
        }

        public static  string LoadAppSetting(string keyName)
        {
            string value = WebConfigurationManager.AppSettings[keyName];
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"Key {keyName} not found in setting!");
            }
            return value;
        }
    }

    public class InstaHangoutsException : Exception
    {
        public string ExcptionMessage { get; set; }
        public ExceptionType Type { get; set; }
        public InstaHangoutsException() : this(string.Empty, ExceptionType.Other)
        {

        }
        public InstaHangoutsException(string message) : this(message, ExceptionType.Other)
        {

        }

        public InstaHangoutsException(string message, ExceptionType type)
        {
            this.ExcptionMessage = message;
            this.Type = type;
        }
    }

    public enum ExceptionType
    {
        Required,
        DatabaseInconsitency,
        Unexpected,
        Other,
    }
}
