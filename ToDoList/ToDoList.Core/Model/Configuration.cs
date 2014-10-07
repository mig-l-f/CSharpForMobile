using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoList.Core.Model
{
    public static class Configuration
    {
        public static string DbConnection
        {
            get 
            {
                return "Data Source=isostore:/ToDo.sdf";
            }
        }
    }
}
