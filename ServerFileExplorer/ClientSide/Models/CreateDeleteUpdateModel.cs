using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.Models
{
    class CreateDeleteUpdateModel
    {
        public string path { get; set; }
        public string newItemName { get; set; }
        public Enums typeOfItemSelected { get; set; }
        public Enums typeOfItemToChange { get; set; }
        public ActionToDo action { get; set; }
        public string contentToWrite { get; set; }
    }
}
