using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSide.Models
{

    public class CreateDeleteUpdateModel
    {
        public string path { get; set; }
        public string newItemName { get; set; }
        public TypeOfItem typeOfItemSelected { get; set; }
        public TypeOfItem typeOfItemToChange { get; set; }
        public ActionToDo action { get; set; }
        public string contentToWrite { get; set; }
    }
}