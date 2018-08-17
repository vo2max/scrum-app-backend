using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeagullAPI.Models
{
    public class PhotoModel
    {
        public int sequence
        {
            get;set;
        }

        public string source
        {
            get;set;
        }

    }
    
    public class EmailModel
    {
        public string firstName
        {
            get;set;
        }

        public string lastName
        {
            get;set;
        }

        public string email
        {
            get;set;
        }
    }

}